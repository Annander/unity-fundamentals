using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpBasics.Examples
{
    public static class Linq
    {
        private sealed class Unit
        {
            public string Name;
            public int Team;
            public float Distance;
        }

        public static void Run()
        {
            var units = new List<Unit>
            {
                new () { Name = "A", Team = 1, Distance = 12f },
                new () { Name = "B", Team = 2, Distance = 3f },
                new () { Name = "C", Team = 2, Distance = 8f },
                new () { Name = "D", Team = 2, Distance = 30f },
            };

            // LINQ: declarative, readable. Great for tools, editor scripts, loading, tests.
            var nearestEnemies = units
                .Where(u => u.Team != 1)
                .OrderBy(u => u.Distance)
                .Take(2)
                .Select(u => u.Name)
                .ToList();
            Console.WriteLine($"LINQ nearest enemies: {string.Join(", ", nearestEnemies)}");

            // Deferred execution: a query is re-evaluated every time you enumerate it.
            IEnumerable<Unit> query = units.Where(u => u.Distance < 10f);
            units.Add(new Unit { Name = "E", Team = 2, Distance = 1f });
            Console.WriteLine($"Deferred query sees later additions: count={query.Count()}");

            // Why NOT in hot paths (Update, per-frame AI, physics callbacks):
            //  - Allocates: iterator objects, delegates/closures, ToList() buffers (see topic 9).
            //  - Hides cost: OrderBy is a full sort; Count() on an IEnumerable may walk everything.
            // The loop version: same result, zero allocations if 'result' is a reused field.
            Unit best = null;
            foreach (var u in units)
            {
                if (u.Team == 1) continue;
                if (best == null || u.Distance < best.Distance) best = u;
            }
            Console.WriteLine($"Loop nearest enemy: {best?.Name}");
        }
    }
}