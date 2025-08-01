using UnityEngine;

namespace GMTK25 {

    public class Jukebox : MonoBehaviour {

        [SerializeField] private AudioSource jukeboxSource = null!;

        public void Play(AudioClip clip) {
            jukeboxSource.PlayOneShot(clip);
        }

    }

}