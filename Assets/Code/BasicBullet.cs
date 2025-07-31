using System;
using UnityEngine;

namespace GMTK25 {

    public class BasicBullet : MonoBehaviour, IBullet {

        private void Awake() {
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
        }

        public BulletType CurrentBulletType { get; set; } = null!;

        public event Action<BulletType>? SuccessHit;

        public void OnDespawnTimeReached() {
            Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType);
        }

        public void OnCollisionEnter2D(Collision2D other) {

            if (other.gameObject.layer == 8) {
                Debug.Log("Hit the wall! 🧱");
                Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType);
                Despawn();
            }

            if (other.gameObject.layer == 9) {
                Debug.Log("Hit the enemy! 👽");
                SuccessHit?.Invoke(CurrentBulletType);
                Despawn();
            }

        }

        public void Despawn() {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }

    }

}