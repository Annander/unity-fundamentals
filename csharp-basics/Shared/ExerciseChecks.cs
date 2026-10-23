using System;
using System.Collections.Generic;

namespace CSharpBasics.Exercises
{
    internal static class ExerciseChecks
    {
        public static void Ex01()
        {
            var a = new Vector2i(1, 2);
            var b = new Vector2i(10, 20);
            var sum = a + b;
            Assert.Equal(11, sum.X, "(a+b).X");
            Assert.Equal(22, sum.Y, "(a+b).Y");
            Assert.That(new Vector2i(3, 4).Equals(new Vector2i(3, 4)), "Equals should compare by value");
            Assert.That(new Vector2i(3, 4) == new Vector2i(3, 4), "== should compare by value");
            Assert.That(new Vector2i(3, 4) != new Vector2i(4, 3), "!= should compare by value");

            var points = new[] { new Vector2i(0, 0), new Vector2i(5, 5) };
            ValueTypeTasks.MoveAll(points, new Vector2i(1, -1));
            Assert.Equal(new Vector2i(1, -1), points[0], "points[0] after MoveAll");
            Assert.Equal(new Vector2i(6, 4), points[1], "points[1] after MoveAll");
        }

        public static void Ex02()
        {
            var inv = new Inventory();
            Assert.Equal(0, inv.CountOf(42), "empty inventory");
            inv.Add(42, 3);
            inv.Add(42, 2);
            inv.Add(7, 1);
            Assert.Equal(5, inv.CountOf(42), "after two adds");
            Assert.Equal(2, inv.DistinctItems, "DistinctItems");
            Assert.That(inv.TryRemove(42, 5), "TryRemove(42,5) should succeed");
            Assert.Equal(0, inv.CountOf(42), "after removing all");
            Assert.Equal(1, inv.DistinctItems, "item with count 0 must be removed from the dictionary");
            Assert.That(!inv.TryRemove(7, 2), "TryRemove more than you have must fail");
            Assert.Equal(1, inv.CountOf(7), "failed remove must not change the count");
            Assert.Throws<ArgumentOutOfRangeException>(() => inv.Add(1, 0), "Add with count <= 0");
        }

        public static void Ex03()
        {
            var health = new Health(100);
            IDamageable target = new Shield(health, 30);
            target.ApplyDamage(20); // shield 30 -> 10, health untouched
            Assert.Equal(100, health.Current, "health after first hit");
            target.ApplyDamage(25); // shield absorbs 10, 15 goes through
            Assert.Equal(85, health.Current, "health after shield breaks");
            target.ApplyDamage(5);
            Assert.Equal(80, health.Current, "health after shield gone");
        }

        public static void Ex04()
        {
            var timer = new Countdown(1.0f);
            int fired = 0;
            float lastOvershoot = -1f;
            timer.Elapsed += overshoot => { fired++; lastOvershoot = overshoot; };
            timer.Tick(0.4f);
            timer.Tick(0.4f);
            Assert.Equal(0, fired, "should not fire before duration");
            timer.Tick(0.4f);
            Assert.Equal(1, fired, "should fire once at 1.2s");
            Assert.That(Math.Abs(lastOvershoot - 0.2f) < 0.001f, $"overshoot should be ~0.2, was {lastOvershoot}");
            timer.Tick(0.9f); // 0.2 carried + 0.9 = 1.1 -> fires again (repeating timer keeps the remainder)
            Assert.Equal(2, fired, "repeating timer should carry the overshoot into the next cycle");
            var silent = new Countdown(0.5f);
            silent.Tick(1f); // no subscribers: must not throw
        }

        private sealed class Bullet { public int Uses; }

        public static void Ex05()
        {
            var pool = new SimplePool<Bullet>();
            var a = pool.Get();
            Assert.That(a != null, "Get must return an instance");
            a.Uses++;
            pool.Release(a);
            Assert.Equal(1, pool.CountInactive, "CountInactive after release");
            var b = pool.Get();
            Assert.That(ReferenceEquals(a, b), "Get should reuse the released instance");
            Assert.Equal(0, pool.CountInactive, "CountInactive after reuse");
            pool.Release(b);
            Assert.Throws<InvalidOperationException>(() => pool.Release(b), "double release");
        }

        public static void Ex06()
        {
            var numbers = new List<int>();
            for (int i = 0; i < 1000; i++) numbers.Add(i);
            int expected = AllocationTasks.SumOfEvenSquaresLinq(numbers);
            Assert.Equal(expected, AllocationTasks.SumOfEvenSquares(numbers), "result");

            AllocationTasks.SumOfEvenSquares(numbers); // warm-up
            long before = GC.GetAllocatedBytesForCurrentThread();
            for (int i = 0; i < 100; i++) AllocationTasks.SumOfEvenSquares(numbers);
            long allocated = GC.GetAllocatedBytesForCurrentThread() - before;
            Assert.Equal(0L, allocated, "bytes allocated by 100 calls");
        }

        public static void Ex07()
        {
            List<Func<int>> counters = ClosureTasks.CreateIndexReporters(3);
            Assert.Equal(3, counters.Count, "count");
            for (int i = 0; i < 3; i++) Assert.Equal(i, counters[i](), $"reporter {i}");
        }
    }
}
