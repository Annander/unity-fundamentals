using UnityEngine;
using UnityEngine.InputSystem;

namespace Course.Demos
{
    [AddComponentMenu("Course/Demos/Debugging Demo")]
    public sealed class DebuggingDemo : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float radius = 2f;

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.lKey.wasPressedThisFrame) Debug.Log($"{name}: radius is {radius}. Click me to find the object.", this);
            if (keyboard.wKey.wasPressedThisFrame) Debug.LogWarning($"{name}: radius {radius} looks small.", this);
            if (keyboard.eKey.wasPressedThisFrame) Debug.LogError("No target!", this);
        }

        // Editor-only drawing; Gizmos cost nothing in a build.
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}