using System;
using UnityEngine;

namespace GMTK25 {

    public class BasicBullet : MonoBehaviour, IBullet {

        [SerializeField] private float damage = 0f;

        private void Awake() {
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
            Damage = damage;
        }

        public float Damage { get; set; }

        public ColorType ColorType { get; set; } = null!;

        public BulletType CurrentBulletType { get; set; } = null!;

        public ColorType? LastHitColor { get; set; }

        public BulletType? LastHitBulletType { get; set; }

        public event Action<BulletType, ColorType?>? SuccessHit;

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
                other.GetComponent<HealthKeeper>().TakeDamage(damage);
                SuccessHit?.Invoke(CurrentBulletType, gameObject.TryGetColorType());
                Despawn();
            }

        }

        public void Despawn() {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }

    }

}