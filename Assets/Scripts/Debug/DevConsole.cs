using System;
using System.Globalization;
using Ironstrait.Arena;
using Ironstrait.Combat;
using Ironstrait.Player;
using Ironstrait.Weapons;
using UnityEngine;

namespace Ironstrait.DebugTools
{
    public class DevConsole : MonoBehaviour
    {
        bool _open;
        string _input = "";
        string _log = "Type help";
        Health _health;
        ScoreProgression _score;
        MercenaryWeapons _weapons;
        FirstPersonController _controller;
        Vector3 _spawn;

        public bool IsOpen => _open;

        public void Bind(Health health, ScoreProgression score, MercenaryWeapons weapons,
            FirstPersonController controller, Vector3 spawn)
        {
            _health = health;
            _score = score;
            _weapons = weapons;
            _controller = controller;
            _spawn = spawn;
        }

        public void Close()
        {
            if (!_open)
            {
                return;
            }

            SetOpen(false);
        }

        void Update()
        {
            // Esc close is handled by SandboxBootstrap so the same keypress cannot also pause.
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                SetOpen(!_open);
            }
        }

        void SetOpen(bool open)
        {
            _open = open;
            if (_controller != null)
            {
                _controller.SetInputEnabled(!_open);
            }

            if (_weapons != null)
            {
                _weapons.InputEnabled = !_open;
            }
        }

        void OnGUI()
        {
            if (!_open)
            {
                return;
            }

            GUI.Box(new Rect(20, 80, Screen.width - 40, 140), "Developer Console  (` or Esc to close)");
            GUI.Label(new Rect(30, 110, Screen.width - 60, 40), _log);
            GUI.SetNextControlName("ConsoleInput");
            _input = GUI.TextField(new Rect(30, 160, Screen.width - 200, 28), _input);
            GUI.FocusControl("ConsoleInput");
            if (GUI.Button(new Rect(Screen.width - 160, 160, 120, 28), "Run") ||
                (Event.current.isKey && Event.current.keyCode == KeyCode.Return &&
                 Event.current.type == EventType.KeyUp))
            {
                Execute(_input);
                _input = "";
            }
        }

        void Execute(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            var parts = line.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();
            try
            {
                switch (cmd)
                {
                    case "help":
                        _log =
                            "help | kill | respawn | reset | set health <n> | set level <n> | set damage_multiplier <n> [sec] | target clear | debug movement on|off";
                        break;
                    case "kill":
                        _health?.SetHealth(0f);
                        _log = "killed";
                        break;
                    case "respawn":
                    case "reset":
                        RespawnPlayer(cmd == "reset");
                        _log = cmd == "reset" ? "reset to baseline at spawn" : "respawned";
                        break;
                    case "set":
                        HandleSet(parts);
                        break;
                    case "target":
                        if (parts.Length > 1 && parts[1] == "clear")
                        {
                            foreach (var d in UnityEngine.Object.FindObjectsByType<DummyTarget>(
                                         FindObjectsSortMode.None))
                            {
                                UnityEngine.Object.Destroy(d.gameObject);
                            }

                            _log = "targets cleared";
                        }
                        else if (parts.Length > 1 && parts[1] == "spawn")
                        {
                            GrayboxArena.Build();
                            _log = "arena rebuilt (includes targets)";
                        }
                        else
                        {
                            _log = "target spawn | target clear";
                        }

                        break;
                    case "debug":
                        _log = parts.Length > 2 && parts[1] == "movement"
                            ? $"movement debug {(parts[2])} (HUD shows speed/charges)"
                            : "debug movement on|off";
                        break;
                    default:
                        _log = $"unknown command: {cmd}";
                        break;
                }
            }
            catch (Exception ex)
            {
                _log = $"error: {ex.Message}";
            }
        }

        void HandleSet(string[] parts)
        {
            if (parts.Length < 3)
            {
                _log = "set health <n> | set level <n> | set damage_multiplier <n> [duration]";
                return;
            }

            var what = parts[1].ToLowerInvariant();
            if (what == "health")
            {
                var v = float.Parse(parts[2], CultureInfo.InvariantCulture);
                _health?.SetHealth(v);
                _log = $"health = {v}";
            }
            else if (what == "level")
            {
                var v = int.Parse(parts[2], CultureInfo.InvariantCulture);
                _score?.SetLevel(v);
                _log = $"level = {v}";
            }
            else if (what == "damage_multiplier")
            {
                _log = "damage_multiplier override stub — not yet applied to weapons";
            }
            else
            {
                _log = "unknown set target";
            }
        }

        void RespawnPlayer(bool fullReset)
        {
            if (_controller != null)
            {
                _controller.Teleport(_spawn, new Vector3(0f, 0f, 0f));
            }

            _health?.ResetFull();
            _weapons?.ResetAmmo();
            if (fullReset)
            {
                _score?.ResetProgress();
            }
        }
    }
}
