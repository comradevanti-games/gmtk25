using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMTK25 {

    public class MainMenu : MonoBehaviour {

        public void OnPlayButtonClicked() {
            SceneManager.LoadScene(1);
        }

        public void OnQuitButtonClicked() {
            Application.Quit();
        }

    }

}