using System;
using System.Collections.Generic;

namespace CSharpBasics.Examples
{
    public static class Exceptions
    {
        public static void Run()
        {
            // Exceptions are for EXCEPTIONAL situations: bugs, broken invariants, I/O failures.
            // They are expensive to throw (stack trace capture + allocation) and hide control flow.

            // BAD: using exceptions as an "if"
            var config = new Dictionary<string, int> { ["speed"] = 5 };
            try
            {
                int jump = config["jump"];          // throws KeyNotFoundException
                Console.WriteLine(jump);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("BAD: caught KeyNotFoundException to detect a missing key");
            }

            // GOOD: the Try pattern
            if (!config.TryGetValue("jump", out int jumpValue)) jumpValue = 1;
            Console.WriteLine($"GOOD: TryGetValue fallback jump={jumpValue}");

            // Throw for programmer errors, and fail EARLY with a clear message.
            try
            {
                SetVolume(1.5f);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine($"Validation exception: {e.Message.Split('\n')[0]}");
            }

            // finally always runs: release what you acquired.
            try { Console.WriteLine("doing work"); }
            finally { Console.WriteLine("finally: cleanup ran"); }

            // 'using' = try/finally that calls Dispose().
            using (var scope = new LogScope("load level"))
            {
                Console.WriteLine("  inside scope");
            }

            // Never write 'catch (Exception) { }' - swallowed exceptions are the hardest bugs to find.
            // (This is sometimes called Pokémon Exception Handling: "Gotta catch 'em all".)
        }

        private static void SetVolume(float v)
        {
            if (v < 0f || v > 1f) throw new ArgumentOutOfRangeException(nameof(v), v, "Volume must be 0..1");
        }

        private sealed class LogScope : IDisposable
        {
            private readonly string _name;
            public LogScope(string name) { _name = name; Console.WriteLine($"begin {_name}"); }
            public void Dispose() => Console.WriteLine($"end {_name}");
        }
    }
}