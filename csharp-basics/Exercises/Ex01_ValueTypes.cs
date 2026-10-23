using System;

namespace CSharpBasics.Exercises
{
    // EXERCISE 01 - value types
    // Goal: understand that structs are copied, and how to mutate elements of an array of structs.
    //
    // TODO 1: implement operator +, Equals, GetHashCode, == and != so Vector2i compares by VALUE.
    //         (Unity's Vector3 does exactly this; note its == is "approximately equal" for floats.)
    // TODO 2: implement ValueTypeTasks.MoveAll so every element of the array is actually moved.
    //         Hint: why can't you do it with foreach?
    public readonly struct Vector2i : IEquatable<Vector2i>
    {
        public readonly int X;
        public readonly int Y;

        public Vector2i(int x, int y) { X = x; Y = y; }

        public static Vector2i operator +(Vector2i a, Vector2i b) => throw new NotImplementedException();
        public static bool operator ==(Vector2i a, Vector2i b) => throw new NotImplementedException();
        public static bool operator !=(Vector2i a, Vector2i b) => throw new NotImplementedException();

        public bool Equals(Vector2i other) => throw new NotImplementedException();
        public override bool Equals(object obj) => throw new NotImplementedException();
        public override int GetHashCode() => throw new NotImplementedException();
        public override string ToString() => $"({X}, {Y})";
    }

    public static class ValueTypeTasks
    {
        public static void MoveAll(Vector2i[] points, Vector2i delta)
        {
            throw new NotImplementedException();
        }
    }
}
