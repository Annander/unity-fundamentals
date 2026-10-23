using UnityEngine;

namespace Course.Fundamentals.Core
{
    [AddComponentMenu("Course/Damage On Trigger")]
    [RequireComponent(typeof(Collider))]
    public sealed class DamageOnTrigger : MonoBehaviour
    {
        [SerializeField, Min(0)] private int damage = 10;
        [SerializeField] private string targetTag = "Player";

        private void OnTriggerEnter(Collider other)
        {
            // CompareTag: no string allocation (other.tag == "..." allocates a new string every call).
            if (!other.CompareTag(targetTag)) return;

            // The collider may be on a child; the Health is often on the root/rigidbody object.
            var target = other.attachedRigidbody != null ? other.attachedRigidbody.gameObject : other.gameObject;

            // TryGetComponent: no allocation when the component is missing (unlike GetComponent in the Editor).
            if (target.TryGetComponent(out IDamageable damageable))
                damageable.ApplyDamage(damage);
        }

        // Editor-only: called when the component is added. Make the collider a trigger by default.
        private void Reset()
        {
            GetComponent<Collider>().isTrigger = true;
        }
    }
}