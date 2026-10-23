using System;
using System.Collections.Generic;

namespace CSharpBasics.Exercises
{
    // EXERCISE 07 - closures
    // This code compiles and is WRONG: every reporter returns 'count' instead of its own index.
    //
    // TODO: fix it with the smallest possible change.
    public static class ClosureTasks
    {
        public static List<Func<int>> CreateIndexReporters(int count)
        {
            var result = new List<Func<int>>();
            for (var i = 0; i < count; i++)
            {
                result.Add(() => i);
            }
            return result;
        }
    }
}