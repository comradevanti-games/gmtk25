using System;
using GMTK25.Enemies;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class ColoredLinkBullet : BulletBase
    {
        [SerializeField] private ColorType colorType = null!;
        [SerializeField] private BulletType linkBulletType = null!;

        private JumpCount jumpCount = null!;

        public override ColorType ColorType => colorType;

        public override BulletType CurrentBulletType { get; set; } = null!;

        public override ColorType? LastHitColor { get; set; }

        public override BulletType? LastHitBulletType { get; set; }

        public override event Action<BulletType, ColorType?>? SuccessHit;

        public override event Action? FailHit;

        protected override void Awake()
        {
            base.Awake();

            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
            gameObject.SetColorType(colorType);
            jumpCount = GetComponent<JumpCount>();
        }

        public override void OnDespawnTimeReached()
        {
            Singletons.Require<BulletPickupHandler>()
                .OnBulletFailed(CurrentBulletType, ColorType);
            Despawn();
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            var hit = new BulletHit(other.gameObject);

            if (hit.Health is { } health)
            {
                var damage = DamageFor(hit);
                health.TakeDamage(damage);
            }

            switch (other.gameObject.layer)
            {
                case 8:

                    if (other.gameObject.CompareTag("ShopItem"))
                    {
                        Despawn();

                        return;
                    }

                    Singletons.Require<BulletPickupHandler>()
                        .OnBulletFailed(CurrentBulletType, ColorType);
                    FailHit?.Invoke();
                    Despawn();

                    break;
                case 9:
                {
                    if (other.gameObject.GetColorType() == colorType)
                    {
                        if (LastHitColor == other.gameObject.GetColorType())
                        {
                            var enemyPos = Singletons.Require<EnemyTracker>()
                                .GetClosestEnemyPosition(
                                    other.transform.position, 2);

                            if (enemyPos.x < 25) ShootLinkBullet(enemyPos);
                        }

                        SuccessHit?.Invoke(CurrentBulletType,
                            gameObject.GetColorType());
                    }
                    else
                    {
                        Singletons.Require<BulletPickupHandler>()
                            .OnBulletFailed(CurrentBulletType, ColorType);
                        FailHit?.Invoke();
                    }

                    Despawn();

                    break;
                }
            }
        }

        private void ShootLinkBullet(Vector2 enemyPos)
        {
            var shootDirection = enemyPos - (Vector2)transform.position;
            var bulletRotation =
                Quaternion.Euler(0, 0,
                    Mathf.Atan2(shootDirection.y, shootDirection.x) *
                    Mathf.Rad2Deg - 90);
            var bulletGameObject = Instantiate(linkBulletType.Prefab,
                transform.position, bulletRotation);

            var bullet = bulletGameObject.GetComponent<ColoredLinkBullet>();
            bullet.CurrentBulletType = linkBulletType;
            bullet.jumpCount.Value = jumpCount.Value + 1;

            var bulletBody = bulletGameObject.GetComponent<Rigidbody2D>();
            bulletBody.AddForce(linkBulletType.InitialSpeed *
                                shootDirection.normalized);
        }

        public override void Despawn()
        {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }
    }
}