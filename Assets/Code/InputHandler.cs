using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GMTK25
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput = null!;
        [SerializeField] private GameObject firstSelectedUIElement = null!;

        private Camera? mainCamera;

        public event Action<Vector2>? MovementInputHandled;

        public event Action<Vector2>? RotationInputHandled;

        public event Action? ShootInputHandled;

        public event Action? DashInput;

        public event Action<bool>? QuitInputHandled;

        public event Action<string>? ControlSchemeSwitched;

        public Vector2 MouseScreenPosition { get; private set; }

        public string? CurrentControlScheme { get; private set; }

        private void Awake()
        {
            mainCamera = Camera.main;
            CurrentControlScheme = playerInput.currentControlScheme;
        }

        public void OnDashInput(InputAction.CallbackContext ctx)
        {
            if (ctx.started) DashInput?.Invoke();
        }

        public void OnMovementInputReceived(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<Vector2>();
            MovementInputHandled?.Invoke(value);
        }

        public void OnLookInputReceived(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                var value = ctx.ReadValue<Vector2>();

                if (playerInput.currentControlScheme == "Gamepad")
                {
                    if (value != Vector2.zero)
                        RotationInputHandled?.Invoke(value);
                }
                else
                {
                    MouseScreenPosition = mainCamera!.ScreenToWorldPoint(value);
                    RotationInputHandled?.Invoke(MouseScreenPosition);
                }
            }
        }

        public void OnShootInputReceived(InputAction.CallbackContext ctx)
        {
            if (ctx.started) ShootInputHandled?.Invoke();
        }

        public void OnQuitInputReceived(InputAction.CallbackContext ctx)
        {
            if (ctx.started) QuitInputHandled?.Invoke(true);

            if (ctx.canceled) QuitInputHandled?.Invoke(false);
        }

        public void SwitchActionMap(string actionMapName)
        {
            playerInput.SwitchCurrentActionMap(actionMapName);
            EventSystem.current.SetSelectedGameObject(null!);
            EventSystem.current.SetSelectedGameObject(firstSelectedUIElement);
        }

        public void OnControlsSwitched(PlayerInput updatedInputs)
        {
            ControlSchemeSwitched?.Invoke(updatedInputs.currentControlScheme);
            CurrentControlScheme = updatedInputs.currentControlScheme;

            if (updatedInputs.currentControlScheme == "Gamepad")
                Cursor.visible = false;
            else
                Cursor.visible = true;
        }
    }
}