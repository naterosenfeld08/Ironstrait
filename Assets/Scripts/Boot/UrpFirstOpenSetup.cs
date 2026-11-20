#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Ironstrait.Boot
{
    /// <summary>
    /// On first project open, creates a URP asset + renderer if Graphics has none assigned.
    /// Safe no-op once Project Settings → Graphics already references a pipeline asset.
    /// </summary>
    [InitializeOnLoad]
    public static class UrpFirstOpenSetup
    {
        const string SettingsFolder = "Assets/Settings";
        const string PipelinePath = SettingsFolder + "/URP-HighFidelity.asset";
        const string RendererPath = SettingsFolder + "/URP-HighFidelity_Renderer.asset";
        const string MarkerKey = "Ironstrait.UrpSetup.Done";

        static UrpFirstOpenSetup()
        {
            EditorApplication.delayCall += TrySetup;
        }

        static void TrySetup()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            if (GraphicsSettings.defaultRenderPipeline != null)
            {
                return;
            }

            if (!Directory.Exists(SettingsFolder))
            {
                Directory.CreateDirectory(SettingsFolder);
                AssetDatabase.Refresh();
            }

            var renderer = AssetDatabase.LoadAssetAtPath<ScriptableRendererData>(RendererPath);
            if (renderer == null)
            {
                renderer = ScriptableObject.CreateInstance<UniversalRendererData>();
                AssetDatabase.CreateAsset(renderer, RendererPath);
            }

            var pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(PipelinePath);
            if (pipeline == null)
            {
                pipeline = UniversalRenderPipelineAsset.Create(renderer);
                AssetDatabase.CreateAsset(pipeline, PipelinePath);
            }

            GraphicsSettings.defaultRenderPipeline = pipeline;
            QualitySettings.renderPipeline = pipeline;
            EditorUtility.SetDirty(pipeline);
            AssetDatabase.SaveAssets();
            Debug.Log("Ironstrait: assigned URP pipeline assets under Assets/Settings/. Reopen Play if materials were pink.");
            SessionState.SetBool(MarkerKey, true);
        }
    }
}
#endif
