using System;
using UnityEngine;

namespace GMTK25
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body = null!;
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float xClamp = 0f;
        [SerializeField] private float yClamp = 0f;

        public event Action? Moved;

        public Vector2 MovementDirection { get; set; }

        public Vector2 Velocity => MovementDirection * movementSpeed;

        private void Awake()
        {
            FindFirstObjectByType<InputHandler>(FindObjectsInactive.Exclude)
                .GetComponent<InputHandler>()
                .MovementInputHandled += OnMovementInput;
        }

        private void FixedUpdate()
        {
            body.linearVelocity = MovementDirection * movementSpeed;

            if (MovementDirection != Vector2.zero)
                Moved?.Invoke();
        }

        private void OnMovementInput(Vector2 dir)
        {
            MovementDirection = dir;
        }
    }
}