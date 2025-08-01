using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMTK25 {

    public class SceneHandler : MonoBehaviour {

        public void OnPlayerDeath() {
            SceneManager.LoadScene(0);
        }

        public void OnEsc() {
            SceneManager.LoadScene(0);
        }

    }

}