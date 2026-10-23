using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Course.Fundamentals.Navigation
{
    [AddComponentMenu("Course/Navigation/Click To Move")]
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class ClickToMove : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private LayerMask groundMask = ~0;
        [SerializeField, Min(1f)] private float maxRayDistance = 500f;
        [Tooltip("How far from the clicked point we search for a NavMesh position.")]
        [SerializeField, Min(0.1f)] private float maxSnapDistance = 1f;

        [SerializeField] private InputAction click = new ("Click", InputActionType.Button, "<Pointer>/press");
        [SerializeField] private InputAction point = new ("Point", InputActionType.Value, "<Pointer>/position", expectedControlType: "Vector2");

        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            if (camera == null) camera = Camera.main;
        }

        private void OnEnable()
        {
            point.Enable();
            click.Enable();
            click.performed += OnClick;
        }

        private void OnDisable()
        {
            click.performed -= OnClick;
            click.Disable();
            point.Disable();
        }

        private void Start()
        {
            if (!_agent.enabled) _agent.enabled = true;
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (camera == null || !_agent.isOnNavMesh) return;

            var screenPosition = point.ReadValue<Vector2>();
            var ray = camera.ScreenPointToRay(screenPosition);

            if (!Physics.Raycast(ray, out var hit, maxRayDistance, groundMask, QueryTriggerInteraction.Ignore))
                return;

            if (NavMesh.SamplePosition(hit.point, out var navHit, maxSnapDistance, NavMesh.AllAreas))
                _agent.SetDestination(navHit.position);
        }
    }
}