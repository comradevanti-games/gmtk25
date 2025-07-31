using System;
using UnityEngine;

namespace GMTK25 {

    public class BasicBullet : MonoBehaviour, IBullet {

        [SerializeField] private float damage = 0f;

        private void Awake() {
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
        }

        public BulletType CurrentBulletType { get; set; } = null!;

        public event Action<BulletType>? SuccessHit;

        public void OnDespawnTimeReached() {
            Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType);
            Despawn();
        }

        public void OnTriggerEnter2D(Collider2D other) {

            if (other.gameObject.layer == 8) {
                Debug.Log("Hit the wall! 🧱");
                Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType);
                Despawn();
            }

            if (other.gameObject.layer == 9) {
                Debug.Log("Hit the enemy! 👽");
                other.GetComponent<HealthKeeper>().TakeDamage(1);
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