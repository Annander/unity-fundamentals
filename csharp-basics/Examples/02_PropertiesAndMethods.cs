using System;

namespace CSharpBasics.Examples
{
    public class Character
    {
        // Field: raw storage. Keep fields private.
        private int _health;

        // Property: looks like a field to callers, but is really a get/set method pair.
        // Lets you validate, raise events, or make the setter private.
        public int Health
        {
            get => _health;
            private set => _health = Math.Max(0, value); // clamp: health never goes negative
        }

        // Auto-property: the compiler creates the hidden field for you.
        public string Name { get; }

        // get-only: assignable only in the constructor
        public int Level { get; private set; } = 1;

        // Expression-bodied, computed property: no storage at all.
        public bool IsDead => Health == 0;

        // Constant vs static readonly: const is baked into callers at compile time; static readonly is not.
        public const int MaxLevel = 99;
        public static readonly TimeSpan RespawnDelay = TimeSpan.FromSeconds(3);

        public Character(string name, int health)
        {
            Name = name;
            Health = health;
        }

        // Method with a default parameter value.
        public void TakeDamage(int amount, bool ignoreArmor = false)
        {
            int final = ignoreArmor ? amount : amount / 2;
            Health -= final;
        }

        // out parameter: the "Try" pattern. Return success, hand back the result via out.
        public bool TryLevelUp(out int newLevel)
        {
            if (Level >= MaxLevel) { newLevel = Level; return false; }
            Level++;
            newLevel = Level;
            return true;
        }

        // Overloading: same name, different parameter lists.
        public void Heal() => Heal(10);
        public void Heal(int amount) => Health += amount;

        // Static method: belongs to the type, not an instance.
        public static Character CreateDefault() => new Character("Nobody", 100);
    }

    public static class PropertiesAndMethods
    {
        public static void Run()
        {
            var hero = new Character("Hero", 100);
            hero.TakeDamage(40);                       // 20 after armour
            hero.TakeDamage(40, ignoreArmor: true);    // named argument for readability
            Console.WriteLine($"{hero.Name} health={hero.Health} dead={hero.IsDead}");

            if (hero.TryLevelUp(out int level))
                Console.WriteLine($"Levelled up to {level}");

            // hero.Health = 5;  // compile error: setter is private. That's the point.

            var nobody = Character.CreateDefault();
            Console.WriteLine($"Default: {nobody.Name} {nobody.Health}");
        }
    }
}