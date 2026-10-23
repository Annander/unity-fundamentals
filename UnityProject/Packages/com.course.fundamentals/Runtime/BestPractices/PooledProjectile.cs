using UnityEngine;
using UnityEngine.Pool;

namespace Course.Fundamentals.BestPractices
{
    /// A projectile that returns itself to its pool instead of being destroyed.
    /// Pooled objects must RESET their state on reuse: OnEnable runs again on every Get().
    [AddComponentMenu("Course/Best Practices/Pooled Projectile")]
    public sealed class PooledProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 20f;
        [SerializeField] private float lifetime = 2f;

        private IObjectPool<PooledProjectile> _pool;
        private float _despawnAt;
        private bool _released;

        public void SetPool(IObjectPool<PooledProjectile> pool) => _pool = pool;

        private void OnEnable()
        {
            _despawnAt = Time.time + lifetime;
            _released = false;
        }

        private void Update()
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
            if (Time.time >= _despawnAt) Despawn();
        }

        public void Despawn()
        {
            if (_released) return; // guard against double release (e.g. hit + timeout in the same frame)
            _released = true;

            if (_pool != null) _pool.Release(this);
            else Destroy(gameObject); // not pooled (e.g. placed in the scene by hand)
        }
    }
}