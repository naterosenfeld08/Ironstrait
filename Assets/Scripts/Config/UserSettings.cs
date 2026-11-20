using System;
using System.IO;
using UnityEngine;

namespace Ironstrait.Config
{
    [Serializable]
    public class KeybindSettings
    {
        public string moveForward = "W";
        public string moveBack = "S";
        public string moveLeft = "A";
        public string moveRight = "D";
        public string jump = "Space";
        public string crouch = "LeftControl";
        public string slide = "LeftCommand";
        public string firePrimary = "Mouse0";
        public string fireSecondary = "Mouse1";
        public string melee = "V";
        public string scoreboard = "Tab";
        public string console = "BackQuote";
        public string pause = "Escape";
    }

    [Serializable]
    public class UserSettingsData
    {
        public int schemaVersion = 1;
        public float mouseSensitivity = 0.12f;
        public float fov = 90f;
        public bool invertY;
        public float masterVolume = 0.8f;
        public bool showDamageNumbers = true;
        public int windowedWidth = 1920;
        public int windowedHeight = 1080;
        public bool fullscreen;
        public KeybindSettings keybinds = new KeybindSettings();
    }

    public static class UserSettings
    {
        const string FileName = "user_settings_v1.json";
        const string DefaultResourcePath = "Config/default_user_settings";

        static UserSettingsData _data;

        public static UserSettingsData Data
        {
            get
            {
                if (_data == null)
                {
                    Load();
                }

                return _data;
            }
        }

        public static string PersistentPath =>
            Path.Combine(Application.persistentDataPath, FileName);

        public static void Load()
        {
            _data = CreateDefaults();

            try
            {
                if (File.Exists(PersistentPath))
                {
                    var json = File.ReadAllText(PersistentPath);
                    var loaded = JsonUtility.FromJson<UserSettingsData>(json);
                    if (loaded != null)
                    {
                        MergeValid(loaded);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Settings load failed, using defaults: {ex.Message}");
                _data = CreateDefaults();
            }

            Validate();
            ApplyDisplay();
        }

        public static void Save()
        {
            Validate();
            try
            {
                var dir = Path.GetDirectoryName(PersistentPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(PersistentPath, JsonUtility.ToJson(Data, true));
            }
            catch (Exception ex)
            {
                Debug.LogError($"Settings save failed: {ex.Message}");
            }
        }

        public static void ResetToDefaults()
        {
            _data = CreateDefaults();
            Save();
            ApplyDisplay();
        }

        static UserSettingsData CreateDefaults()
        {
            var asset = Resources.Load<TextAsset>(DefaultResourcePath);
            if (asset != null)
            {
                try
                {
                    var fromResource = JsonUtility.FromJson<UserSettingsData>(asset.text);
                    if (fromResource != null)
                    {
                        return fromResource;
                    }
                }
                catch
                {
                    // fall through
                }
            }

            return new UserSettingsData();
        }

        static void MergeValid(UserSettingsData loaded)
        {
            if (loaded.schemaVersion < 1)
            {
                return;
            }

            _data = loaded;
            if (_data.keybinds == null)
            {
                _data.keybinds = new KeybindSettings();
            }
        }

        static void Validate()
        {
            var d = Data;
            d.mouseSensitivity = Mathf.Clamp(d.mouseSensitivity, 0.01f, 1f);
            d.fov = Mathf.Clamp(d.fov, 50f, 100f);
            d.masterVolume = Mathf.Clamp01(d.masterVolume);
            d.windowedWidth = Mathf.Clamp(d.windowedWidth, 800, 7680);
            d.windowedHeight = Mathf.Clamp(d.windowedHeight, 600, 4320);
            if (d.keybinds == null)
            {
                d.keybinds = new KeybindSettings();
            }
        }

        public static void ApplyDisplay()
        {
            var d = Data;
            if (d.fullscreen)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            }
            else
            {
                var w = d.windowedWidth;
                var h = d.windowedHeight;
                if (w > Screen.currentResolution.width || h > Screen.currentResolution.height)
                {
                    w = Mathf.Min(w, Screen.currentResolution.width);
                    h = Mathf.Min(h, Screen.currentResolution.height);
                }

                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.SetResolution(w, h, false);
            }

            AudioListener.volume = d.masterVolume;
        }
    }
}
