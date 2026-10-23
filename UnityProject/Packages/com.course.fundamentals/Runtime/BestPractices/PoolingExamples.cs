using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Course.Fundamentals.BestPractices
{
    [AddComponentMenu("Course/Best Practices/Pooling Examples")]
    public sealed class PoolingExamples : MonoBehaviour
    {
        [SerializeField] private PooledProjectile prefab = null;
        [SerializeField] private bool usePool = true;
        [SerializeField, Min(0.01f)] private float fireInterval = 0.05f;

        private ObjectPool<PooledProjectile> _pool;
        private float _nextFire;

        private void Awake()
        {
            // Lambdas here are created ONCE, so their allocation doesn't matter.
            _pool = new ObjectPool<PooledProjectile>(
                createFunc: CreateProjectile,
                actionOnGet: p => p.gameObject.SetActive(true),
                actionOnRelease: p => p.gameObject.SetActive(false),
                actionOnDestroy: p => Destroy(p.gameObject),
                collectionCheck: true,   // throws if you release the same object twice; costs a lookup per Release
                defaultCapacity: 32,
                maxSize: 256);           // beyond this, released objects are destroyed instead of kept
        }

        private PooledProjectile CreateProjectile()
        {
            var p = Instantiate(prefab);
            p.SetPool(_pool);
            return p;
        }

        private void Update()
        {
            if (!prefab || Time.time < _nextFire) return;
            _nextFire = Time.time + fireInterval;

            if (usePool)
            {
                // GOOD: reuse.
                var p = _pool.Get();
                p.transform.SetPositionAndRotation(transform.position, transform.rotation);
            }
            else
            {
                // BAD (for frequently spawned objects): Instantiate + Destroy every shot.
                // Each pair costs CPU on the main thread and creates garbage.
                var p = Instantiate(prefab, transform.position, transform.rotation);
                p.SetPool(null); // will Destroy itself
            }
        }

        private void OnDestroy() => _pool?.Clear();

        // Collection pools: temporary lists without garbage
        public int CountCollidersNearby(float radius)
        {
            // ListPool<T>.Get returns a pooled List and a disposable handle that releases it.
            using (ListPool<Collider>.Get(out List<Collider> results))
            {
                return results.Count;
            }
        }
    }
}