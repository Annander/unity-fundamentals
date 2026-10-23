using System.Collections.Generic;
using System.Linq;

namespace CSharpBasics.Exercises
{
    // SOLUTION 06
    public static class AllocationTasks
    {
        public static int SumOfEvenSquaresLinq(IEnumerable<int> numbers) => numbers.Where(n => n % 2 == 0).Select(n => n * n).Sum();

        public static int SumOfEvenSquares(List<int> numbers)
        {
            // A for loop over List<T> by index: no enumerator, no delegate, no allocation.
            // (foreach over List<T> - the concrete type - would also be allocation-free,
            //  because List<T>.Enumerator is a struct. Over IEnumerable<int> it would box.)
            var sum = 0;
            for (var i = 0; i < numbers.Count; i++)
            {
                var n = numbers[i];
                if ((n & 1) == 0) sum += n * n;
            }
            return sum;
        }
    }
}