using Course.Fundamentals.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Course.Fundamentals.EditorTools.Examples
{
    public sealed class LevelCheckWindow : EditorWindow
    {
        [MenuItem("Course/Examples/Level Check")]
        private static void Open() => GetWindow<LevelCheckWindow>("Level Check");

        private void CreateGUI()
        {
            var result = new Label("Press Check.");
            rootVisualElement.Add(new Button(() =>
            {
                var count = Object.FindObjectsByType<Health>().Length;
                result.text = $"{count} Health component(s) in the open scenes.";
            }) { text = "Check" });

            rootVisualElement.Add(result);
        }
    }
}