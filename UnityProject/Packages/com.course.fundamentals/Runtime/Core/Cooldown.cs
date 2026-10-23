using System;
using UnityEngine;

namespace Course.Fundamentals.Core
{
    [Serializable]
    public sealed class Cooldown
    {
        [SerializeField, Min(0f)] private float duration = 1f;
        private float _readyAt;

        public Cooldown() {}

        public Cooldown(float duration)
        {
            this.duration = duration;
        }

        public float Duration => duration;

        public bool IsReady(float now) => now >= _readyAt;

        public bool TryConsume(float now)
        {
            if (!IsReady(now))
                return false;

            _readyAt = now + duration;
            return true;
        }

        public void Reset() => _readyAt = 0f;
    }
}