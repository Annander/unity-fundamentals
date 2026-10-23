using System;

namespace CSharpBasics.Exercises
{
    // EXERCISE 04 - events
    // A repeating countdown. In Unity you'd call Tick(Time.deltaTime) from Update.
    //
    // TODO:
    //   1. Implement Tick: accumulate dt; when the accumulated time reaches Duration, raise Elapsed
    //      with the overshoot (accumulated - Duration), and KEEP the overshoot for the next cycle.
    //   2. Raising with no subscribers must not throw.
    //   3. Question for the group: why can't code outside this class write `timer.Elapsed = null;`?
    //
    // (The compiler warns CS0067 "event is never used" until you raise it. That warning is your TODO.)
    public sealed class Countdown
    {
        public event Action<float> Elapsed;

        public float Duration { get; }
        private float _accumulated;

        public Countdown(float duration) { Duration = duration; }

        public void Tick(float dt)
        {
            throw new NotImplementedException();
        }
    }
}
