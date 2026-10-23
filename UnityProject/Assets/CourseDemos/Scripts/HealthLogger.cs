using Course.Fundamentals.Core;
using UnityEngine;

namespace Course.Demos
{
    [AddComponentMenu("Course/Demos/Health Logger")]
    [RequireComponent(typeof(Health))]
    public sealed class HealthLogger : MonoBehaviour
    {
        private Health _health;

        private void Awake() => _health = GetComponent<Health>();

        private void OnEnable()
        {
            _health.Changed += OnChanged;
            _health.Died += OnDied;
        }

        private void OnDisable()
        {
            _health.Changed -= OnChanged;
            _health.Died -= OnDied;
        }

        private void OnChanged(int current, int max) => Debug.Log($"{name}: {current}/{max}", this);

        private void OnDied(Health health) => Debug.Log($"{name}: died", this);
    }
}