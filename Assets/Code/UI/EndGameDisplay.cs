using System;
using UnityEngine;

namespace GMTK25 {

    public class EndGameDisplay : MonoBehaviour {

        [SerializeField] private GameObject dialogWindow = null!;

        public void OnGameCompleted() {
            dialogWindow.SetActive(true);
        }

        public void OnGameStarted() {
            dialogWindow.SetActive(false);
        }

    }

}