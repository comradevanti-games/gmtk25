using System;
using UnityEngine;

namespace GMTK25 {

    public class PlayerMovement : MonoBehaviour {

        [SerializeField] private Rigidbody2D body = null!;
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float xClamp = 0f;
        [SerializeField] private float yClamp = 0f;

        public event Action? Moved;

        private Vector2 MovementDirection { get; set; }

        private void Awake() {
            FindFirstObjectByType<InputHandler>(FindObjectsInactive.Exclude).GetComponent<InputHandler>()
                .MovementInputHandled += OnMovementInput;
        }

        private void FixedUpdate() {
            if (MovementDirection == Vector2.zero) {
                return;
            }

            Vector2 nextPosition = body.position + MovementDirection * (Time.fixedDeltaTime * movementSpeed);
            Vector2 clampedPosition = new Vector2(Mathf.Clamp(nextPosition.x, -xClamp, xClamp),
                Mathf.Clamp(nextPosition.y, -yClamp, yClamp));
            body.MovePosition(clampedPosition);
            Moved?.Invoke();
        }

        private void OnMovementInput(Vector2 dir) {
            MovementDirection = dir;
        }

    }

}