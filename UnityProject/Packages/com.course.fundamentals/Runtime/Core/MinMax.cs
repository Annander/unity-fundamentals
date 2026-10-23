using System;

namespace Course.Fundamentals.Core
{
    [Serializable]
    public struct MinMax
    {
        public float min;
        public float max;

        public float Random() => UnityEngine.Random.Range(min, max);
    }
}