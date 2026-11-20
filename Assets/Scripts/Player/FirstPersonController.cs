using Ironstrait.Audio;
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
        AudioSource _audio;

        float _pitch;
        float _yaw;
        Vector3 _velocity;
        bool _crouching;
        bool _wishJump;
        float _jumpBufferTimer;
        float _coyoteTimer;
        bool _jumpLockedUntilRelease;

        // Slide / leap stubs — charge tracking only for v0
        int _slideCharges = 1;
        int _leapCharges = 1;
        float _slideRefillTimer;
        float _leapRefillTimer;

        const float GroundProbeDistance = 0.28f;
        const float MinFloorNormalY = 0.65f;
        // Preserve intentional air-strafe / flat bhop above moveSpeed; block collision boosts.
        const float AirSpeedSoftCapMultiplier = 2.75f;

        public bool InputEnabled { get; set; } = true;
        public Camera PlayerCamera => _camera;
        public bool IsGrounded => IsWalkableGrounded();
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
            _audio = GetComponent<AudioSource>();
            if (_audio == null)
            {
                _audio = gameObject.AddComponent<AudioSource>();
                _audio.playOnAwake = false;
                _audio.spatialBlend = 0f;
            }
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
            if (!Input.GetKey(KeyCode.Space))
            {
                _jumpLockedUntilRelease = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
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
            var walkable = IsWalkableGrounded();
            if (walkable)
            {
                _coyoteTimer = 0.08f;
            }
            else
            {
                _coyoteTimer = Mathf.Max(0f, _coyoteTimer - Time.deltaTime);
            }

            // Airborne: disable step-up so box lips don't convert jump holds into climbs/launches.
            _controller.stepOffset = walkable ? 0.35f : 0.05f;

            if (walkable && _velocity.y < 0f)
            {
                _velocity.y = -2f;
            }

            var horizontal = new Vector3(_velocity.x, 0f, _velocity.z);

            if (walkable)
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

            // Jump from walkable floor or brief coyote window (not from box lips / side contacts).
            var canJump = (walkable || _coyoteTimer > 0f) && !_jumpLockedUntilRelease;
            if (_wishJump && canJump)
            {
                _velocity.y = cfg.jumpSpeed;
                _wishJump = false;
                _jumpBufferTimer = 0f;
                _coyoteTimer = 0f;
                walkable = false;
                PlaceholderSfx.Play(_audio, PlaceholderSfx.Jump, 0.35f, 1f);
            }

            // Auto-bhop on flat ground: holding Space queues the next jump while airborne so landing fires it.
            if (!walkable && Input.GetKey(KeyCode.Space) && !_jumpLockedUntilRelease)
            {
                _wishJump = true;
                _jumpBufferTimer = 0.12f;
            }

            _velocity.x = horizontal.x;
            _velocity.z = horizontal.z;

            var preHorizontal = new Vector3(_velocity.x, 0f, _velocity.z);
            var preHorizontalSpeed = preHorizontal.magnitude;
            var preVertical = _velocity.y;

            var flags = _controller.Move(_velocity * Time.deltaTime);
            SanitizePostCollisionVelocity(flags, preHorizontal, preHorizontalSpeed, preVertical, speed);
        }

        void SanitizePostCollisionVelocity(CollisionFlags flags, Vector3 preHorizontal,
            float preHorizontalSpeed, float preVertical, float groundedMaxSpeed)
        {
            var hitSides = (flags & CollisionFlags.Sides) != 0;
            var hitAbove = (flags & CollisionFlags.Above) != 0;
            var hitBelow = (flags & CollisionFlags.Below) != 0;

            // Ceiling / overhead props: never bounce upward from the hit.
            if (hitAbove && _velocity.y > 0f)
            {
                _velocity.y = 0f;
            }

            // Side/prop hits while jumping must not invent vertical launch.
            if (hitSides && !IsWalkableGrounded())
            {
                if (_velocity.y > preVertical)
                {
                    _velocity.y = preVertical;
                }

                if (_velocity.y > 0f && preVertical <= 0f)
                {
                    _velocity.y = 0f;
                }

                // Holding jump against boxes previously re-armed grounded jumps — lock until release.
                if (Input.GetKey(KeyCode.Space))
                {
                    _jumpLockedUntilRelease = true;
                    _wishJump = false;
                }
            }

            // Clamp unintended horizontal boost from geometry collision (keep pre-collision / grounded max).
            if (hitSides || hitAbove)
            {
                var post = new Vector3(_velocity.x, 0f, _velocity.z);
                var postSpeed = post.magnitude;
                var allowed = Mathf.Max(preHorizontalSpeed, groundedMaxSpeed);
                // Soft ceiling so air-strafe bhop still feels good, but collision cannot stack forever.
                var hardCap = groundedMaxSpeed * AirSpeedSoftCapMultiplier;
                allowed = Mathf.Min(Mathf.Max(allowed, groundedMaxSpeed), hardCap);

                if (postSpeed > allowed && postSpeed > 0.001f)
                {
                    post *= allowed / postSpeed;
                    _velocity.x = post.x;
                    _velocity.z = post.z;
                }
                else if (postSpeed > preHorizontalSpeed && preHorizontalSpeed > groundedMaxSpeed)
                {
                    // Collision added speed above an already-supersonic air strafe — snap back to pre.
                    if (preHorizontalSpeed > 0.001f)
                    {
                        post = preHorizontal.normalized * preHorizontalSpeed;
                        _velocity.x = post.x;
                        _velocity.z = post.z;
                    }
                }
            }

            // False "grounded" on steep/side hits: strip sticky downward clamp abuse next frame via probe.
            if (hitBelow && !ProbeWalkableFloor(out _))
            {
                // Don't treat as a land for jump re-arm.
                if (Input.GetKey(KeyCode.Space))
                {
                    _jumpLockedUntilRelease = true;
                    _wishJump = false;
                }
            }
        }

        bool IsWalkableGrounded()
        {
            if (_controller == null)
            {
                return false;
            }

            // CharacterController.isGrounded alone is true on box lips/sides and enables the launch exploit.
            if (!_controller.isGrounded && !ProbeWalkableFloor(out _))
            {
                return false;
            }

            return ProbeWalkableFloor(out _);
        }

        bool ProbeWalkableFloor(out RaycastHit hit)
        {
            hit = default;
            if (_controller == null)
            {
                return false;
            }

            var radius = Mathf.Max(0.05f, _controller.radius * 0.9f);
            var origin = transform.position + Vector3.up * (radius + 0.05f);
            var distance = GroundProbeDistance + _controller.skinWidth + 0.05f;
            if (!Physics.SphereCast(origin, radius * 0.95f, Vector3.down, out hit, distance, ~0,
                    QueryTriggerInteraction.Ignore))
            {
                return false;
            }

            return hit.normal.y >= MinFloorNormalY;
        }

        void TryStubSlide()
        {
            if (_slideCharges <= 0 || !IsGrounded)
            {
                return;
            }

            _slideCharges--;
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
            _wishJump = false;
            _jumpLockedUntilRelease = false;
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
