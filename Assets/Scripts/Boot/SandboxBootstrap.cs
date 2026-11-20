using Ironstrait.Arena;
using Ironstrait.Combat;
using Ironstrait.Config;
using Ironstrait.DebugTools;
using Ironstrait.Player;
using Ironstrait.UI;
using Ironstrait.Weapons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ironstrait.Boot
{
    /// <summary>
    /// Builds the playable sandbox at runtime so the project opens without
    /// hand-authored prefab wiring in the editor.
    /// </summary>
    public class SandboxBootstrap : MonoBehaviour
    {
        FirstPersonController _controller;
        MercenaryWeapons _weapons;
        Health _health;
        ScoreProgression _score;
        SandboxHud _hud;
        DevConsole _console;
        void Start()
        {
            UserSettings.Load();
            GameConfig.Reload();
            GrayboxArena.Build();
            SpawnPlayer();
            SetupHudAndConsole();
        }

        void SpawnPlayer()
        {
            var player = new GameObject("MercenaryPlayer");
            player.tag = "Player";
            player.transform.position = GrayboxArena.PlayerSpawn;

            var controller = player.AddComponent<CharacterController>();
            controller.height = 1.8f;
            controller.radius = 0.35f;
            controller.center = new Vector3(0f, 0.9f, 0f);
            controller.stepOffset = 0.35f;
            controller.slopeLimit = 45f;

            _controller = player.AddComponent<FirstPersonController>();
            _health = player.AddComponent<Health>();
            _health.ConfigureFromMercenary();
            _score = player.AddComponent<ScoreProgression>();
            _weapons = player.AddComponent<MercenaryWeapons>();

            var camPivot = new GameObject("CameraPivot");
            camPivot.transform.SetParent(player.transform, false);
            camPivot.transform.localPosition = new Vector3(0f, 1.6f, 0f);

            var camGo = new GameObject("PlayerCamera");
            camGo.transform.SetParent(camPivot.transform, false);
            var cam = camGo.AddComponent<Camera>();
            cam.nearClipPlane = 0.05f;
            cam.farClipPlane = 200f;
            cam.fieldOfView = UserSettings.Data.fov;
            camGo.AddComponent<AudioListener>();
            cam.tag = "MainCamera";

            var vmRoot = new GameObject("Viewmodels");
            vmRoot.transform.SetParent(camGo.transform, false);
            vmRoot.transform.localPosition = Vector3.zero;

            _controller.BindCamera(cam, camPivot.transform);
            _weapons.Initialize(_controller, _score, _health, vmRoot.transform);

            _health.Died += _ =>
            {
                _hud?.ShowToast("You died — auto-respawn");
                Invoke(nameof(AutoRespawn), 1.25f);
            };
        }

        void AutoRespawn()
        {
            if (_controller == null)
            {
                return;
            }

            _controller.Teleport(GrayboxArena.PlayerSpawn);
            _health.ResetFull();
            _weapons.ResetAmmo();
        }

        void SetupHudAndConsole()
        {
            var ui = new GameObject("SandboxUI");
            _hud = ui.AddComponent<SandboxHud>();
            _hud.Bind(_health, _weapons, _score, _controller);
            _console = ui.AddComponent<DevConsole>();
            _console.Bind(_health, _score, _weapons, _controller, GrayboxArena.PlayerSpawn);
            ui.AddComponent<PauseMenu>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Same Esc must close console without also opening pause.
                if (_console != null && _console.IsOpen)
                {
                    _console.Close();
                    return;
                }

                var pause = Time.timeScale > 0.01f;
                Time.timeScale = pause ? 0f : 1f;
                _controller?.SetInputEnabled(!pause);
                if (_weapons != null)
                {
                    _weapons.InputEnabled = !pause;
                }
            }
        }
    }

    public class PauseMenu : MonoBehaviour
    {
        void OnGUI()
        {
            if (Time.timeScale > 0.01f)
            {
                return;
            }

            var rect = new Rect(Screen.width * 0.5f - 140, Screen.height * 0.5f - 80, 280, 160);
            GUI.Box(rect, "Paused");
            GUILayout.BeginArea(new Rect(rect.x + 20, rect.y + 40, 240, 120));
            if (GUILayout.Button("Resume (Esc)", GUILayout.Height(36)))
            {
                Time.timeScale = 1f;
                var fp = Object.FindFirstObjectByType<FirstPersonController>();
                fp?.SetInputEnabled(true);
                var w = Object.FindFirstObjectByType<MercenaryWeapons>();
                if (w != null)
                {
                    w.InputEnabled = true;
                }
            }

            if (GUILayout.Button("Main Menu", GUILayout.Height(36)))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("BootMenu");
            }

            GUILayout.EndArea();
        }
    }
}
