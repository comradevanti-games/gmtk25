using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK25.UI {

    public class PlayerHealthDisplay : MonoBehaviour {

        [SerializeField] private Image heartImage = null!;
        [SerializeField] private Image heartImageBG = null!;
        [SerializeField] private float pulsateSpeed = 2f;
        [SerializeField] private float pulsateGrowth = 1.2f;

        private Vector3 originalScale;
        private Coroutine? pulsateCoroutine;

        private void Start() {
            originalScale = heartImage.transform.localScale;
        }

        public void OnHealthChanged(float newHealth) {
            heartImage.fillAmount = Convert.ToSingle(1) / 3 * (int)newHealth;

            if (Mathf.Approximately(newHealth, 1f)) {
                pulsateCoroutine ??= StartCoroutine(Pulsate());
            }
            else {

                if (pulsateCoroutine == null) {
                    return;
                }

                StopCoroutine(pulsateCoroutine);
                pulsateCoroutine = null;

                heartImage.transform.localScale = originalScale;
                heartImageBG.transform.localScale = originalScale;
            }
        }

        private IEnumerator Pulsate() {
            while (true) {
                yield return ScaleOverTime(originalScale, originalScale * pulsateGrowth, pulsateSpeed);
                yield return ScaleOverTime(originalScale * pulsateGrowth, originalScale, pulsateSpeed);
            }
        }

        private IEnumerator ScaleOverTime(Vector3 startScale, Vector3 endScale, float speed) {
            float startTime = Time.time;
            const float distance = 1f;

            while (Time.time < startTime + distance / speed) {
                float fractionOfJourney = (Time.time - startTime) * speed / distance;
                float t = Mathf.Sin(fractionOfJourney * Mathf.PI * 0.5f);
                heartImage.transform.localScale = Vector3.Lerp(startScale, endScale, t);
                heartImageBG.transform.localScale = Vector3.Lerp(startScale, endScale, t);

                yield return null;
            }

            heartImage.transform.localScale = endScale;
            heartImageBG.transform.localScale = endScale;
        }

    }

}