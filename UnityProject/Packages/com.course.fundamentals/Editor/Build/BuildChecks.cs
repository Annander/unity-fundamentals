using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Course.Fundamentals.EditorTools
{
    /// Runs before and after EVERY Player build, however it was started (menu, Build Profiles window, CLI).
    internal sealed class BuildChecks : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            var s = report.summary;
            var development = (s.options & BuildOptions.Development) != 0;
            var namedTarget = NamedBuildTarget.FromBuildTargetGroup(s.platformGroup);
            var backend = PlayerSettings.GetScriptingBackend(namedTarget);

            // Print the configuration, so every build log says exactly what was built.
            Debug.Log($"[BuildChecks] {PlayerSettings.productName} {PlayerSettings.bundleVersion} | {s.platform} | " +
                      $"{backend} | {(development ? "Development" : "Release")}");

            // Throwing BuildFailedException stops the build with a clear message.
            // Team rule: we render with URP. No pipeline asset means the deprecated Built-in pipeline.
            if (GraphicsSettings.defaultRenderPipeline == null)
                throw new BuildFailedException("No render pipeline asset assigned (Project Settings > Graphics). We use URP.");

            // An example team rule: release builds use IL2CPP.
            if (!development && backend != ScriptingImplementation.IL2CPP)
                Debug.LogWarning("[BuildChecks] Release build on Mono. We ship on IL2CPP. Test what you ship.");
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            // report.summary.result isn't final yet at this point; BuildScript logs the result afterwards.
            Debug.Log($"[BuildChecks] Player written to {report.summary.outputPath}. Run it, then read its " +
                      "Player.log (the BuildInfoOverlay shows the path).");
        }
    }
}