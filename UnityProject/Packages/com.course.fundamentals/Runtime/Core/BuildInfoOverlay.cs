using UnityEngine;

namespace Course.Fundamentals
{
    public sealed class BuildInfoOverlay : MonoBehaviour
    {
        // Conditional compilation: in a release build this class is an empty MonoBehaviour.
        // DEVELOPMENT_BUILD is defined when "Development Build" is ticked; UNITY_EDITOR only in the Editor.
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        private string _text;

        private void Awake()
        {
#if ENABLE_IL2CPP
            const string backend = "IL2CPP";
#else
            const string backend = "Mono";
#endif
            // Built once, not per frame (session 06).
            _text = $"{Application.productName} {Application.version}\n" +
                    $"{Application.platform} | {backend} | Unity {Application.unityVersion}\n" +
                    $"Editor: {Application.isEditor} | Development: {Debug.isDebugBuild}\n" +
                    $"GPU: {SystemInfo.graphicsDeviceName} ({SystemInfo.graphicsDeviceType})\n" +
                    $"Log: {Application.consoleLogPath}";
        }

        // OnGUI is the old immediate-mode UI. Fine for a debug overlay; don't build game UI with it.
        private void OnGUI() => GUI.Label(new Rect(10, 10, 900, 100), _text);
#endif
    }
}