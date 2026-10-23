using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Course.Fundamentals.BestPractices
{
    // Struct WITHOUT IEquatable<T>: Dictionary/HashSet/List.Contains fall back to object.Equals
    // and GetHashCode via the default comparer -> boxing on every comparison.
    public struct GridPosBad
    {
        public int X, Y;
        public GridPosBad(int x, int y) { X = x; Y = y; }
    }

    // Struct WITH IEquatable<T>: EqualityComparer<T>.Default calls Equals(T) directly. No boxing.
    public readonly struct GridPos : IEquatable<GridPos>
    {
        private readonly int _x, _y;

        public GridPos(int x, int y) { _x = x; _y = y; }
        public bool Equals(GridPos other) => _x == other._x && _y == other._y;
        public override bool Equals(object obj) => obj is GridPos other && Equals(other);
        public override int GetHashCode() => (_x * 397) ^ _y;
    }

    /// Per-frame allocations and their fixes. Toggle useBadVersion in Play Mode and watch the GC Alloc column for "AllocationExamples.Update" in the Profiler.
    [AddComponentMenu("Course/Best Practices/Allocation Examples")]
    public sealed class AllocationExamples : MonoBehaviour
    {
        private sealed class Target
        {
            public int Team;
            public bool IsAlive = true;
        }

        [SerializeField] private bool useBadVersion;
        [SerializeField] private int team = 1;

        private readonly List<Target> _targets = new ();
        private IEnumerable<Target> _targetsAsInterface;

        private readonly HashSet<GridPosBad> _visitedBad = new ();
        private readonly HashSet<GridPos> _visited = new ();

        private int _score;
        private int _lastShownScore = -1;
        private string _scoreLabel = string.Empty;
        private readonly StringBuilder _builder = new (32);

        public string ScoreLabel => _scoreLabel; // imagine this feeds a UI text element

        private void Awake()
        {
            for (var i = 0; i < 64; i++)
                _targets.Add(new Target { Team = i % 3 });

            _targetsAsInterface = _targets;

            for (var i = 0; i < 16; i++)
            {
                _visitedBad.Add(new GridPosBad(i, i));
                _visited.Add(new GridPos(i, i));
            }
        }

        private void Update()
        {
            _score += 1;
            if (useBadVersion) UpdateBad();
            else UpdateGood();
        }

        private void UpdateBad()
        {
            // 1) String building every frame: new string each frame.
            _scoreLabel = "Score: " + _score;

            // 2) LINQ + a lambda capturing a field via 'this' -> iterator + delegate allocations.
            int alive = _targets.Where(t => t.IsAlive && t.Team != this.team).Count();

            // 3) foreach over an interface-typed collection -> boxed enumerator.
            int total = 0;
            foreach (Target t in _targetsAsInterface) total += t.Team;

            // 4) params overload: Mathf.Max(float, float, float) binds to Max(params float[]) -> array alloc.
            float m = Mathf.Max(1f, 2f, 3f);

            // 5) Closure capturing a LOCAL -> a new closure object + delegate on every call.
            var team = this.team;
            var found = _targets.Find(t => t.Team == team);

            // 6) Struct without IEquatable in a HashSet -> boxing in Contains.
            var seen = _visitedBad.Contains(new GridPosBad(3, 3));

            Use(alive + total + m + (found != null ? 1 : 0) + (seen ? 1 : 0));
        }

        private void UpdateGood()
        {
            // 1) Rebuild the label only when the value changes.
            if (_score != _lastShownScore)
            {
                _lastShownScore = _score;
                _builder.Clear().Append("Score: ").Append(_score);
                _scoreLabel = _builder.ToString();
            }

            // 2) Plain loop.
            var alive = 0;
            for (var i = 0; i < _targets.Count; i++)
            {
                var t = _targets[i];
                if (t.IsAlive && t.Team != team) alive++;
            }

            // 3) foreach over the concrete List<T> uses its struct enumerator: no allocation.
            var total = 0;
            foreach (var t in _targets)
                total += t.Team;

            // 4) Two-argument overload, nested.
            var m = Mathf.Max(1f, Mathf.Max(2f, 3f));

            // 5) Loop instead of a capturing lambda.
            Target found = null;
            for (var i = 0; i < _targets.Count; i++)
            {
                if (_targets[i].Team == team) { found = _targets[i]; break; }
            }

            // 6) IEquatable<T> struct.
            var seen = _visited.Contains(new GridPos(3, 3));

            Use(alive + total + m + (found != null ? 1 : 0) + (seen ? 1 : 0));
        }

        private float _sink;

        private void Use(float value) => _sink = value;
    }
}