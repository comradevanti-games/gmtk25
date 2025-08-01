using System;
using GMTK25.Enemies;
using UnityEngine;

namespace GMTK25 {

    public class ColoredLinkBullet : MonoBehaviour, IBullet {

        [SerializeField] private float damage = 0f;
        [SerializeField] private ColorType colorType = null!;
        [SerializeField] private BulletType linkBulletType = null!;

        public float Damage { get; set; }

        public ColorType ColorType { get; set; } = null!;

        public BulletType CurrentBulletType { get; set; } = null!;

        public ColorType? LastHitColor { get; set; }

        public BulletType? LastHitBulletType { get; set; }

        public event Action<BulletType, ColorType?>? SuccessHit;

        private void Awake() {
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
            gameObject.SetColorType(colorType);
            Damage = damage;
        }

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

                    if (LastHitColor == other.gameObject.GetColorType()) {

                        Singletons.Require<EnemyTracker>().GetClosestEnemyPosition(other.transform.position);

                        GameObject bulletGameObject = Instantiate(linkBulletType.Prefab, transform.position,
                            transform.rotation);

                        IBullet bullet = bulletGameObject.GetComponent<IBullet>();
                        bullet.CurrentBulletType = linkBulletType;
                    }

                    SuccessHit?.Invoke(CurrentBulletType, gameObject.GetColorType());
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