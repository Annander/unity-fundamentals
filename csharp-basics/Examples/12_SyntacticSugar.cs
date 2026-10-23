using System;
using System.Collections.Generic;

namespace CSharpBasics.Examples
{
    // Syntactic sugar: shorter ways to write things you could already write.
    // Each section shows the LONG form first, then the SUGAR. They compile to (nearly) the same thing.
    // Everything here works in Unity (C# 9). Unity caveats are marked "UNITY:".
    public static class SyntacticSugar
    {
        public static void Run()
        {
            Conditional();
            NullOperators();
            EventInvoke();
            CompoundAssignment();
            Initializers();
            StringsAndNames();
            TuplesAndDiscards();
            PatternsAndSwitch();
            IndexFromEnd();
            ExpressionBodies();
            UsingDeclaration();
        }

        // ---- 1. Conditional (ternary) operator: condition ? whenTrue : whenFalse ----
        private static void Conditional()
        {
            var health = 15;

            // Long form
            string state;
            if (health > 0) state = "alive";
            else state = "dead";

            // Sugar
            state = health > 0 ? "alive" : "dead";
            Console.WriteLine($"ternary: {state}");

            // Can nest, but stop at one level. Past that, use if/else or a switch expression (section 8).
            string band = health > 50 ? "healthy" : health > 0 ? "hurt" : "dead";
            Console.WriteLine($"nested ternary: {band}");
        }

        // ---- 2. Null operators: ?.  ??  ??=  ?[] ----
        private sealed class Weapon { public string Name = "Shock Rifle"; }

        private static void NullOperators()
        {
            Weapon weapon = null;
            int[] ammo = null;

            // Long form
            string name;
            if (weapon != null) name = weapon.Name;
            else name = "(unarmed)";

            // Sugar: ?. returns null instead of throwing; ?? supplies a fallback.
            name = weapon?.Name ?? "(unarmed)";
            Console.WriteLine($"?. and ??: {name}");

            // ?[] is ?. for indexers. int? because the result may be null.
            int? firstClip = ammo?[0];
            Console.WriteLine($"?[]: {firstClip?.ToString() ?? "null"}");

            // ??= assigns only if currently null. Great for lazy creation.
            weapon ??= new Weapon();
            Console.WriteLine($"??=: {weapon.Name}");

            // UNITY: do NOT use ?. ?? ??= or 'is null' on anything deriving from UnityEngine.Object
            // (GameObject, components, ScriptableObject...). They skip Unity's "destroyed" check, so a
            // destroyed object looks alive. Use 'if (obj != null)' or 'if (obj)'. Session 03 covers why.
        }

        // ---- 3. Invoking an event or delegate: event?.Invoke() ----
        private sealed class Health
        {
            public event Action<int> Damaged;   // null until someone subscribes

            public void TakeDamage(int amount)
            {
                // Long form: copy to a local first, so it can't become null between the check and the call.
                Action<int> handler = Damaged;
                if (handler != null) handler(amount);

                // Sugar: exactly the same, in one line. Note: ?.Invoke, not ?.() .
                Damaged?.Invoke(amount);
            }
        }

        private static void EventInvoke()
        {
            var health = new Health();
            health.TakeDamage(5);                                    // no subscribers: nothing happens, no crash
            health.Damaged += amount => Console.WriteLine($"event?.Invoke: took {amount}");
            health.TakeDamage(10);                                   // printed twice: long form + sugar

            // Fine in Unity: C# events and delegates aren't UnityEngine.Objects, so ?. is safe here.
            // UnityEvent is different: it's never null once serialized, so call _onDied.Invoke() directly.
        }

        // ---- 4. Compound assignment and increment ----
        private static void CompoundAssignment()
        {
            int score = 0;
            score = score + 10;   // long form
            score += 10;          // sugar. Also -= *= /= %= &= |= ^= <<= >>=
            score++;              // += 1
            Console.WriteLine($"compound: {score}");

            // ++ before or after matters only when you use the value in the same expression:
            int i = 5;
            Console.WriteLine($"i++ gives {i++}, then i is {i}; ++i gives {++i}");
        }

        // ---- 5. var, target-typed new, object and collection initializers ----
        private sealed class Enemy
        {
            public string Name;
            public int Health = 100;
        }

        private static void Initializers()
        {
            // Long form
            Dictionary<string, int> longScores = new Dictionary<string, int>();
            longScores.Add("Malcolm", 20);
            Enemy longEnemy = new Enemy();
            longEnemy.Name = "Xan";
            longEnemy.Health = 250;

            // Sugar: var infers the type from the right-hand side.
            var scores = new Dictionary<string, int> { ["Malcolm"] = 20, ["Brock"] = 15 };
            var enemy = new Enemy { Name = "Xan", Health = 250 };

            // Target-typed new (C# 9): the type is on the left, so 'new()' is enough.
            List<Enemy> enemies = new() { enemy, new Enemy { Name = "Loque" } };
            Console.WriteLine($"initializers: {scores.Count} scores, {enemies.Count} enemies, first is {enemies[0].Name}");

            // Use var when the type is obvious from the right-hand side; spell it out when it isn't.
            // UNITY: 'record' types and 'init' setters are C# 9 but need a runtime type Unity doesn't ship.
            // Don't use them in Unity code.
        }

