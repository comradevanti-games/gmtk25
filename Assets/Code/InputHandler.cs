using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMTK25 {

    public class InputHandler : MonoBehaviour {

        public event Action<Vector2>? MovementInputHandled;

        public event Action<Vector2>? RotationInputHandled;

        public event Action? ShootInputHandled;

        public void OnMovementInputReceived(InputAction.CallbackContext ctx) {

            Vector2 value = ctx.ReadValue<Vector2>();
            MovementInputHandled?.Invoke(value);

        }

        public void OnMousePositionInputReceived(InputAction.CallbackContext ctx) {
            Vector2 value = ctx.ReadValue<Vector2>();
            RotationInputHandled?.Invoke(value);
        }

        public void OnShootInputReceived(InputAction.CallbackContext ctx) {

            if (ctx.started) {
                ShootInputHandled?.Invoke();
            }

        }

    }

}