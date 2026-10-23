using System.Collections.Generic;
using UnityEngine;

namespace Course.Fundamentals.BestPractices
{
    public enum StatId
    {
        Strength = 0,
        Agility = 1,
        Intellect = 2,
        Count = 3, // keep last: used to size arrays
    }

    [AddComponentMenu("Course/Best Practices/String Comparison Examples")]
    public sealed class StringComparisonExamples : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private Renderer renderer = null;
        [SerializeField] private bool useBadVersion;

        // GOOD: hash once, store in static readonly fields. Lookups then use ints.
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        private MaterialPropertyBlock _block;

        // BAD: string keys in a hot path. Every lookup hashes the whole string; typos fail at runtime.
        private readonly Dictionary<string, float> _statsByName = new ()
        {
            { "Strength", 10f },
            { "Agility", 12f },
            { "Intellect", 8f },
        };

        // GOOD: enum-indexed array. Index = O(1), no hashing, typos are compile errors.
        private readonly float[] _stats = { 10f, 12f, 8f };

        private void Awake() => _block = new MaterialPropertyBlock();

        private void Update()
        {
            var t = Mathf.PingPong(Time.time, 1f);
            if (useBadVersion) UpdateBad(t);
            else UpdateGood(t);
        }

        private void UpdateBad(float t)
        {
            // BAD: string parameter name, hashed on every call.
            if (animator != null && animator.runtimeAnimatorController != null)
                animator.SetFloat("Speed", t);

            // BAD x2: string property name, AND Renderer.material creates a per-renderer material
            // copy on first access (more draw-call state, and a leak if you never Destroy it).
            if (renderer != null)
                renderer.material.SetColor("_BaseColor", Color.Lerp(Color.white, Color.red, t));

            // BAD: string-keyed lookup every frame.
            var strength = _statsByName["Strength"];
            Use(strength);
        }

        private void UpdateGood(float t)
        {
            if (animator != null && animator.runtimeAnimatorController != null)
                animator.SetFloat(SpeedHash, t);

            if (renderer != null)
            {
                // MaterialPropertyBlock: per-renderer overrides without cloning the material.
                renderer.GetPropertyBlock(_block);
                _block.SetColor(BaseColorId, Color.Lerp(Color.white, Color.red, t));
                renderer.SetPropertyBlock(_block);
            }

            var strength = _stats[(int)StatId.Strength];
            Use(strength);
        }

        // Tags
        private void OnTriggerEnter(Collider other)
        {
            if (useBadVersion)
            {
                // BAD: GameObject.tag returns a NEW managed string each call (GC alloc), and a typo in the literal silently never matches.
                if (other.tag == "Player") Debug.Log("Player entered (bad compare)");
            }
            else
            {
                // GOOD: no allocation; an undefined tag is reported as an error instead of silently failing.
                if (other.CompareTag("Player")) Debug.Log("Player entered");
            }
        }

        private float _sink;
        private void Use(float value) => _sink = value;
    }
}