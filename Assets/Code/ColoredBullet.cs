using System;
using UnityEngine;

namespace GMTK25 {

    public class ColoredBullet : MonoBehaviour, IBullet {

        [SerializeField] private float damage = 0f;
        [SerializeField] private ColorType colorType = null!;

        public float Damage { get; set; }

        public ColorType ColorType => colorType;

        public BulletType CurrentBulletType { get; set; } = null!;

        public ColorType? LastHitColor { get; set; }

        public BulletType? LastHitBulletType { get; set; }

        public event Action<BulletType, ColorType?>? SuccessHit;

        public event Action? FailHit;

        private void Awake() {
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
            gameObject.SetColorType(colorType);
            Damage = damage;
        }

        public void OnDespawnTimeReached() {
            Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType, ColorType);
            Despawn();
        }

        public void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.layer == 8) {
                Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType, ColorType);
                FailHit?.Invoke();
                Despawn();
            }

            if (other.gameObject.layer == 9) {

                if (other.gameObject.GetColorType() == gameObject.GetColorType()) {
                    other.GetComponent<HealthKeeper>().TakeDamage(damage);
                    SuccessHit?.Invoke(CurrentBulletType, gameObject.GetColorType());
                }
                else {
                    Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType, ColorType);
                    FailHit?.Invoke();
                    other.GetComponent<HealthKeeper>().TakeDamage(0.5f);
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