using System;
using UnityEngine;

namespace GMTK25 {

    public class NormalBullet : MonoBehaviour, IBullet {

        [SerializeField] private float damage = 0f;

        private void Awake() {
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
            Damage = damage;
        }

        public float Damage { get; set; }

        public ColorType ColorType { get; } = null!;

        public BulletType CurrentBulletType { get; set; } = null!;

        public ColorType? LastHitColor { get; set; }

        public BulletType? LastHitBulletType { get; set; }

        public event Action<BulletType, ColorType?>? SuccessHit;

        public event Action? FailHit;

        public void OnDespawnTimeReached() {
            Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType, ColorType);
            Despawn();
        }

        public void OnTriggerEnter2D(Collider2D other) {

            switch (other.gameObject.layer) {
                case 8:
                    Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType, ColorType);
                    FailHit?.Invoke();
                    Despawn();

                    break;
                case 9:
                    other.GetComponent<HealthKeeper>().TakeDamage(Damage);
                    SuccessHit?.Invoke(CurrentBulletType, gameObject.TryGetColorType());
                    Despawn();

                    break;
            }

        }

        public void Despawn() {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }

    }

}