using System;
using UnityEngine;

namespace GMTK25 {

    public class EndGameDisplay : MonoBehaviour {

        [SerializeField] private GameObject dialogWindow = null!;

        private InputHandler? inputHandler;

        private void Awake() {
            inputHandler = Singletons.Require<InputHandler>();
        }

        public void OnGameCompleted() {
            dialogWindow.SetActive(true);
            inputHandler!.SwitchActionMap("UI");
        }

        public void OnGameStarted() {
            dialogWindow.SetActive(false);
            inputHandler!.SwitchActionMap("Player");
        }

    }

}