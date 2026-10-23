using System;
using System.Collections.Generic;

namespace CSharpBasics.Examples
{
    public enum ItemId
    {
        Sword = 1,
        Potion = 2,
        Key = 3
    }

    public static class Collections
    {
        public static void Run()
        {
            // Array: fixed size, fastest, serializable in Unity.
            int[] scores = new int[4];      // all zero
            scores[0] = 10;
            int[] primes = { 2, 3, 5, 7 };
            Console.WriteLine($"array length={primes.Length}, last={primes[primes.Length - 1]}");

            // List<T>: growable array. Serializable in Unity. Your default collection.
            var enemies = new List<string>(capacity: 8); // pre-size if you know roughly how many
            enemies.Add("Goblin");
            enemies.Add("Orc");
            enemies.Insert(0, "Rat");
            enemies.Remove("Orc");
            Console.WriteLine($"list count={enemies.Count}: {string.Join(", ", enemies)}");

            // Removing while iterating: iterate BACKWARDS by index (foreach would throw).
            for (int i = enemies.Count - 1; i >= 0; i--)
                if (enemies[i].StartsWith("R", StringComparison.Ordinal)) enemies.RemoveAt(i);
            Console.WriteLine($"after removal: {string.Join(", ", enemies)}");

            // Clear() keeps the capacity: reuse lists instead of allocating new ones each frame.
            enemies.Clear();

            // Dictionary<TKey,TValue>: key -> value lookup, ~O(1). NOT serialized by Unity.
            var inventory = new Dictionary<ItemId, int>();
            inventory[ItemId.Sword] = 1;    // indexer set: add or overwrite
            inventory.Add(ItemId.Potion, 3); // Add throws if the key already exists

            // TryGetValue: one lookup, no exception if missing. Prefer it over ContainsKey + indexer.
            if (inventory.TryGetValue(ItemId.Potion, out int potions))
                Console.WriteLine($"potions={potions}");
            if (!inventory.TryGetValue(ItemId.Key, out _))
                Console.WriteLine("no key");

            foreach (KeyValuePair<ItemId, int> pair in inventory)
                Console.WriteLine($"  {pair.Key}: {pair.Value}");

            // HashSet<T>: "is X in the set?" fast; Queue<T>/Stack<T>: FIFO/LIFO.
            var visited = new HashSet<int> { 1, 2 };
            Console.WriteLine($"visited.Add(2) returned {visited.Add(2)} (already present)");
            var queue = new Queue<string>();
            queue.Enqueue("first"); queue.Enqueue("second");
            Console.WriteLine($"queue dequeues '{queue.Dequeue()}'");

            // Choosing:
            //   need Inspector editing             -> array or List<T>
            //   lookup by key at runtime           -> Dictionary (build it in Awake from a serialized list)
            //   key is an enum or small int range  -> consider a plain array indexed by the int
        }
    }
}