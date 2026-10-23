using UnityEngine;
using UnityEngine.InputSystem;

namespace Course.Demos
{
    [AddComponentMenu("Course/Demos/Enemy Spawner")]
    public sealed class EnemySpawner : MonoBehaviour
    {
        [Tooltip("The prefab ASSET to copy. Instantiate clones it into the open scene.")]
        [SerializeField] private GameObject prefab = null;
        [SerializeField, Min(0f)] private float radius = 3f;
        [Tooltip("Seconds before the spawned instance is destroyed. Destroy(obj, delay) schedules it.")]
        [SerializeField, Min(0.1f)] private float lifetime = 5f;

        private int _spawnCount;

        private void Update()
        {
            // Keyboard.current is null when no keyboard is connected: check before using it.
            var keyboard = Keyboard.current;
            if (keyboard == null || !keyboard.nKey.wasPressedThisFrame) return;

            if (prefab == null)
            {
                // The second argument (context) makes the Console highlight this object when clicked.
                Debug.LogWarning("EnemySpawner has no prefab assigned.", this);
                return;
            }

            var offset = Random.insideUnitCircle * radius;
            var position = transform.position + new Vector3(offset.x, 0f, offset.y);

            var spawned = Instantiate(prefab, position, Quaternion.identity);
            spawned.name = $"{prefab.name} #{++_spawnCount}";
            Destroy(spawned, lifetime);

            Debug.Log($"Spawned {spawned.name} at {position}; it will be destroyed in {lifetime} s.", spawned);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}