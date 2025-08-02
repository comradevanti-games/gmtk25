using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK25 {

    public class QuitDisplay : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI quitText = null!;
        [SerializeField] private Image leftBar = null!;
        [SerializeField] private Image rightBar = null!;

        private bool isQuitting;
        private TimeSpan timeToQuit = TimeSpan.FromSeconds(2);
        private Coroutine? quitRoutine;

        private void Awake() {
            Singletons.Require<InputHandler>().QuitInputHandled += OnQuitInput;
        }

        private void OnQuitInput(bool quitting) {

            isQuitting = quitting;

            if (isQuitting) {
                quitRoutine = StartCoroutine(Quit());
            }
            else {
                StopCoroutine(quitRoutine);
                quitText.gameObject.SetActive(false);
                leftBar.fillAmount = 0;
                rightBar.fillAmount = 0;
                timeToQuit = TimeSpan.FromSeconds(2);
            }

        }

        private IEnumerator Quit() {

            quitText.gameObject.SetActive(true);

            while (isQuitting) {

                timeToQuit -= TimeSpan.FromSeconds(Time.deltaTime);
                float t = Mathf.InverseLerp(2000,0, (float)timeToQuit.TotalMilliseconds);
                leftBar.fillAmount = Mathf.Lerp(0, 1, t);
                rightBar.fillAmount = Mathf.Lerp(0, 1, t);

                if (timeToQuit.TotalMilliseconds <= 0) {
                    Singletons.Require<SceneHandler>().OnEsc();
                }

                yield return null;

            }

        }

    }

}