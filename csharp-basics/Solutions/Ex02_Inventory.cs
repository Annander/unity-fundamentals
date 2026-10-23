using System;
using System.Collections.Generic;

namespace CSharpBasics.Exercises
{
    // SOLUTION 02
    public sealed class Inventory
    {
        private readonly Dictionary<int, int> _counts = new Dictionary<int, int>();

        public int DistinctItems => _counts.Count;

        public void Add(int itemId, int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Must be positive");
            _counts.TryGetValue(itemId, out var current); // current = 0 if missing
            _counts[itemId] = current + count;
        }

        public int CountOf(int itemId) => _counts.GetValueOrDefault(itemId, 0);

        public bool TryRemove(int itemId, int count)
        {
            if (count <= 0) return false;
            if (!_counts.TryGetValue(itemId, out int current) || current < count) return false;

            var left = current - count;
            if (left == 0) _counts.Remove(itemId);
            else _counts[itemId] = left;
            return true;
        }
    }
}