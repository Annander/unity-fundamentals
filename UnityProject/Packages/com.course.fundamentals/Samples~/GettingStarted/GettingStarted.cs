using Course.Fundamentals.Core;
using UnityEngine;

namespace Course.Samples
{
    public sealed class GettingStarted : MonoBehaviour
    {
        [SerializeField] private Health _health = null;
        [SerializeField] private GameEventChannel _anyoneDied = null;
        [SerializeField, Min(1)] private int _damagePerPress = 25;

        private void OnEnable()
        {
            if (_health != null) _health.Changed += OnHealthChanged;
            if (_anyoneDied != null) _anyoneDied.Raised += OnAnyoneDied;
        }

        private void OnDisable()
        {
            if (_health != null) _health.Changed -= OnHealthChanged;
            if (_anyoneDied != null) _anyoneDied.Raised -= OnAnyoneDied;
        }

        [ContextMenu("Deal Damage")]
        public void DealDamage()
        {
            if (_health != null) _health.ApplyDamage(_damagePerPress);
        }

        private void OnHealthChanged(int current, int max) => Debug.Log($"Health {current}/{max}", this);
        private void OnAnyoneDied() => Debug.Log("Channel: someone died", this);
    }
}
