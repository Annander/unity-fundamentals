using Course.Fundamentals.Core;
using UnityEngine;

namespace Course.Demos
{
    [AddComponentMenu("Course/Demos/Configured Enemy")]
    public sealed class ConfiguredEnemy : MonoBehaviour
    {
        [SerializeField] private EnemyConfig config = null;
        [Tooltip("How far the enemy walks back and forth along its local X axis.")]
        [SerializeField, Min(0f)] private float patrolDistance = 2f;

        private float _speed;
        private Vector3 _start;

        private void Awake()
        {
            if (config == null)
            {
                Debug.LogWarning($"{name} has no EnemyConfig assigned.", this);
                enabled = false;
                return;
            }

            _speed = config.MoveSpeed;
            _start = transform.position;
            Debug.Log($"{name} uses '{config.name}': MaxHealth {config.MaxHealth}, MoveSpeed {config.MoveSpeed}", this);
        }

        private void Update()
        {
            var offset = Mathf.PingPong(Time.time * _speed, patrolDistance);
            transform.position = _start + transform.right * offset;
        }
    }
}