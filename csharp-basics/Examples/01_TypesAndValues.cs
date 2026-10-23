using System;

namespace CSharpBasics.Examples
{
    // A struct is a VALUE type: assignment copies the whole thing.
    // Unity's Vector3, Quaternion, Color, RaycastHit are all structs.
    public struct PointStruct
    {
        public float X;
        public float Y;
        public PointStruct(float x, float y) { X = x; Y = y; }
        public override string ToString() => $"({X}, {Y})";
    }

    // A class is a REFERENCE type: assignment copies the reference (both names point at one object).
    // Every MonoBehaviour, GameObject and ScriptableObject is a class.
    public class PointClass
    {
        public float X;
        public float Y;
        public PointClass(float x, float y) { X = x; Y = y; }
        public override string ToString() => $"({X}, {Y})";
    }

    public static class TypesAndValues
    {
        public static void Run()
        {
            // Built-in value types: int, float, double, bool, char, enums, structs.
            // Built-in reference types: string, arrays, classes, delegates.
            int a = 5;
            int b = a;   // copy
            b++;
            Console.WriteLine($"int: a={a}, b={b}  (b is a copy)");

            var s1 = new PointStruct(1, 1);
            var s2 = s1; // copies X and Y
            s2.X = 99;
            Console.WriteLine($"struct: s1={s1}, s2={s2}  (independent copies)");

            var c1 = new PointClass(1, 1);
            var c2 = c1; // copies the reference only
            c2.X = 99;
            Console.WriteLine($"class:  c1={c1}, c2={c2}  (same object!)");

            // The Unity gotcha this explains: transform.position returns a COPY of a Vector3.
            //   transform.position.x = 5;  // compile error CS1612: you'd be modifying a temporary copy
            // Correct:
            //   Vector3 p = transform.position; p.x = 5; transform.position = p;
            PointStruct[] points = { new PointStruct(0, 0) };
            foreach (var p in points)
            {
                // p.X = 1; // compile error: foreach iteration variable is read-only (and a copy anyway)
            }
            points[0].X = 1; // indexing an ARRAY gives you the element itself, so this works
            Console.WriteLine($"array element mutated in place: {points[0]}");

            // Passing to methods: value types are copied unless you use ref/out/in.
            var moved = new PointStruct(0, 0);
            MoveByValue(moved);
            Console.WriteLine($"after MoveByValue: {moved}");
            MoveByRef(ref moved);
            Console.WriteLine($"after MoveByRef:   {moved}");

            // string is a reference type that BEHAVES like a value: it's immutable.
            // Every "modification" creates a new string (= an allocation; see topic 9).
            string name = "Ada";
            string other = name;
            other += " Lovelace";
            Console.WriteLine($"string: name='{name}', other='{other}'");

            // When to pick struct: small (rule of thumb <= 16-24 bytes), short-lived, logically a value
            // (a position, a colour, a damage packet). Otherwise class.
        }

        private static void MoveByValue(PointStruct p) { p.X += 10; }
        private static void MoveByRef(ref PointStruct p) { p.X += 10; }
    }
}
