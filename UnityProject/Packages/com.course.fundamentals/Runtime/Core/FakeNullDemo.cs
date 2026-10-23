using System.Collections;
using UnityEngine;

namespace Course.Fundamentals.Core
{
    [AddComponentMenu("Course/Fake Null Demo")]
    public sealed class FakeNullDemo : MonoBehaviour
    {
        private IEnumerator Start()
        {
            var temp = new GameObject("Temporary");
            var body = temp.AddComponent<Rigidbody>();
            body.isKinematic = true;

            Destroy(temp);
            yield return null;

            Debug.Log($"body == null -> {body == null}   (Unity's overload: native object is gone)");
            Debug.Log($"(bool)body -> {(bool)body}  (same check, implicit bool)");
            Debug.Log($"body is null -> {body is null}  (C# reference check: wrapper still exists!)");
            Debug.Log($"ReferenceEquals(body,null) -> {ReferenceEquals(body, null)}");

            // BAD: ?. doesn't see the destruction, so the call goes through and throws.
            try
            {
                body?.AddForce(Vector3.up);
            }
            catch (MissingReferenceException e)
            {
                Debug.Log($"body?.AddForce threw {e.GetType().Name}: ?. did not protect us.");
            }

            // BAD: ?? also checks only the C# reference, so 'chosen' is the destroyed body.
            var fallback = GetComponent<Rigidbody>();
            var chosen = body ?? fallback;
            Debug.Log($"body ?? fallback picked the destroyed body: {ReferenceEquals(chosen, body)}");

            // GOOD: explicit Unity null checks.
            if (body != null) body.AddForce(Vector3.up);
            var safe = body != null ? body : fallback;
            Debug.Log($"Explicit '!= null' chose the fallback (which may itself be null here): {safe == fallback}");
        }
    }
}