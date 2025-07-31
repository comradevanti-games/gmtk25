using UnityEngine;

namespace GMTK25 {

    public class PlayerRotation : MonoBehaviour {

        [SerializeField] private Rigidbody2D body = null!;
        [SerializeField] private PlayerMovement playerMovement = null!;

        private Camera? mainCam;
        private InputHandler? inputHandler;

        private Vector2 LastKnownMousePosition { get; set; }

        private void Awake() {

            if (Camera.main != null) {
                mainCam = Camera.main;
            }

            inputHandler = FindFirstObjectByType<InputHandler>(FindObjectsInactive.Exclude).GetComponent<InputHandler>();
            inputHandler.RotationInputHandled += OnRotationInput;
            playerMovement.Moved += OnPlayerMoved;

        }

        private void SetNewRotation() {

            Vector2 mouseWorldPosition = mainCam!.ScreenToWorldPoint(LastKnownMousePosition);
            Vector2 directionToMouse = mouseWorldPosition - body.position;
            float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
            body.SetRotation(angle - 90f);

        }

        private void OnRotationInput(Vector2 mousePos) {
            LastKnownMousePosition = mousePos;
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