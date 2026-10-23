using System;
using System.Collections.Generic;

namespace CSharpBasics.Exercises
{
    // EXERCISE 05 - generics
    // A minimal object pool. (Unity ships a production one: UnityEngine.Pool.ObjectPool<T>)
    //
    // TODO:
    //   - Get(): reuse an inactive instance if there is one, otherwise create one with 'new T()'.
    //   - Release(item): return it to the pool.
    //   - CountInactive: how many instances are waiting in the pool.
    // Hint: Stack<T> for storage; HashSet<T> (or Stack.Contains, slower) to detect double release.
    public sealed class SimplePool<T> where T : class, new()
    {
        private readonly Stack<T> _inactive = new Stack<T>();

        public int CountInactive => throw new NotImplementedException();

        public T Get() => throw new NotImplementedException();

        public void Release(T item) => throw new NotImplementedException();
    }
}