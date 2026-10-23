using System;
using System.Collections.Generic;

namespace CSharpBasics.Examples
{
    // A generic type: written once, works for any T, fully type-safe, no casts, no boxing.
    public sealed class RingBuffer<T>
    {
        private readonly T[] _items;
        private int _next;
        public int Count { get; private set; }

        public RingBuffer(int capacity) { _items = new T[capacity]; }

        public void Push(T item)
        {
            _items[_next] = item;
            _next = (_next + 1) % _items.Length;
            if (Count < _items.Length) Count++;
        }

        public T this[int index] => _items[(_next - Count + index + _items.Length) % _items.Length];
    }

    public static class Generics
    {
        // Generic method with a CONSTRAINT: T must be comparable to itself.
        public static T Max<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b) >= 0 ? a : b;

        // Constraint "class, new()": reference type with a parameterless constructor.
        public static T CreateOrReuse<T>(Stack<T> cache) where T : class, new() => cache.Count > 0 ? cache.Pop() : new T();

        public static void Run()
        {
            Console.WriteLine($"Max(3, 7) = {Max(3, 7)}");             // T inferred as int
            Console.WriteLine($"Max(\"a\", \"b\") = {Max("a", "b")}"); // T inferred as string

            var lastPositions = new RingBuffer<float>(3);
            for (int i = 1; i <= 5; i++) lastPositions.Push(i * 1.5f);
            Console.Write("Ring buffer keeps the last 3: ");
            for (int i = 0; i < lastPositions.Count; i++) Console.Write($"[{lastPositions[i]}] ");
            Console.WriteLine();

            var cache = new Stack<List<int>>();
            var list = CreateOrReuse(cache);
            Console.WriteLine($"Created a {list.GetType().Name}");

            // Unity APIs you'll meet that are generic:
            //   GetComponent<T>(), TryGetComponent<T>(out T), FindAnyObjectByType<T>(),
            //   ObjectPool<T>, List<T>, Dictionary<TKey,TValue>, Action<T>, UnityEvent<T>.
        }
    }
}