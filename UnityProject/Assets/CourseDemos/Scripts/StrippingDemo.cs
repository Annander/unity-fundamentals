using System;
using UnityEngine;

namespace Course.Demos
{
    [AddComponentMenu("Course/Demos/Stripping Demo")]
    public sealed class StrippingDemo : MonoBehaviour
    {
        [SerializeField] private string typeName = "Course.Demos.SaveDataV2";

        private void Start()
        {
            var type = Type.GetType(typeName);

            if (type == null)
            {
                Debug.LogWarning($"Type '{typeName}' not found: stripped from this build? Add [Preserve] or a link.xml.", this);
                return;
            }

            var instance = Activator.CreateInstance(type);
            Debug.Log($"Type '{typeName}' found and created: {instance}", this);
        }
    }

    public sealed class SaveDataV2
    {
        public int Version = 2;

        public override string ToString() => $"SaveDataV2 (version {Version})";
    }
}