using System.Collections;
using UnityEngine;

namespace GMTK25 {

    public class ScreenShake : MonoBehaviour {

        private Transform cameraTransform = null!;
        private Vector3 originalCameraPosition;
        private float currentShakeDuration;

        private void Awake() {
            cameraTransform = Camera.main!.transform;
            originalCameraPosition = cameraTransform.localPosition;
        }

        public void Shake(float duration, float maxX, float maxY, float shakeSpeed) {
            StartCoroutine(ShakeCoroutine(duration, maxX, maxY, shakeSpeed));
        }

        private IEnumerator ShakeCoroutine(float duration, float maxX, float maxY, float shakeSpeed) {
            currentShakeDuration = duration;

            while (currentShakeDuration > 0) {
                float offsetX = Random.Range(-1f, 1f) * maxX;
                float offsetY = Random.Range(-1f, 1f) * maxY;

                Vector3 targetPosition = originalCameraPosition + new Vector3(offsetX, offsetY, 0);
                cameraTransform.localPosition =
                    Vector3.Lerp(cameraTransform.localPosition, targetPosition, shakeSpeed * Time.deltaTime);

                currentShakeDuration -= Time.deltaTime;

                yield return null;
            }

            while (Vector3.Distance(cameraTransform.localPosition, originalCameraPosition) > 0.01f) {
                cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, originalCameraPosition,
                    shakeSpeed * Time.deltaTime);

                yield return null;
            }

            cameraTransform.localPosition = originalCameraPosition;
        }

    }

}