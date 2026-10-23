using UnityEngine;
using UnityEngine.InputSystem;

namespace Course.Fundamentals.InputSamples
{
    [AddComponentMenu("Course/Input/Action Based Mover")]
    public sealed class ActionBasedMover : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float speed = 5f;
        [SerializeField, Min(0f)] private float hopHeight = 0.5f;

        // Field initializers run when the component is created; the Inspector then owns the bindings.
        [SerializeField] private InputAction move = CreateMoveAction();
        [SerializeField] private InputAction jump = new ("Jump", InputActionType.Button, "<Keyboard>/space");

        private float _hopUntil;
        private float _groundY;

        private void Awake() => _groundY = transform.position.y;

        private static InputAction CreateMoveAction()
        {
            var action = new InputAction("Move", InputActionType.Value, expectedControlType: "Vector2");

            action.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");

            action.AddBinding("<Gamepad>/leftStick");

            return action;
        }

        // Actions do nothing until enabled. Pair Enable/Disable and +=/-= in OnEnable/OnDisable.
        private void OnEnable()
        {
            move.Enable();
            jump.Enable();
            jump.performed += OnJump;
        }

        private void OnDisable()
        {
            jump.performed -= OnJump;
            jump.Disable();
            move.Disable();
        }

        private void Update()
        {
            // Polling a Value action each frame is fine and allocation-free.
            var input = move.ReadValue<Vector2>();
            var delta = new Vector3(input.x, 0f, input.y) * (speed * Time.deltaTime);
            transform.Translate(delta, Space.World);

            // Tiny visual "hop" so the jump is visible without physics.
            var p = transform.position;
            p.y = _groundY + (Time.time < _hopUntil ? hopHeight : 0f);
            transform.position = p;
        }

        // Event-style handling for discrete Button actions.
        private void OnJump(InputAction.CallbackContext context)
        {
            _hopUntil = Time.time + 0.25f;
        }
    }
}