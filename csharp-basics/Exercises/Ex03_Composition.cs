using System;

namespace CSharpBasics.Exercises
{
    // EXERCISE 03 - interfaces and composition
    // A Shield WRAPS another IDamageable (composition, not inheritance).
    // It absorbs damage until its capacity is used up, then passes the remainder to the wrapped target.
    //
    // TODO: implement Shield.ApplyDamage. Health is already done for you.
    public interface IDamageable
    {
        void ApplyDamage(int amount);
    }

    public sealed class Health : IDamageable
    {
        public int Current { get; private set; }
        public Health(int max) { Current = max; }
        public void ApplyDamage(int amount) => Current = Math.Max(0, Current - amount);
    }

    public sealed class Shield : IDamageable
    {
        private readonly IDamageable _inner;
        private int _remaining;

        public Shield(IDamageable inner, int capacity)
        {
            _inner = inner;
            _remaining = capacity;
        }

        public void ApplyDamage(int amount)
        {
            throw new NotImplementedException();
        }
    }
}