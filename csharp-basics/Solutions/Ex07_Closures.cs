using System;
using System.Collections.Generic;

namespace CSharpBasics.Exercises
{
    // SOLUTION 07
    // WHY it was broken: a 'for' loop declares ONE variable 'i' for the whole loop. Every lambda
    // captured that same variable (not its value), so after the loop they all read its final value.
    // FIX: copy into a variable declared INSIDE the loop body - a fresh variable per iteration.
    public static class ClosureTasks
    {
        public static List<Func<int>> CreateIndexReporters(int count)
        {
            var result = new List<Func<int>>();
            for (var i = 0; i < count; i++)
            {
                var index = i;
                result.Add(() => index);
            }
            return result;
        }
    }
}