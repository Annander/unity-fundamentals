using UnityEngine;

namespace Course.Fundamentals.Core
{
    [AddComponentMenu("Course/Lifecycle Logger")]
    public sealed class LifecycleLogger : MonoBehaviour
    {
        [SerializeField] private bool logEveryFrame;

        private bool _loggedFixedUpdate;
        private bool _loggedUpdate;
        private bool _loggedLateUpdate;

        private void Awake() => Log("Awake  (once, when the object is created/loaded - even if the component is disabled)");
        private void OnEnable() => Log("OnEnable  (every time the component/GameObject becomes active)");
        private void Start() => Log("Start  (once, before this component's first Update; only if enabled)");

        private void FixedUpdate()
        {
            if (logEveryFrame || !_loggedFixedUpdate) Log("FixedUpdate  (0..n times per frame, fixed timestep)");
            _loggedFixedUpdate = true;
        }

        private void Update()
        {
            if (logEveryFrame || !_loggedUpdate) Log("Update  (once per frame)");
            _loggedUpdate = true;
        }

        private void LateUpdate()
        {
            if (logEveryFrame || !_loggedLateUpdate) Log("LateUpdate  (once per frame, after all Updates)");
            _loggedLateUpdate = true;
        }

        private void OnDisable() => Log("OnDisable  (component/GameObject deactivated, or before OnDestroy)");
        private void OnDestroy() => Log("OnDestroy  (object destroyed, or scene unloaded / Play Mode exited)");
        private void OnApplicationQuit() => Log("OnApplicationQuit");

        private void Log(string message)
        {
            // Time.frameCount shows which callbacks share a frame.
            Debug.Log($"[frame {Time.frameCount}] {name}: {message}", this);
        }
    }
}