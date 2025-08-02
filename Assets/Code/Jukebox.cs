using System;
using UnityEngine;

namespace GMTK25 {

    public class Jukebox : MonoBehaviour {

        [SerializeField] private AudioSource jukeboxSource = null!;

        private void Awake() {

            if (PlayerPrefs.HasKey("Mute")) {

                if (PlayerPrefs.GetInt("Mute") == 1) {
                    jukeboxSource.Play();
                }
                
            }
            
        }

        public void Play(AudioClip clip) {
            jukeboxSource.PlayOneShot(clip);
        }

    }

}