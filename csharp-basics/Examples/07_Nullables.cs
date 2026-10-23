using System;

namespace CSharpBasics.Examples
{
    public static class Nullables
    {
        private sealed class Weapon { public string Name = "Axe"; }

        public static void Run()
        {
            // Value types can't be null... unless wrapped in Nullable<T>, written T?.
            int? lockedTarget = null;
            Console.WriteLine($"HasValue={lockedTarget.HasValue}, GetValueOrDefault={lockedTarget.GetValueOrDefault(-1)}");
            lockedTarget = 7;
            if (lockedTarget is int id) Console.WriteLine($"pattern match gave id={id}");

            // Reference types can be null. Null-handling operators:
            Weapon weapon = null;
            string name = weapon?.Name ?? "(unarmed)";   // ?. short-circuits, ?? supplies a fallback
            Console.WriteLine($"weapon name: {name}");
            weapon ??= new Weapon();                      // assign only if null (C# 8)
            Console.WriteLine($"after ??=: {weapon.Name}");

            if (weapon is not null) Console.WriteLine("'is not null' (C# 9 pattern)");

            // Nullable reference types (C# 8+): opt-in compile-time warnings, no runtime change.
#nullable enable
            string? maybe = null;
            Console.WriteLine($"nullable-annotated string length: {maybe?.Length ?? 0}");
#nullable restore
            // Unity supports '#nullable enable' per file. Be aware the Unity API itself is not annotated,
            // and serialized fields are assigned by Unity, not your constructor, so the analysis
            // is weaker in MonoBehaviours than in plain C# code.
        }
    }
}