        // ---- 6. String interpolation and nameof ----
        private static void StringsAndNames()
        {
            int frags = 25;
            float accuracy = 0.4237f;

            // Long form
            string a = "Frags: " + frags + ", accuracy: " + (accuracy * 100).ToString("0.0") + "%";

            // Sugar: $"..." with {expression[:format]}
            string b = $"Frags: {frags}, accuracy: {accuracy:P1}";
            Console.WriteLine($"interpolation: {b}");

            // nameof gives the name as a string, and survives renames.
            Console.WriteLine($"nameof: {nameof(frags)}, {nameof(Enemy.Health)}");

            // UNITY: interpolation allocates a new string every time. Fine on events, not in Update().
            _ = a;
        }

        // ---- 7. Tuples, deconstruction, out var, discards ----
        private static (int Frags, int Deaths) GetStats() => (25, 7);

        private static void TuplesAndDiscards()
        {
            // Return two values without writing a class or struct; deconstruct into locals.
            var (frags, deaths) = GetStats();
            Console.WriteLine($"tuple: {frags}/{deaths}");

            // Swap two variables without a temp.
            int x = 1, y = 2;
            (x, y) = (y, x);
            Console.WriteLine($"swap: x={x} y={y}");

            // Long form
            int parsedLong;
            bool ok = int.TryParse("42", out parsedLong);

            // Sugar: declare the out variable inline.
            if (int.TryParse("42", out int parsed)) Console.WriteLine($"out var: {parsed}");

            // Discard _ : "I don't need this value".
            var (_, onlyDeaths) = GetStats();
            Console.WriteLine($"discard: deaths={onlyDeaths}");
            _ = ok; _ = parsedLong;
        }

        // ---- 8. Pattern matching and switch expressions ----
        private enum Team { Red, Blue, Spectator }

        private static void PatternsAndSwitch()
        {
            object pickup = 50;

            // Long form
            if (pickup is int)
            {
                int amountLong = (int)pickup;
                Console.WriteLine($"cast: {amountLong}");
            }

            // Sugar: test and cast in one go.
            if (pickup is int amount) Console.WriteLine($"is-pattern: {amount}");

            // Relational and logical patterns (C# 9): reads like English.
            int health = 30;
            if (health is > 0 and < 50) Console.WriteLine("pattern: hurt but alive");

            // Switch expression: a switch that returns a value. _ is the default case.
            Team team = Team.Blue;
            string color = team switch
            {
                Team.Red => "#FF0000",
                Team.Blue => "#0000FF",
                _ => "#888888",
            };
            Console.WriteLine($"switch expression: {color}");

            string band = health switch
            {
                <= 0 => "dead",
                < 50 => "hurt",
                _ => "healthy",
            };
            Console.WriteLine($"switch with patterns: {band}");
        }

        // ---- 9. Index from end ^ and ranges .. ----
        private static void IndexFromEnd()
        {
            int[] lastScores = { 10, 20, 30, 40 };

            // Long form vs sugar
            int lastLong = lastScores[lastScores.Length - 1];
            int last = lastScores[^1];   // ^1 = last, ^2 = second to last
            Console.WriteLine($"^1: {last} (same as {lastLong})");

            // Ranges: start..end (end exclusive)
            int[] middle = lastScores[1..3];   // { 20, 30 }
            Console.WriteLine($"range 1..3: {string.Join(",", middle)}");

            // UNITY: ^ is free. A range on an array ALLOCATES a new array. Keep it out of hot paths.
            // Ranges also don't work on List<T>.
        }

        // ---- 10. Expression-bodied members and auto-properties ----
        private sealed class Ammo
        {
            // Long form
            private int _maxLong = 50;
            public int MaxLong { get { return _maxLong; } }

            // Sugar: auto-property with initializer, and expression bodies (=>) for one-liners.
            public int Max { get; } = 50;
            public int Current { get; private set; } = 50;
            public bool IsEmpty => Current == 0;
            public float Fraction => (float)Current / Max;
            public void Spend() => Current = Math.Max(0, Current - 1);
        }

        private static void ExpressionBodies()
        {
            var ammo = new Ammo();
            ammo.Spend();
            Console.WriteLine($"expression bodies: {ammo.Current}/{ammo.Max}, empty={ammo.IsEmpty}, {ammo.Fraction:P0}");

            // UNITY: auto-properties are not serialized. Use [field: SerializeField] or a
            // [SerializeField] private field with a property in front of it (session 01).
        }

        // ---- 11. using declaration: dispose at the end of the scope ----
        private sealed class Timer : IDisposable
        {
            private readonly string _label;
            public Timer(string label) { _label = label; Console.WriteLine($"  start {_label}"); }
            public void Dispose() => Console.WriteLine($"  end {_label}");
        }

        private static void UsingDeclaration()
        {
            Console.WriteLine("using:");

            // Long form: a using block.
            using (new Timer("block"))
            {
                Console.WriteLine("  work");
            }

            // Sugar (C# 8): disposed automatically when the method (scope) ends. No braces needed.
            using var timer = new Timer("declaration");
            Console.WriteLine("  more work");

            // UNITY: this is exactly how ProfilerMarker.Auto() is used: 'using var _ = s_Marker.Auto();'
        }
    }
}