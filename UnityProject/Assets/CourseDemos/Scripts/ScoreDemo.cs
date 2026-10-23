using Course.Fundamentals.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Course.Demos
{
    [AddComponentMenu("Course/Demos/Score Demo")]
    public sealed class ScoreDemo : MonoBehaviour
    {
        [SerializeField, Min(1)] private int pointsPerPress = 10;

        private static int _pressesNeverReset;

        private void OnEnable() => ScoreService.ScoreChanged += OnScoreChanged;
        private void OnDisable() => ScoreService.ScoreChanged -= OnScoreChanged;

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null || !keyboard.sKey.wasPressedThisFrame) return;

            _pressesNeverReset++;
            ScoreService.Add(pointsPerPress);
        }

        private void OnScoreChanged(int score)
        {
            Debug.Log($"ScoreService.Score = {score} (reset every Play session); " +
                      $"static presses without reset = {_pressesNeverReset}", this);
        }
    }
}