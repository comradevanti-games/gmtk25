using System;
using UnityEngine;

namespace GMTK25 {

    public class PlayerMovement : MonoBehaviour {

        [SerializeField] private Rigidbody2D body = null!;
        [SerializeField] private float movementSpeed = 10f;

        private Vector2 MovementDirection { get; set; }

        private void Awake() {
            FindFirstObjectByType<InputHandler>(FindObjectsInactive.Exclude).GetComponent<InputHandler>()
                .MovementInputHandled += OnMovementInput;
        }

        private void FixedUpdate() {
            if (MovementDirection == Vector2.zero) {
                return;
            }

            body.MovePosition(body.position + MovementDirection * (Time.fixedDeltaTime * movementSpeed));
        }

        private void OnMovementInput(Vector2 dir) {
            MovementDirection = dir;
        }

    }

}