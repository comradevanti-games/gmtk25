using System;
using GMTK25.Enemies;
using UnityEngine;

namespace GMTK25 {

    public class ColoredLinkBullet : MonoBehaviour, IBullet {

        [SerializeField] private float damage = 0f;
        [SerializeField] private ColorType colorType = null!;
        [SerializeField] private BulletType linkBulletType = null!;

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
            Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType);
            Despawn();
        }

        public void OnTriggerEnter2D(Collider2D other) {

            switch (other.gameObject.layer) {
                case 8:
                    Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType);
                    FailHit?.Invoke();
                    Despawn();

                    break;
                case 9:
                {
                    if (other.gameObject.GetColorType() == colorType) {

                        if (LastHitColor == other.gameObject.GetColorType()) {

                            Vector2 enemyPos = Singletons.Require<EnemyTracker>()
                                .GetClosestEnemyPosition(other.transform.position, 2);

                            if (enemyPos.x < 25) {
                                ShootLinkBullet(enemyPos);
                            }

                        }

                        other.GetComponent<HealthKeeper>().TakeDamage(damage);
                        SuccessHit?.Invoke(CurrentBulletType, gameObject.GetColorType());
                    }
                    else {
                        Singletons.Require<BulletPickupHandler>().OnBulletFailed(CurrentBulletType);
                        FailHit?.Invoke();
                        other.GetComponent<HealthKeeper>().TakeDamage(0.5f);
                    }

                    Despawn();

                    break;
                }
            }

        }

        private void ShootLinkBullet(Vector2 enemyPos) {

            Vector2 shootDirection = enemyPos - (Vector2)transform.position;
            Quaternion bulletRotation =
                Quaternion.Euler(0, 0, Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg - 90);
            GameObject bulletGameObject = Instantiate(linkBulletType.Prefab, transform.position, bulletRotation);

            IBullet bullet = bulletGameObject.GetComponent<IBullet>();
            bullet.CurrentBulletType = linkBulletType;
            bullet.Damage = Damage / 2f;

            Rigidbody2D bulletBody = bulletGameObject.GetComponent<Rigidbody2D>();
            bulletBody.AddForce(linkBulletType.InitialSpeed * shootDirection.normalized);

        }

        public void Despawn() {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }

    }

}