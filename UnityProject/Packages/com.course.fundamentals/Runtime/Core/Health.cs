using System;
using UnityEngine;
using UnityEngine.Events;

namespace Course.Fundamentals.Core
{
    [AddComponentMenu("Course/Health")]
    [DisallowMultipleComponent]
    public sealed class Health : MonoBehaviour, IDamageable
    {
        [SerializeField, Min(1)] private int max = 100;
        [SerializeField] private UnityEvent onDied = new ();
        [SerializeField] private GameEventChannel diedChannel;

        public event Action<int, int> Changed;
        public event Action<Health> Died;

        public int Max => max;
        public int Current { get; private set; }
        public bool IsDead => Current <= 0;

        private void Awake() => Current = max;

        public void ApplyDamage(int amount)
        {
            if (amount <= 0 || IsDead) return;
            Current = Mathf.Max(0, Current - amount);
            Changed?.Invoke(Current, max);
            if (Current == 0) Die();
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || IsDead) return;
            Current = Mathf.Min(max, Current + amount);
            Changed?.Invoke(Current, max);
        }

        private void Die()
        {
            Died?.Invoke(this);
            onDied.Invoke();
            // UnityEngine.Object: use '!= null' (or implicit bool), never '?.' - see FakeNullDemo.
            if (diedChannel != null) diedChannel.Raise();
        }

        // Editor-only callback: runs when values change in the Inspector. Keep it cheap and side-effect free.
        private void OnValidate() => max = Mathf.Max(1, max);
    }
}