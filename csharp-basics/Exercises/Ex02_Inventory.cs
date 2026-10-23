using System;
using System.Collections.Generic;

namespace CSharpBasics.Exercises
{
    // EXERCISE 02 - collections and properties
    // Build an inventory keyed by integer item IDs (not strings!).
    //
    // TODO: implement every member. Rules:
    //   - Add(itemId, count): count must be > 0, otherwise throw ArgumentOutOfRangeException.
    //   - CountOf(itemId): 0 for unknown items. Use TryGetValue, not ContainsKey + indexer.
    //   - TryRemove(itemId, count): false (and no change) if there aren't enough;
    //     when an item reaches 0 remove the key entirely.
    //   - DistinctItems: number of different items currently held (read-only property).
    public sealed class Inventory
    {
        private readonly Dictionary<int, int> _counts = new ();

        public int DistinctItems => throw new NotImplementedException();

        public void Add(int itemId, int count) => throw new NotImplementedException();

        public int CountOf(int itemId) => throw new NotImplementedException();

        public bool TryRemove(int itemId, int count) => throw new NotImplementedException();
    }
}