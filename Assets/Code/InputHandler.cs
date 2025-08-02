using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMTK25 {

    public class InputHandler : MonoBehaviour {

        private Camera? mainCamera;

        public event Action<Vector2>? MovementInputHandled;

        public event Action<Vector2>? RotationInputHandled;

        public event Action? ShootInputHandled;

        public event Action<bool>? QuitInputHandled;

        public Vector2 MouseScreenPosition { get; set; }

        private void Awake() {
            mainCamera = Camera.main;
        }

        public void OnMovementInputReceived(InputAction.CallbackContext ctx) {
            Vector2 value = ctx.ReadValue<Vector2>();
            MovementInputHandled?.Invoke(value);
        }

        public void OnMousePositionInputReceived(InputAction.CallbackContext ctx) {

            if (ctx.performed) {
                Vector2 value = ctx.ReadValue<Vector2>();
                MouseScreenPosition = mainCamera!.ScreenToWorldPoint(value);
                RotationInputHandled?.Invoke(MouseScreenPosition);
            }
        }

        public void OnShootInputReceived(InputAction.CallbackContext ctx) {

            if (ctx.started) {
                ShootInputHandled?.Invoke();
            }

        }

        public void OnQuitInputReceived(InputAction.CallbackContext ctx) {

            if (ctx.started) {
                QuitInputHandled?.Invoke(true);
            }

            if (ctx.canceled) {
                QuitInputHandled?.Invoke(false);
            }

        }

    }

}