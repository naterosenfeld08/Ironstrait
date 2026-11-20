using Ironstrait.Config;
using UnityEngine;

namespace Ironstrait.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        CharacterController _controller;
        Camera _camera;
        Transform _cameraPivot;

        float _pitch;
        float _yaw;
        Vector3 _velocity;
        bool _crouching;
        bool _wishJump;
        float _jumpBufferTimer;

        // Slide / leap stubs — charge tracking only for v0
        int _slideCharges = 1;
        int _leapCharges = 1;
        float _slideRefillTimer;
        float _leapRefillTimer;
        bool _slideStubUsed;
        bool _leapStubUsed;

        public bool InputEnabled { get; set; } = true;
        public Camera PlayerCamera => _camera;
        public bool IsGrounded => _controller != null && _controller.isGrounded;
        public Vector3 Velocity => _velocity;
        public int SlideCharges => _slideCharges;
        public int LeapCharges => _leapCharges;

        public void BindCamera(Camera cam, Transform pivot)
        {
            _camera = cam;
            _cameraPivot = pivot;
        }

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        void Start()
        {
            var cfg = GameConfig.Mercenary;
            var settings = UserSettings.Data;
            if (_camera != null)
            {
                _camera.fieldOfView = settings.fov > 0 ? settings.fov : cfg.defaultFov;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _yaw = transform.eulerAngles.y;
        }

        void Update()
        {
            if (!InputEnabled)
            {
                return;
            }

            Look();
            GatherJumpBuffer();
            Move();
            UpdateMovementStubs();
        }

        void Look()
        {
            var sens = UserSettings.Data.mouseSensitivity;
            var invert = UserSettings.Data.invertY ? -1f : 1f;
            var mx = Input.GetAxisRaw("Mouse X") * sens * 10f;
            var my = Input.GetAxisRaw("Mouse Y") * sens * 10f * invert;
            _yaw += mx;
            _pitch = Mathf.Clamp(_pitch - my, -89f, 89f);
            transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
            if (_cameraPivot != null)
            {
                _cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
            }
        }

        void GatherJumpBuffer()
        {
            if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKey(KeyCode.Space) && IsGrounded))
            {
                _wishJump = true;
                _jumpBufferTimer = 0.15f;
            }

            if (_jumpBufferTimer > 0f)
            {
                _jumpBufferTimer -= Time.deltaTime;
                if (_jumpBufferTimer <= 0f)
                {
                    _wishJump = false;
                }
            }

            // Slide stub (Command / Left Ctrl alt on non-Mac): records charge spend, minimal impulse
            if (Input.GetKeyDown(KeyCode.LeftCommand) || Input.GetKeyDown(KeyCode.RightCommand) ||
                (Application.platform != RuntimePlatform.OSXPlayer &&
                 Application.platform != RuntimePlatform.OSXEditor &&
                 Input.GetKeyDown(KeyCode.LeftAlt)))
            {
                TryStubSlide();
            }

            // Leap stub: double-tap space while airborne later; for v0 use Q
            if (Input.GetKeyDown(KeyCode.Q))
            {
                TryStubLeap();
            }
        }

        void Move()
        {
            var cfg = GameConfig.Mercenary;
            _crouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);
            var targetHeight = _crouching ? 1.2f : 1.8f;
            _controller.height = Mathf.Lerp(_controller.height, targetHeight, 1f - Mathf.Exp(-20f * Time.deltaTime));
            _controller.center = new Vector3(0f, _controller.height * 0.5f, 0f);
            if (_cameraPivot != null)
            {
                var eye = _crouching ? 1.0f : 1.6f;
                var lp = _cameraPivot.localPosition;
                lp.y = Mathf.Lerp(lp.y, eye, 1f - Mathf.Exp(-20f * Time.deltaTime));
                _cameraPivot.localPosition = lp;
            }

            var input = new Vector2(
                (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f),
                (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f));
            if (input.sqrMagnitude > 1f)
            {
                input.Normalize();
            }

            var wishDir = transform.right * input.x + transform.forward * input.y;
            wishDir.y = 0f;
            if (wishDir.sqrMagnitude > 0f)
            {
                wishDir.Normalize();
            }

            var speed = cfg.moveSpeed * (_crouching ? cfg.crouchSpeedMultiplier : 1f);
            var grounded = IsGrounded;

            if (grounded && _velocity.y < 0f)
            {
                _velocity.y = -2f;
            }

            var horizontal = new Vector3(_velocity.x, 0f, _velocity.z);

            if (grounded)
            {
                if (wishDir.sqrMagnitude > 0f)
                {
                    horizontal = Vector3.MoveTowards(
                        horizontal,
                        wishDir * speed,
                        cfg.groundAcceleration * Time.deltaTime);
                }
                else
                {
                    horizontal = Vector3.MoveTowards(
                        horizontal,
                        Vector3.zero,
                        cfg.groundFriction * speed * Time.deltaTime);
                }

                if (_wishJump)
                {
                    _velocity.y = cfg.jumpSpeed;
                    _wishJump = false;
                    _jumpBufferTimer = 0f;
                    grounded = false;
                }
            }
            else
            {
                // Air strafe: accelerate along wish direction without deleting existing speed.
                if (wishDir.sqrMagnitude > 0f)
                {
                    var currentSpeed = Vector3.Dot(horizontal, wishDir);
                    var addSpeed = speed - currentSpeed;
                    if (addSpeed > 0f)
                    {
                        var accelSpeed = cfg.airAcceleration * speed * Time.deltaTime * cfg.airStrafeControl;
                        if (accelSpeed > addSpeed)
                        {
                            accelSpeed = addSpeed;
                        }

                        horizontal += wishDir * accelSpeed;
                    }
                }

                _velocity.y += cfg.gravity * Time.deltaTime;
            }

            _velocity.x = horizontal.x;
            _velocity.z = horizontal.z;
            _controller.Move(_velocity * Time.deltaTime);
        }

        void TryStubSlide()
        {
            if (_slideCharges <= 0 || !IsGrounded)
            {
                return;
            }

            _slideCharges--;
            _slideStubUsed = true;
            var dir = transform.forward;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.01f)
            {
                dir = transform.forward;
            }

            dir.Normalize();
            var impulse = dir * (GameConfig.Mercenary.slideLeapDistance * 1.4f);
            _velocity.x = impulse.x;
            _velocity.z = impulse.z;
            Debug.Log("Slide stub: charge spent (full slide feel TBD).");
        }

        void TryStubLeap()
        {
            if (_leapCharges <= 0)
            {
                return;
            }

            _leapCharges--;
            _leapStubUsed = true;
            var input = new Vector3(
                (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f),
                0f,
                (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f));
            if (input.sqrMagnitude < 0.01f)
            {
                input = Vector3.forward;
            }

            input = transform.TransformDirection(input.normalized);
            input.y = 0f;
            input.Normalize();
            var dist = GameConfig.Mercenary.slideLeapDistance;
            _velocity.x = input.x * dist * 1.2f;
            _velocity.z = input.z * dist * 1.2f;
            _velocity.y = Mathf.Max(_velocity.y, GameConfig.Mercenary.jumpSpeed * 0.55f);
            Debug.Log("Leap stub: charge spent (trajectory TBD).");
        }

        void UpdateMovementStubs()
        {
            var cfg = GameConfig.Mercenary;
            if (_slideCharges < 1)
            {
                if (IsGrounded)
                {
                    _slideRefillTimer += Time.deltaTime;
                    if (_slideRefillTimer >= cfg.slideLeapChargeDelay + cfg.slideLeapRefillSeconds)
                    {
                        _slideCharges = 1;
                        _slideRefillTimer = 0f;
                    }
                }
            }
            else
            {
                _slideRefillTimer = 0f;
            }

            if (_leapCharges < 1)
            {
                if (IsGrounded)
                {
                    _leapRefillTimer += Time.deltaTime;
                    if (_leapRefillTimer >= cfg.slideLeapChargeDelay + cfg.slideLeapRefillSeconds)
                    {
                        _leapCharges = 1;
                        _leapRefillTimer = 0f;
                    }
                }
            }
            else
            {
                _leapRefillTimer = 0f;
            }
        }

        public void Teleport(Vector3 position, Vector3? lookEuler = null)
        {
            _controller.enabled = false;
            transform.position = position;
            _controller.enabled = true;
            _velocity = Vector3.zero;
            if (lookEuler.HasValue)
            {
                _yaw = lookEuler.Value.y;
                _pitch = lookEuler.Value.x;
            }
        }

        public void SetInputEnabled(bool enabled)
        {
            InputEnabled = enabled;
            if (enabled)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
