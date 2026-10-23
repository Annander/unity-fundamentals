using Unity.AI.Navigation;
using UnityEngine;

namespace Course.Fundamentals.Navigation
{
    [AddComponentMenu("Course/Navigation/Runtime NavMesh Baker")]
    [RequireComponent(typeof(NavMeshSurface))]
    [DefaultExecutionOrder(-1000)]
    public sealed class RuntimeNavMeshBaker : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }
}