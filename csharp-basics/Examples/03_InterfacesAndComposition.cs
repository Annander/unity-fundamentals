using System;
using System.Collections.Generic;

namespace CSharpBasics.Examples
{
    // Interface: a contract ("can do X"), no state.
    public interface IDamageable
    {
        void ApplyDamage(int amount);
    }

    // Inheritance: "is a".
    public abstract class Enemy : IDamageable
    {
        public int Health { get; protected set; } = 100;

        public virtual void ApplyDamage(int amount) => Health -= amount;

        // abstract: subclasses MUST implement.
        public abstract string Attack();
    }

    public class Goblin : Enemy
    {
        public override string Attack() => "Goblin stabs";
    }

    public class ArmoredGoblin : Goblin
    {
        // override + base call
        public override void ApplyDamage(int amount) => base.ApplyDamage(amount / 2);
    }

    // The inheritance trap: now design wants a FLYING armored goblin that can also SWIM.
    // FlyingArmoredSwimmingGoblin : ArmoredGoblin ...?  C# has single inheritance of classes,
    // so deep hierarchies collapse under combinations. This is exactly why Unity uses components.

    // Composition: "has a". Behaviour is assembled from small parts.
    public interface IMovement { string Move(); }
    public sealed class Walk : IMovement { public string Move() => "walks"; }
    public sealed class Fly : IMovement { public string Move() => "flies"; }
    public sealed class Swim : IMovement { public string Move() => "swims"; }

    public sealed class Armor
    {
        public int Reduce(int damage) => damage / 2;
    }

    public sealed class Creature : IDamageable
    {
        private readonly List<IMovement> _movements = new List<IMovement>();
        private readonly Armor _armor; // may be null: "no armour" is just "no component"
        public string Name { get; }
        public int Health { get; private set; } = 100;

        public Creature(string name, Armor armor, params IMovement[] movements)
        {
            Name = name;
            _armor = armor;
            _movements.AddRange(movements);
        }

        public void ApplyDamage(int amount)
        {
            Health -= _armor?.Reduce(amount) ?? amount;
        }

        public string Describe()
        {
            var parts = new List<string>();
            foreach (var m in _movements)
                parts.Add(m.Move());

            return $"{Name} {string.Join(" and ", parts)}, health {Health}";
        }
    }

    public static class InterfacesAndComposition
    {
        public static void Run()
        {
            Enemy g = new ArmoredGoblin();
            g.ApplyDamage(40);
            Console.WriteLine($"{g.Attack()}; armored goblin health={g.Health} (virtual dispatch picked the override)");

            var monster = new Creature("Gorgon", new Armor(), new Walk(), new Fly(), new Swim());
            monster.ApplyDamage(40);
            Console.WriteLine(monster.Describe());

            // Program against the interface: this code doesn't care what it hits.
            IDamageable[] targets = { new Goblin(), monster };
            foreach (var t in targets) t.ApplyDamage(10);
            Console.WriteLine("Both took damage through IDamageable.");
        }
    }
}