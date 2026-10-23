using System;
using System.Collections.Generic;
using UnityEngine;

namespace Course.Fundamentals.Core
{
    public interface IAbilityEffect
    {
        void Apply(GameObject target);
    }

    [Serializable]
    public sealed class DamageEffect : IAbilityEffect
    {
        [SerializeField] private int amount = 10;

        public void Apply(GameObject target)
        {
            if (target.TryGetComponent(out IDamageable damageable)) damageable.ApplyDamage(amount);
        }
    }

    [Serializable]
    public sealed class HealEffect : IAbilityEffect
    {
        [SerializeField] private int amount = 5;

        public void Apply(GameObject target)
        {
            if (target.TryGetComponent(out Health health)) health.Heal(amount);
        }
    }

    [Serializable]
    public struct DropEntry
    {
        public string debugName;
        [Range(0f, 1f)] public float chance;
    }

    [AddComponentMenu("Course/Serialization Examples")]
    public sealed class SerializationExamples : MonoBehaviour
    {
        // SERIALIZED (public). Works, but prefer the next line.
        public int publicField = 1;

        // SERIALIZED. Encapsulated + editable. Team default.
        [SerializeField] private float privateField = 2f;

        // not serialized (private, no attribute)
        private float _plainPrivate = 3f;

        // not serialized ([NonSerialized])
        [NonSerialized] public float PublicButSkipped = 4f;

        // never serialized (static)
        public static int StaticField = 5;

        // never serialized (const)
        public const int ConstField = 6;

        // never serialized (readonly)
        public readonly int ReadonlyField = 7;

        // not serialized (properties never are)
        public int PlainProperty { get; set; }

        // SERIALIZED (the hidden backing field)
        [field: SerializeField] public int BackedProperty { get; private set; } = 8;

        // SERIALIZED: List of [Serializable] struct
        [SerializeField] private List<DropEntry> drops = new ();

        // SERIALIZED: arrays too
        [SerializeField] private DropEntry[] dropArray = Array.Empty<DropEntry>();

        // Unity 6.5+ ships a serialization analyzer that flags these two as UAC1009 ("unsupported type").
        // Older versions skip them silently. The warning is the lesson here: remove the #pragma to show it live.
#pragma warning disable UAC1009
        // NOT serialized
        [SerializeField] private Dictionary<string, int> _dictionary = new ();

        // NOT serialized (nested containers)
        [SerializeField] private List<List<int>> _nestedLists = new ();
#pragma warning restore UAC1009

        // An interface-typed field is NOT serialized with [SerializeField]...
        [SerializeField] private IAbilityEffect ignoredEffect = null;

        // ...but [SerializeReference] stores managed references.
        [SerializeReference] private List<IAbilityEffect> effects = new ();

        // Reset() runs (Editor only) when the component is added or "Reset" is chosen in its menu.
        private void Reset()
        {
            effects = new List<IAbilityEffect> { new DamageEffect(), new HealEffect() };
        }

        public void ApplyAll(GameObject target)
        {
            foreach (var effect in effects)
                effect?.Apply(target);
        }

        // Keep the compiler quiet about fields that only exist for the demo.
        private void OnValidate()
        {
            _ = privateField + _plainPrivate + PublicButSkipped + dropArray.Length + drops.Count + _dictionary.Count + _nestedLists.Count + (ignoredEffect == null ? 0 : 1);
        }
    }
}