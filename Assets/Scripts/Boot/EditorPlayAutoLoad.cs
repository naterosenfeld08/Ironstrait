#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ironstrait.Boot
{
    /// <summary>
    /// When pressing Play from an arbitrary scene, jump to BootMenu if the
    /// open scene lacks our bootstrap. Keeps Hub → Play reliable.
    /// </summary>
    [InitializeOnLoad]
    public static class EditorPlayAutoLoad
    {
        const string BootScenePath = "Assets/Scenes/BootMenu.unity";

        static EditorPlayAutoLoad()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.EnteredPlayMode)
            {
                return;
            }

            var active = SceneManager.GetActiveScene();
            if (active.name == "BootMenu" || active.name == "SandboxArena")
            {
                return;
            }

            if (Object.FindFirstObjectByType<SandboxBootstrap>() != null ||
                Object.FindFirstObjectByType<BootMenuEntry>() != null)
            {
                return;
            }

            if (System.IO.File.Exists(BootScenePath))
            {
                SceneManager.LoadScene("BootMenu");
            }
        }
    }
}
#endif
