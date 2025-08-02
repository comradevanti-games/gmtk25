using System;
using UnityEngine;

namespace GMTK25 {

    public class Jukebox : MonoBehaviour {

        [SerializeField] private AudioSource jukeboxSource = null!;
        [SerializeField] private AudioLowPassFilter lpf = null!;

        private void Awake() {

            if (PlayerPrefs.HasKey("Mute")) {

                if (PlayerPrefs.GetInt("Mute") == 0) {
                    jukeboxSource.Play();
                }

            }

        }

        public void Play(AudioClip clip) {
            jukeboxSource.PlayOneShot(clip);
        }

        public void OnHealthChanged(float health) {
            lpf.cutoffFrequency = health <= 1 ? 500f : 5000f;
        }

        public void OnGameWon() {
            jukeboxSource.pitch = 0.5f;
        }

        public void OnGameResume() {
            jukeboxSource.pitch = 1;
        }

        public void OnMuteChanged() {

            if (jukeboxSource.isPlaying) {
                jukeboxSource.Stop();
            }
            else {
                jukeboxSource.Play();
            }

        }

    }

}