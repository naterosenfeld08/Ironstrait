using Ironstrait.Config;
using Ironstrait.UI;
using UnityEngine;

namespace Ironstrait.Boot
{
    /// <summary>
    /// Ensures the boot menu scene has a camera + menu controller even if
    /// the YAML scene is minimal.
    /// </summary>
    public class BootMenuEntry : MonoBehaviour
    {
        void Awake()
        {
            UserSettings.Load();

            if (Camera.main == null)
            {
                var camGo = new GameObject("MenuCamera");
                var cam = camGo.AddComponent<Camera>();
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = new Color(0.12f, 0.13f, 0.15f);
                camGo.AddComponent<AudioListener>();
                cam.tag = "MainCamera";
            }

            if (FindFirstObjectByType<MainMenuController>() == null)
            {
                gameObject.AddComponent<MainMenuController>();
            }

            // Soft fill light feel via ambient
            RenderSettings.ambientLight = new Color(0.2f, 0.22f, 0.25f);
        }
    }
}
