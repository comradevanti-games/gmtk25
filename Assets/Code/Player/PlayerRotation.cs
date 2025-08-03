using UnityEngine;

namespace GMTK25 {

    public class PlayerRotation : MonoBehaviour {

        [SerializeField] private Rigidbody2D body = null!;
        [SerializeField] private PlayerMovement playerMovement = null!;
        [SerializeField] private SpriteRenderer gamePadCrosshair = null!;
        [SerializeField] private float gamePadDeadZone = 0f;

        private InputHandler? inputHandler;

        private Vector2 RotationInput { get; set; }

        private string ControlScheme { get; set; } = "MKB";

        private void Awake() {

            inputHandler = FindFirstObjectByType<InputHandler>(FindObjectsInactive.Exclude).GetComponent<InputHandler>();
            inputHandler.RotationInputHandled += OnRotationInput;
            inputHandler.ControlSchemeSwitched += OnControlSchemeChanged;
            playerMovement.Moved += OnPlayerMoved;

        }

        private void SetNewRotation() {

            if (ControlScheme == "MKB") {
                Vector2 directionToMouse = RotationInput - body.position;
                float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
                body.SetRotation(angle - 90f);
            }
            else if (ControlScheme == "Gamepad") {
                if (RotationInput.sqrMagnitude > gamePadDeadZone) {
                    float angle = Mathf.Atan2(RotationInput.y, RotationInput.x) * Mathf.Rad2Deg;
                    body.SetRotation(angle - 90f);
                }
            }

        }

        private void OnControlSchemeChanged(string scheme) {
            ControlScheme = scheme;

            if (scheme == "Gamepad") {
                gamePadCrosshair.enabled = true;
            }

            if (scheme == "MKB") {
                gamePadCrosshair.enabled = false;
            }

        }

        private void OnRotationInput(Vector2 mousePos) {
            RotationInput = mousePos;
            SetNewRotation();
        }

        private void OnPlayerMoved() {
            SetNewRotation();
        }

        private void OnDisable() {
            if (inputHandler != null) {
                inputHandler.RotationInputHandled -= OnRotationInput;
            }
        }

    }

}