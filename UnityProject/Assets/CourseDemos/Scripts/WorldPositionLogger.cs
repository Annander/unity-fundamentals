using UnityEngine;
using UnityEngine.InputSystem;

namespace Course.Demos
{
    [AddComponentMenu("Course/Demos/World Position Logger")]
    public sealed class WorldPositionLogger : MonoBehaviour
    {
        private void Start() => Log();

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard.pKey.wasPressedThisFrame) Log();
        }

        private void Log()
        {
            var parent = transform.parent != null ? transform.parent.name : "(none)";
            Debug.Log($"{name}: position (world) {transform.position}, localPosition (relative to {parent}) {transform.localPosition}", this);
        }
    }
}