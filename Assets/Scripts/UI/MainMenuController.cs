using Ironstrait.Config;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ironstrait.UI
{
    public class MainMenuController : MonoBehaviour
    {
        enum Panel
        {
            Main,
            Settings
        }

        Panel _panel = Panel.Main;
        string _status = "Ironstrait · Mercenary sandbox v0";
        float _sens;
        float _fov;
        float _volume;
        bool _invertY;
        bool _fullscreen;

        void Start()
        {
            UserSettings.Load();
            SyncFromSettings();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void SyncFromSettings()
        {
            var d = UserSettings.Data;
            _sens = d.mouseSensitivity;
            _fov = d.fov;
            _volume = d.masterVolume;
            _invertY = d.invertY;
            _fullscreen = d.fullscreen;
        }

        void OnGUI()
        {
            var w = 420f;
            var h = _panel == Panel.Main ? 280f : 360f;
            var rect = new Rect((Screen.width - w) * 0.5f, (Screen.height - h) * 0.5f, w, h);
            GUI.Box(rect, "");
            GUILayout.BeginArea(new Rect(rect.x + 24, rect.y + 20, w - 48, h - 40));

            var title = new GUIStyle(GUI.skin.label)
            {
                fontSize = 28,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
            GUILayout.Label("IRONSTRAIT", title);
            GUILayout.Label(_status);
            GUILayout.Space(12);

            if (_panel == Panel.Main)
            {
                if (GUILayout.Button("Play Sandbox", GUILayout.Height(40)))
                {
                    SceneManager.LoadScene("SandboxArena");
                }

                GUILayout.Space(8);
                if (GUILayout.Button("Settings", GUILayout.Height(36)))
                {
                    _panel = Panel.Settings;
                    SyncFromSettings();
                }

                GUILayout.Space(8);
                if (GUILayout.Button("Quit", GUILayout.Height(36)))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
            }
            else
            {
                GUILayout.Label($"Mouse sensitivity: {_sens:0.000}");
                _sens = GUILayout.HorizontalSlider(_sens, 0.01f, 0.5f);
                GUILayout.Label($"FOV: {_fov:0}");
                _fov = GUILayout.HorizontalSlider(_fov, 50f, 100f);
                GUILayout.Label($"Master volume: {_volume:0.00}");
                _volume = GUILayout.HorizontalSlider(_volume, 0f, 1f);
                _invertY = GUILayout.Toggle(_invertY, "Invert Y");
                _fullscreen = GUILayout.Toggle(_fullscreen, "Fullscreen");

                GUILayout.Space(10);
                if (GUILayout.Button("Save", GUILayout.Height(32)))
                {
                    var d = UserSettings.Data;
                    d.mouseSensitivity = _sens;
                    d.fov = _fov;
                    d.masterVolume = _volume;
                    d.invertY = _invertY;
                    d.fullscreen = _fullscreen;
                    UserSettings.Save();
                    UserSettings.ApplyDisplay();
                    _status = $"Saved → {UserSettings.PersistentPath}";
                }

                if (GUILayout.Button("Reset defaults", GUILayout.Height(28)))
                {
                    UserSettings.ResetToDefaults();
                    SyncFromSettings();
                    _status = "Defaults restored";
                }

                GUILayout.Space(8);
                if (GUILayout.Button("Back", GUILayout.Height(32)))
                {
                    _panel = Panel.Main;
                }
            }

            GUILayout.EndArea();
        }
    }
}
