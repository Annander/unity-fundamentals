using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Course.Fundamentals.EditorTools
{
    public static class BuildScript
    {
        private const string BuildRoot = "Builds";

        [MenuItem("Course/Build/Windows (Development)")]
        private static void BuildWindowsDevelopment() => Build(BuildTarget.StandaloneWindows64, development: true);

        [MenuItem("Course/Build/Windows (Release)")]
        private static void BuildWindowsRelease() => Build(BuildTarget.StandaloneWindows64, development: false);

        /// <summary>Entry point for -executeMethod. Uses the target given by -buildTarget (the active target).</summary>
        public static void BuildFromCommandLine()
        {
            var development = Environment.GetCommandLineArgs().Contains("-devBuild");
            var report = Build(EditorUserBuildSettings.activeBuildTarget, development);

            // A non-zero exit code is how CI knows the build failed.
            EditorApplication.Exit(report != null && report.summary.result == BuildResult.Succeeded ? 0 : 1);
        }

        public static BuildReport Build(BuildTarget target, bool development)
        {
            var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();

            if (scenes.Length == 0)
            {
                Debug.LogError("[Build] No scenes in the scene list. Add them in File > Build Profiles > Scene List.");
                return null;
            }

            var folder = Path.Combine(BuildRoot, $"{target}-{(development ? "Development" : "Release")}");
            var executable = Path.Combine(folder, PlayerSettings.productName + Extension(target));

            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = executable,
                target = target,
                // Development: debug symbols, Development Build watermark, profiler auto-connect,
                // and a managed debugger can attach. Release: none of that.
                options = development
                    ? BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging
                    : BuildOptions.None,
            };

            // If target differs from the active platform, Unity switches platform first (and may reimport).
            var report = BuildPipeline.BuildPlayer(options);
            LogSummary(report);
            return report;
        }

        private static void LogSummary(BuildReport report)
        {
            var s = report.summary;
            Debug.Log($"[Build] {s.result}: {s.platform}, {s.totalSize / (1024f * 1024f):F1} MB, " +
                      $"{s.totalTime.TotalSeconds:F0} s, {s.totalErrors} errors, {s.totalWarnings} warnings\n" +
                      $"Output: {Path.GetFullPath(s.outputPath)}");

            // The ten biggest assets in the build. Usually a texture someone forgot to compress.
            var largest = report.packedAssets
                .SelectMany(p => p.contents)
                .Where(c => !string.IsNullOrEmpty(c.sourceAssetPath))
                .OrderByDescending(c => c.packedSize)
                .Take(10)
                .Select(c => $"  {c.packedSize / 1024f,10:F0} KB  {c.sourceAssetPath}");

            Debug.Log("[Build] Largest assets:\n" + string.Join("\n", largest));
        }

        private static string Extension(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64: return ".exe";
                case BuildTarget.StandaloneOSX: return ".app";
                case BuildTarget.Android: return EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk";
                default: return string.Empty; // Linux: no extension; iOS/WebGL: a folder
            }
        }
    }
}