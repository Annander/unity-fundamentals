using System;
using UnityEngine;

namespace Course.Fundamentals.Core
{
    [CreateAssetMenu(menuName = "Course/Events/Game Event Channel", fileName = "NewGameEvent")]
    public class GameEventChannel : ScriptableObject
    {
        public event Action Raised;

        public void Raise() => Raised?.Invoke();
    }
}