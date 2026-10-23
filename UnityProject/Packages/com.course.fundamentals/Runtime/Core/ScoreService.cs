using System;
using UnityEngine;

namespace Course.Fundamentals.Core
{
    public static class ScoreService
    {
        public static int Score { get; private set; }
        public static event Action<int> ScoreChanged;

        public static void Add(int points)
        {
            Score += points;
            ScoreChanged?.Invoke(Score);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            Score = 0;
            ScoreChanged = null;
        }
    }
}