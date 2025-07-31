using System;
using UnityEngine;

namespace GMTK25 {

    public class BulletDespawner : MonoBehaviour {

        [SerializeField] private float despawnTime = 0f;
        private float timeAlive = 0f;

        public event Action<BulletType>? HasDespawned;

        private void Update() {

            if (timeAlive <= despawnTime) {
                timeAlive += Time.deltaTime;
            }

            KillBullet();

        }

        private void KillBullet() {
            //HasDespawned?.Invoke();
            Destroy(gameObject);
        }

    }

}