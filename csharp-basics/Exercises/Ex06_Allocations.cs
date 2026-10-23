using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpBasics.Exercises
{
    // EXERCISE 06 - remove allocations from a hot path
    //
    // TODO: implement SumOfEvenSquares with the same result as the LINQ version but ZERO
    //       allocations. The check measures it with GC.GetAllocatedBytesForCurrentThread().
    // Things that would allocate: LINQ, lambdas capturing locals, foreach over IEnumerable<int>,
    // ToList()/ToArray(). Things that don't: for loops, foreach over List<T> directly.
    public static class AllocationTasks
    {
        // Reference implementation. Readable, correct, and allocates every call.
        public static int SumOfEvenSquaresLinq(IEnumerable<int> numbers) => numbers.Where(n => n % 2 == 0).Select(n => n * n).Sum();

        public static int SumOfEvenSquares(List<int> numbers)
        {
            throw new NotImplementedException();
        }
    }
}