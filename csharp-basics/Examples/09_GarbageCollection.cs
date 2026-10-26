using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpBasics.Examples
{
    public static class GarbageCollection
    {
        // Results are written to static "sinks" so the JIT can't prove the objects don't escape and optimise the allocation away (newer .NET runtimes can do that; Unity's can't).
        private static object s_sink;
        private static int s_intSink;

        private static readonly List<int> Numbers = Enumerable.Range(0, 100).ToList();
        private static readonly IList<int> NumbersAsInterface = Numbers;

        public static void Run()
        {
            Measure("int arithmetic", () => { s_intSink = s_intSink * 3 + 1; });
            Measure("new int[16]", () => { s_sink = new int[16]; });
            Measure("string concatenation", () => { s_sink = "Score: " + s_intSink; });
            Measure("string interpolation", () => { s_sink = $"Score: {s_intSink}"; });
            Measure("boxing int -> object", () => { s_sink = (object)s_intSink; });
            Measure("foreach over List<int>", () =>
            {
                int sum = 0;
                foreach (int n in Numbers) sum += n;
                s_intSink = sum;
            });
            Measure("foreach over IList<int>", () =>
            {
                int sum = 0;
                foreach (int n in NumbersAsInterface) sum += n;
                s_intSink = sum;
            });
            Measure("LINQ Where().Sum()", () => { s_intSink = Numbers.Where(n => n % 2 == 0).Sum(); });
            Measure("lambda capturing a local", () =>
            {
                int threshold = s_intSink;
                Func<int, bool> pred = n => n > threshold;
                s_sink = pred;
            });
            Measure("params int[] call", () => { s_intSink = SumParams(1, 2, 3); });
        }

        private static int SumParams(params int[] values)
        {
            int s = 0;
            foreach (int v in values) s += v;
            return s;
        }

        private static void Measure(string label, Action action)
        {
            action();
            const int iterations = 1000;
            long before = GC.GetAllocatedBytesForCurrentThread();
            for (int i = 0; i < iterations; i++) action();
            long after = GC.GetAllocatedBytesForCurrentThread();
            double perCall = (after - before) / (double)iterations;
            Console.WriteLine($"  {label,-28} {perCall,7:0.#} bytes/call {(perCall > 0 ? "<- allocates" : "")}");
        }
    }
}