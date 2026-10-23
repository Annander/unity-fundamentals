using Course.Fundamentals.Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Course.Fundamentals.EditorTools.Examples
{
    [CustomPropertyDrawer(typeof(MinMax))]
    public sealed class MinMaxDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var row = new VisualElement { style = { flexDirection = FlexDirection.Row } };

            row.Add(new Label(property.displayName) { style = { minWidth = 120 } });
            row.Add(new PropertyField(property.FindPropertyRelative("min"), "Min") { style = { flexGrow = 1 } });
            row.Add(new PropertyField(property.FindPropertyRelative("max"), "Max") { style = { flexGrow = 1 } });

            return row;
        }
    }
}