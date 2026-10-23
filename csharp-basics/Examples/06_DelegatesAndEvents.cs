using System;
using System.Collections.Generic;

namespace CSharpBasics.Examples
{
    // A delegate is a type-safe reference to a method (or several). Action/Func are ready-made ones:
    //   Action            -> void ()
    //   Action<int>       -> void (int)
    //   Func<float>       -> float ()
    //   Func<int, bool>   -> bool (int)
    public sealed class HealthModel
    {
        private int _current;

        // 'event' restricts outsiders to += and -=. They can't invoke it or overwrite it with '='.
        public event Action<int, int> Changed; // (current, max)
        public event Action Died;

        public int Max { get; }

        public HealthModel(int max) { Max = max; _current = max; }

        public void Damage(int amount)
        {
            if (_current == 0) return;
            _current = Math.Max(0, _current - amount);

            // Notify listeners.
            Changed?.Invoke(_current, Max);
            if (_current == 0) Died?.Invoke();
        }
    }

    public static class DelegatesAndEvents
    {
        public static void Run()
        {
            // Plain delegates
            Func<int, int, int> add = (x, y) => x + y;
            Action<string> log = Console.WriteLine;
            log($"add(2,3)={add(2, 3)}");

            // Events: the observer pattern built in
            var health = new HealthModel(30);
            Action<int, int> onChanged = (cur, max) => Console.WriteLine($"  UI: {cur}/{max}");
            health.Changed += onChanged;
            health.Died += OnDied;

            health.Damage(10);
            health.Damage(25);

            // ALWAYS unsubscribe when the listener goes away.
            // A forgotten subscription keeps the listener alive (leak) and calls into destroyed objects (MissingReferenceException).
            health.Changed -= onChanged;
            health.Died -= OnDied;

            // Closures: a lambda that captures a local variable
            int hits = 0;
            Action hit = () => hits++;
            hit(); hit();
            Console.WriteLine($"captured variable 'hits' = {hits}");

            // Classic loop-capture surprise
            var actions = new List<Action>();
            for (int i = 0; i < 3; i++) actions.Add(() => Console.Write($"{i} "));
            Console.Write("for-loop capture prints: ");
            foreach (var a in actions) a();
            Console.WriteLine();
        }

        private static void OnDied() => Console.WriteLine("  Died event received");
    }
}