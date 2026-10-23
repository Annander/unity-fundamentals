using System;
using System.Collections.Generic;
using Course.Fundamentals.Core;
using UnityEngine;

namespace Course.Fundamentals.BestPractices
{
    public static class ControlFlowExamples
    {
        // Missing components
        public static void DamageBad(GameObject target, int amount)
        {
            try
            {
                // Throws if Health is missing.
                target.GetComponent<Health>().ApplyDamage(amount);
            }
            catch (Exception)
            {
                // BAD: also swallows exceptions from INSIDE ApplyDamage - real bugs vanish.
            }
        }

        public static void DamageGood(GameObject target, int amount)
        {
            if (target.TryGetComponent(out Health health))
                health.ApplyDamage(amount);
        }

        // Parsing
        public static int ParseBad(string text)
        {
            try { return int.Parse(text); }
            catch (FormatException) { return 0; }
        }

        public static int ParseGood(string text) => int.TryParse(text, out var value) ? value : 0;

        // Dictionary lookups
        public static float LookupBad(Dictionary<int, float> table, int key)
        {
            try { return table[key]; }
            catch (KeyNotFoundException) { return 0f; }
        }

        public static float LookupGood(Dictionary<int, float> table, int key) => table.TryGetValue(key, out var value) ? value : 0f;

        // When DO you throw?
        // For programmer errors / broken invariants, as early as possible, with a clear message:
        public static void SetVolume(float value)
        {
            // For "expected" failures at runtime (missing save file, no network), return a result (bool + out, or a small result struct) instead of throwing.
            if (value is < 0f or > 1f)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Volume must be in 0..1");
        }
    }
}