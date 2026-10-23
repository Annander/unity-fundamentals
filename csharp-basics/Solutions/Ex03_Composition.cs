using System;

namespace CSharpBasics.Exercises
{
    // SOLUTION 03
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

    // Shield neither knows nor cares that _inner is a Health. It could wrap another Shield,
    // a destructible crate, or a test double. That's the payoff of programming to an interface.
    public sealed class Shield : IDamageable
    {
        private readonly IDamageable _inner;
        private int _remaining;

        public Shield(IDamageable inner, int capacity)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _remaining = capacity;
        }

        public void ApplyDamage(int amount)
        {
            var absorbed = Math.Min(_remaining, amount);
            _remaining -= absorbed;
            var passThrough = amount - absorbed;
            if (passThrough > 0) _inner.ApplyDamage(passThrough);
        }
    }
}