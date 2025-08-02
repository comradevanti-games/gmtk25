using System;
using System.Collections;
using GMTK25.Enemies;
using TMPro;
using UnityEngine;

namespace GMTK25 {

    public class WaveTimeDisplay : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI timeDisplayText = null!;

        private TimeSpan timeRemaining;
        private bool timerIsRunning = false;

        public void OnSubWaveStarted(SubWave newSubWave) {

            if (timerIsRunning) {
                StopAllCoroutines();
            }

            timeRemaining = newSubWave.Duration;
            StartCoroutine(UpdateTimerCoroutine());
        }

        public void OnBreakStarted() {
            
            if (timerIsRunning) {
                StopAllCoroutines();
            }

            timeRemaining = TimeSpan.FromSeconds(20);
            StartCoroutine(UpdateTimerCoroutine());
            
        }

        private IEnumerator UpdateTimerCoroutine() {
            timerIsRunning = true;

            while (timeRemaining > TimeSpan.Zero) {
                timeRemaining -= TimeSpan.FromSeconds(Time.deltaTime);
                UpdateTimeDisplay();

                yield return null;
            }

            timerIsRunning = false;
            timeDisplayText.text = "00:00";
        }

        private void UpdateTimeDisplay() {
            timeDisplayText.text = $"{timeRemaining.Minutes:D2}:{timeRemaining.Seconds:D2}";
        }

    }

}