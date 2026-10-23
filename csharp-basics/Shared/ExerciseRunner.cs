using System;
using System.Collections.Generic;

namespace CSharpBasics.Exercises
{
    // Shared by the Exercises and Solutions projects. Do not edit while doing the exercises.
    // Run Exercises (F5 in your IDE, or double-click Exercises.exe) to grade your work.
    // Run Solutions the same way to grade the reference answers: all should pass.
    public static class ExerciseRunner
    {
        public static int Main()
        {
            Console.WriteLine("Checking the exercises. TODO = not started, FAIL = not right yet, PASS = done.");
            Console.WriteLine();

            var checks = new (string Name, Action Check)[]
            {
                ("Ex01 value types (Vector2i)", ExerciseChecks.Ex01),
                ("Ex02 collections (Inventory)", ExerciseChecks.Ex02),
                ("Ex03 interfaces + composition (Shield)", ExerciseChecks.Ex03),
                ("Ex04 events (Countdown)", ExerciseChecks.Ex04),
                ("Ex05 generics (SimplePool<T>)", ExerciseChecks.Ex05),
                ("Ex06 zero-allocation loop", ExerciseChecks.Ex06),
                ("Ex07 closures (loop capture bug)", ExerciseChecks.Ex07),
            };

            int passed = 0;
            foreach (var (name, check) in checks)
            {
                try
                {
                    check();
                    Console.WriteLine($"PASS  {name}");
                    passed++;
                }
                catch (NotImplementedException)
                {
                    Console.WriteLine($"TODO  {name}");
                }
                catch (CheckFailedException e)
                {
                    Console.WriteLine($"FAIL  {name}: {e.Message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"FAIL  {name}: unexpected {e.GetType().Name}: {e.Message}");
                }
            }

            Console.WriteLine($"\n{passed}/{checks.Length} passed");

            // Keep the window open when started from the IDE or by double-clicking.
            Console.Write("\nPress Enter to close...");
            Console.ReadLine();

            return passed == checks.Length ? 0 : 1;
        }
    }

    public sealed class CheckFailedException : Exception
    {
        public CheckFailedException(string message) : base(message) { }
    }

    internal static class Assert
    {
        public static void That(bool condition, string message)
        {
            if (!condition) throw new CheckFailedException(message);
        }

        public static void Equal<T>(T expected, T actual, string what)
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
                throw new CheckFailedException($"{what}: expected {expected}, got {actual}");
        }

        public static void Throws<TException>(Action action, string what) where TException : Exception
        {
            try { action(); }
            catch (TException) { return; }
            throw new CheckFailedException($"{what}: expected {typeof(TException).Name}");
        }
    }
}
