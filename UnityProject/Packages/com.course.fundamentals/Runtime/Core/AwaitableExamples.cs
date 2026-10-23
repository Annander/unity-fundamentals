using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Course.Fundamentals.Core
{
    [AddComponentMenu("Course/Awaitable Examples")]
    public sealed class AwaitableExamples : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float delaySeconds = 1f;
        [SerializeField] private bool useCoroutine;

        // Cache yield instructions: 'new WaitForSeconds' in a loop allocates every iteration.
        private WaitForSeconds _wait;

        private void Start()
        {
            if (useCoroutine)
            {
                _wait = new WaitForSeconds(delaySeconds);
                StartCoroutine(DelayedRoutine());
            }
            else
            {
                RunAwaitable();
            }
        }

        // Coroutine version
        //  Stops automatically when this GameObject is deactivated or destroyed.
        //  Can't return a value; exceptions are logged but don't propagate to a caller.
        private IEnumerator DelayedRoutine()
        {
            yield return _wait;
            Debug.Log("Coroutine: delay finished", this);
            yield return null; // next frame
            Debug.Log("Coroutine: one frame later", this);
        }

        // Awaitable version
        //  'async void' is acceptable ONLY for top-level fire-and-forget entry points like this one.
        private async void RunAwaitable()
        {
            try
            {
                var token = destroyCancellationToken;

                await Awaitable.WaitForSecondsAsync(delaySeconds, token);
                Debug.Log("Awaitable: delay finished", this);

                await Awaitable.NextFrameAsync(token);
                Debug.Log("Awaitable: one frame later", this);

                int result = await SumOnBackgroundThreadAsync(10_000_000, token);
                // Back on the main thread here (SumOnBackgroundThreadAsync switched back), so Unity APIs are safe.
                Debug.Log($"Awaitable: background sum = {result}", this);
            }
            catch (OperationCanceledException)
            {
                // Expected when the object is destroyed mid-wait. Not an error.
            }
        }

        private static async Awaitable<int> SumOnBackgroundThreadAsync(int count, CancellationToken token)
        {
            await Awaitable.BackgroundThreadAsync();
            // On a thread-pool thread now: do NOT touch GameObjects, Transforms, or most Unity APIs here.
            int sum = 0;
            for (int i = 0; i < count; i++)
            {
                if ((i & 0xFFFF) == 0) token.ThrowIfCancellationRequested();
                sum = unchecked(sum + (i & 7));
            }
            await Awaitable.MainThreadAsync();
            return sum;
        }

        // Awaitable gotcha: instances are pooled. Await each one exactly once; never store and re-await.
    }
}