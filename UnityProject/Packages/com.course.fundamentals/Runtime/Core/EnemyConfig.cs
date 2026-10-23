using UnityEngine;

namespace Course.Fundamentals.Core
{
    [CreateAssetMenu(menuName = "Course/Enemy Config", fileName = "NewEnemyConfig")]
    public sealed class EnemyConfig : ScriptableObject
    {
        [SerializeField, Min(1)] private int maxHealth = 50;
        [SerializeField, Min(0f)] private float moveSpeed = 3.5f;
        [SerializeField, Min(0f)] private float attackCooldown = 1.2f;
        [SerializeField] private GameObject prefab = null;

        // Read-only properties: other code can read the config but not change it.
        public int MaxHealth => maxHealth;
        public float MoveSpeed => moveSpeed;
        public float AttackCooldown => attackCooldown;
        public GameObject Prefab => prefab;
    }
}