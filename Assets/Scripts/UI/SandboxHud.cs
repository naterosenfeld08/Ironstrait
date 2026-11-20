using Ironstrait.Combat;
using Ironstrait.Player;
using Ironstrait.Weapons;
using UnityEngine;

namespace Ironstrait.UI
{
    public class SandboxHud : MonoBehaviour
    {
        Health _health;
        MercenaryWeapons _weapons;
        ScoreProgression _score;
        FirstPersonController _controller;

        GUIStyle _boxStyle;
        GUIStyle _labelStyle;
        GUIStyle _crosshairStyle;
        bool _stylesReady;

        string _toast;
        float _toastUntil;

        public void Bind(Health health, MercenaryWeapons weapons, ScoreProgression score,
            FirstPersonController controller)
        {
            _health = health;
            _weapons = weapons;
            _score = score;
            _controller = controller;
        }

        public void ShowToast(string message, float seconds = 1.5f)
        {
            _toast = message;
            _toastUntil = Time.time + seconds;
        }

        void EnsureStyles()
        {
            if (_stylesReady)
            {
                return;
            }

            _boxStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(12, 12, 8, 8)
            };
            _labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };
            _crosshairStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 22,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(1f, 0.2f, 0.2f, 0.95f) }
            };
            _stylesReady = true;
        }

        void OnGUI()
        {
            if (_health == null || _weapons == null)
            {
                return;
            }

            EnsureStyles();

            // Crosshair — small red outlined circle approximation
            var cx = Screen.width * 0.5f;
            var cy = Screen.height * 0.5f;
            GUI.Label(new Rect(cx - 12, cy - 14, 24, 28), "○", _crosshairStyle);

            var hp = $"{Mathf.CeilToInt(_health.CurrentHealth)} / {Mathf.CeilToInt(_health.MaxHealth)}";
            var sg = _weapons.ShotgunReloading
                ? $"SG … ({_weapons.ShotgunAmmo}/{_weapons.ShotgunMagSize})"
                : $"SG {_weapons.ShotgunAmmo}/{_weapons.ShotgunMagSize}";
            var gl = _weapons.GrenadeReloading
                ? $"GL … ({_weapons.GrenadeAmmo}/{_weapons.GrenadeMagSize})"
                : $"GL {_weapons.GrenadeAmmo}/{_weapons.GrenadeMagSize}";

            var scoreText = _score != null
                ? $"Score {_score.Score:0.##}   Lv {_score.Level}   next {_score.ScoreToNext:0.##}"
                : "Score —";

            var move = _controller != null
                ? $"Slide {_controller.SlideCharges}  Leap {_controller.LeapCharges}  spd {_controller.Velocity.magnitude:0.0}"
                : "";

            GUI.Box(new Rect(20, Screen.height - 110, 420, 90), "", _boxStyle);
            GUI.Label(new Rect(32, Screen.height - 102, 400, 24), $"HP {hp}", _labelStyle);
            GUI.Label(new Rect(32, Screen.height - 78, 400, 24), $"{sg}    {gl}", _labelStyle);
            GUI.Label(new Rect(32, Screen.height - 54, 400, 24), scoreText, _labelStyle);

            GUI.Label(new Rect(20, 16, 500, 24), "Ironstrait sandbox — Esc menu · ` console · Q leap stub", _labelStyle);
            if (!string.IsNullOrEmpty(move))
            {
                GUI.Label(new Rect(20, 42, 500, 22), move, _labelStyle);
            }

            if (!string.IsNullOrEmpty(_toast) && Time.time < _toastUntil)
            {
                GUI.Label(new Rect(Screen.width * 0.5f - 150, Screen.height * 0.35f, 300, 30), _toast,
                    _crosshairStyle);
            }
        }
    }
}
