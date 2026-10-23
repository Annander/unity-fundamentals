using Course.Fundamentals.Core;
using UnityEngine;

namespace Course.Fundamentals.BestPractices
{
    [AddComponentMenu("Course/Best Practices/Caching Examples")]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class CachingExamples : MonoBehaviour
    {
        [SerializeField] private bool useBadVersion;
        [SerializeField] private float thrust = 2f;
        [SerializeField] private Camera camera; // Don't use Camera.main - especially not in loops

        private Rigidbody _body;
        private Health _health;

        private void Awake()
        {
            // GOOD: look things up once.
            _body = GetComponent<Rigidbody>(); // guaranteed by [RequireComponent]
            TryGetComponent(out _health); // optional component: may stay null

            // Fallback to Camera.main if not assigned in inspector. This is a one-time lookup, not per frame.
            if (camera == null)
                camera = Camera.main;
        }

        private void FixedUpdate()
        {
            if (useBadVersion)
            {
                // BAD: GetComponent every physics step.
                GetComponent<Rigidbody>().AddForce(Vector3.up * thrust);

                // BAD: scene-wide search every step. Cost grows with scene size.
                var anyHealth = FindAnyObjectByType<Health>();
                if (anyHealth != null)
                    Use(anyHealth.Current);
            }
            else
            {
                _body.AddForce(Vector3.up * thrust);
                if (_health != null)
                    Use(_health.Current);
            }
        }

        private void LateUpdate()
        {
            if (camera == null) return;
            Use(Vector3.Distance(camera.transform.position, transform.position));
        }

        private float _sink;
        private void Use(float value) => _sink = value;
    }
}