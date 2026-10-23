using System;

namespace CSharpBasics.Exercises
{
    // SOLUTION 01
    public readonly struct Vector2i : IEquatable<Vector2i>
    {
        public readonly int X;
        public readonly int Y;

        public Vector2i(int x, int y) { X = x; Y = y; }

        public static Vector2i operator +(Vector2i a, Vector2i b) => new Vector2i(a.X + b.X, a.Y + b.Y);
        public static bool operator ==(Vector2i a, Vector2i b) => a.Equals(b);
        public static bool operator !=(Vector2i a, Vector2i b) => !a.Equals(b);

        // IEquatable<T>.Equals avoids boxing when used by Dictionary/HashSet/List.Contains.
        public bool Equals(Vector2i other) => X == other.X && Y == other.Y;
        // object.Equals override: boxes, kept only for completeness/correctness.
        public override bool Equals(object obj) => obj is Vector2i other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y); // HashCode exists in .NET Standard 2.1 too
        public override string ToString() => $"({X}, {Y})";
    }

    public static class ValueTypeTasks
    {
        public static void MoveAll(Vector2i[] points, Vector2i delta)
        {
            // foreach gives a read-only COPY of each element. Index into the array instead:
            // points[i] refers to the storage slot itself.
            for (int i = 0; i < points.Length; i++)
                points[i] = points[i] + delta;
            // Note: with a List<Vector2i>, list[i] returns a copy (the indexer is a method),
            // so 'list[i].X = 1' doesn't compile; 'list[i] = list[i] + delta' does.
        }
    }
}
