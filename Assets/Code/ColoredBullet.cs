using System;
using UnityEngine;

namespace GMTK25 {

    public class ColoredBullet : MonoBehaviour, IBullet {

        [SerializeField] private float damage = 0f;
        [SerializeField] private ColorType colorType = null!;

        private void Awake() {
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
            gameObject.SetColorType(colorType);
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

                if (other.gameObject.GetColorType() == gameObject.GetColorType()) {
                    other.GetComponent<HealthKeeper>().TakeDamage(damage);
                    SuccessHit?.Invoke(CurrentBulletType);
                }
                else {
                    Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType);
                }

                Despawn();

            }

        }

        public void Despawn() {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }

    }

}