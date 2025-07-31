using System;
using UnityEngine;

namespace GMTK25 {

    public class TimedDespawner : MonoBehaviour {

        [SerializeField] private float despawnTime = 0f;
        private float timeAlive = 0f;
        private float initialTime = 0f;
        private bool shouldElapse = true;

        public event Action? Elapsed;

        private void Awake() {
            initialTime = Time.time;
        }

        private void Update() {

            if (!shouldElapse) return;

            if (initialTime + timeAlive >= initialTime + despawnTime) {
                Elapsed?.Invoke();
                shouldElapse = false;
            }

            timeAlive += Time.deltaTime;

        }

    }

}