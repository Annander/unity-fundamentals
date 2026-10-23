using UnityEngine;

namespace Course.Fundamentals.Core
{
    [CreateAssetMenu(menuName = "Course/Events/Int Event Channel", fileName = "NewIntEvent")]
    public sealed class IntEventChannel : EventChannel<int> {}
}