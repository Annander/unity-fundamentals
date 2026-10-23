using System;

namespace CSharpBasics.Exercises
{
    // SOLUTION 04
    public sealed class Countdown
    {
        // 'event' means outsiders can only += / -=. They can't invoke it or wipe other
        // subscribers with '='. That's the answer to TODO question 3.
        public event Action<float> Elapsed;

        public float Duration { get; }
        private float _accumulated;

        public Countdown(float duration) { Duration = duration; }

        public void Tick(float dt)
        {
            _accumulated += dt;

            // 'while' handles a huge dt (e.g. after a hitch) that spans several cycles.
            while (_accumulated >= Duration)
            {
                _accumulated -= Duration;      // keep the remainder: no drift over time
                Elapsed?.Invoke(_accumulated); // ?. : no subscribers -> no call, no exception
            }
        }
    }
}