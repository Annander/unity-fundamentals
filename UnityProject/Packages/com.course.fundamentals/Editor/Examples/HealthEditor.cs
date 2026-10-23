using Course.Fundamentals.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Course.Fundamentals.EditorTools.Examples
{
    [CustomEditor(typeof(Health))]
    public sealed class HealthEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            // Everything Health would show anyway: undo and prefab overrides keep working.
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // Only does something in Play Mode, where Health has run Awake.
            root.Add(new Button(() => ((Health)target).ApplyDamage(10)) { text = "Damage 10 (Play Mode)" });

            return root;
        }
    }
}