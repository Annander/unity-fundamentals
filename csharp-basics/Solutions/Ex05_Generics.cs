using System;
using System.Collections.Generic;

namespace CSharpBasics.Exercises
{
    // SOLUTION 05
    public sealed class SimplePool<T> where T : class, new()
    {
        private readonly Stack<T> _inactive = new ();

        // HashSet gives O(1) double-release detection. UnityEngine.Pool.ObjectPool<T> has a
        // similar safety check, switched on by its 'collectionCheck' constructor argument.
        private readonly HashSet<T> _inactiveSet = new ();

        public int CountInactive => _inactive.Count;

        public T Get()
        {
            if (_inactive.Count == 0) return new T();
            T item = _inactive.Pop();
            _inactiveSet.Remove(item);
            return item;
        }

        public void Release(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (!_inactiveSet.Add(item)) throw new InvalidOperationException("Item was already released to the pool.");
            _inactive.Push(item);
        }
    }
}