using System;
using UnityEngine;

namespace GMTK25 {

    public class TimedDespawner : MonoBehaviour {

        [SerializeField] private float despawnTime = 0f;
        private float timeAlive = 0f;

        public event Action? Elapsed;

        private void Update() {

            if (timeAlive <= despawnTime) {
                timeAlive += Time.deltaTime;

                return;
            }

            Despawn();

        }

        private void Despawn() {
            Elapsed?.Invoke();
            Destroy(gameObject);
        }

    }

}