using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMTK25 {

    public class InputHandler : MonoBehaviour {

        public event Action<Vector2> MovementInputHandled;

        public void OnMovementInputReceived(InputAction.CallbackContext ctx) {

            Vector2 value = ctx.ReadValue<Vector2>();
            MovementInputHandled?.Invoke(value);

        }

    }

}