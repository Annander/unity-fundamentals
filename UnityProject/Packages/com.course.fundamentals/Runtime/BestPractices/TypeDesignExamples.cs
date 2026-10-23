using UnityEngine;

namespace Course.Fundamentals.BestPractices
{
    // BAD: a mutable struct passed around. Mutating a copy is a silent no-op bug.
    public struct DamageInfoMutable
    {
        public int Amount;
        public Vector3 Point;
        public int SourceId;
        public float Scaled(float multiplier) => Amount * multiplier;
    }

    // GOOD: readonly struct. Immutable (no copy bugs), and 'in' parameters need no defensive copies.
    public readonly struct DamageInfo
    {
        public readonly int Amount;
        public readonly Vector3 Point;
        public readonly int SourceId; // an int id, not a string name (session 06)

        public DamageInfo(int amount, Vector3 point, int sourceId)
        {
            Amount = amount;
            Point = point;
            SourceId = sourceId;
        }

        public float Scaled(float multiplier) => Amount * multiplier;

        // "With"-style copy, since C# 9 'with' expressions only work on records (and records need extra setup in Unity).
        public DamageInfo WithAmount(int amount) => new DamageInfo(amount, Point, SourceId);
    }

    public static class DamageMath
    {
        // 'in' = pass by readonly reference. Worth it for larger structs (roughly > 16 bytes) in hot code.
        public static float Mitigate(in DamageInfo info, float armor) => info.Scaled(1f - Mathf.Clamp01(armor));
    }

    // GOOD default for classes not designed for inheritance: documents intent, prevents fragile
    // subclassing, and lets compilers devirtualize some calls. Unseal deliberately, not by accident.
    public sealed class DamageLog
    {
        // static readonly array shared by all instances, instead of allocating one per call.
        private static readonly string[] SeverityNames = { "Low", "Medium", "High" };

        public static string Severity(int amount) => SeverityNames[amount < 10 ? 0 : amount < 50 ? 1 : 2];
    }

    // BAD: allocates a new array on every call.
    public static class DamageLogBad
    {
        public static string Severity(int amount)
        {
            string[] names = { "Low", "Medium", "High" };
            return names[amount < 10 ? 0 : amount < 50 ? 1 : 2];
        }
    }
}