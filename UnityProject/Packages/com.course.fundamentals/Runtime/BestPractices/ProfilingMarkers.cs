using Unity.Profiling;
using UnityEngine;

namespace Course.Fundamentals.BestPractices
{
    [AddComponentMenu("Course/Best Practices/Profiling Markers")]
    public sealed class ProfilingMarkers : MonoBehaviour
    {
        // GOOD: one static marker per region, created once.
        private static readonly ProfilerMarker SThinkMarker = new ProfilerMarker("Course.AI.Think");
        private static readonly ProfilerMarker SPathMarker = new ProfilerMarker("Course.AI.Path");

        private void Update()
        {
            using (SThinkMarker.Auto())
            {
                Think();
                using (SPathMarker.Auto())
                {
                    Path();
                }
            }
        }

       private float _sink;
        private void Think() { for (var i = 0; i < 1000; i++) _sink += Mathf.Sin(i); }
        private void Path() { for (var i = 0; i < 500; i++) _sink += Mathf.Cos(i); }
    }
}