using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GMTK25 {

    public class MainMenu : MonoBehaviour {

        [SerializeField] private Image muteIconRenderer = null!;
        [SerializeField] private Sprite muteIcon = null!;
        [SerializeField] private Sprite unmuteIcon = null!;

        private void Awake() {

            if (!PlayerPrefs.HasKey("Mute")) {
                PlayerPrefs.SetInt("Mute", 0);
            }
            else {
                muteIconRenderer.sprite = PlayerPrefs.GetInt("Mute") == 0 ? unmuteIcon : muteIcon;
            }
        }

        public void OnPlayButtonClicked() {
            SceneManager.LoadScene(1);
        }

        public void OnQuitButtonClicked() {
            Application.Quit();
        }

        public void OnMuteButtonClicked() {

            if (!PlayerPrefs.HasKey("Mute")) return;

            if (PlayerPrefs.GetInt("Mute") == 0) {
                PlayerPrefs.SetInt("Mute", 1);
                muteIconRenderer.sprite = muteIcon;
            }
            else {
                PlayerPrefs.SetInt("Mute", 0);
                muteIconRenderer.sprite = unmuteIcon;
            }

        }

    }

}