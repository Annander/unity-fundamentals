using UnityEngine;

namespace Course.Fundamentals.BestPractices
{
    [AddComponentMenu("Course/Best Practices/Physics Query Examples")]
    public sealed class PhysicsQueryExamples : MonoBehaviour
    {
        [SerializeField] private bool useBadVersion;
        [SerializeField, Min(0.1f)] private float radius = 5f;
        [SerializeField] private LayerMask mask = ~0; // GOOD: filter by layer in the query, not after it

        // Size the buffer for the realistic maximum. Results beyond its length are silently dropped.
        private readonly Collider[] _overlapBuffer = new Collider[32];
        private readonly RaycastHit[] _hitBuffer = new RaycastHit[16];

        private void FixedUpdate()
        {
            if (useBadVersion)
                QueryBad();
            else
                QueryGood();
        }

        private void QueryBad()
        {
            var all = Physics.OverlapSphere(transform.position, radius); // new array each call, no mask
            var enemies = 0;

            foreach (Collider c in all)
            {
                if (c.gameObject.layer == LayerMask.NameToLayer("Default"))
                    enemies++; // string lookup per item
            }

            var hits = Physics.RaycastAll(transform.position, transform.forward, 50f); // new array
            Use(enemies + hits.Length);
        }

        private void QueryGood()
        {
            var count = Physics.OverlapSphereNonAlloc(transform.position, radius, _overlapBuffer, mask, QueryTriggerInteraction.Ignore);

            if (count == _overlapBuffer.Length)
                Debug.LogWarning("Overlap buffer full: results may be truncated. Increase its size.", this);

            for (var i = 0; i < count; i++)
            {
                var c = _overlapBuffer[i];
                // ... use c
            }

            var hitCount = Physics.RaycastNonAlloc(new Ray(transform.position, transform.forward), _hitBuffer, 50f, mask, QueryTriggerInteraction.Ignore);

            // NOTE: RaycastNonAlloc results are NOT sorted by distance. Sort or scan for the nearest yourself.
            Use(count + hitCount);
        }

        private float _sink;
        private void Use(float value) => _sink = value;
    }
}