using System;
using System.Linq;
using GMTK25.Enemies;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class ColoredLinkBullet : MonoBehaviour, IBullet
    {
        [SerializeField] private ColorType colorType = null!;
        [SerializeField] private BulletType linkBulletType = null!;

        private BaseDamage baseDamage = null!;
        private JumpCount jumpCount = null!;

        private IDamageMultiplier[] damageMultipliers =
            Array.Empty<IDamageMultiplier>();

        public ColorType ColorType => colorType;

        public BulletType CurrentBulletType { get; set; } = null!;

        public ColorType? LastHitColor { get; set; }

        public BulletType? LastHitBulletType { get; set; }

        public event Action<BulletType, ColorType?>? SuccessHit;

        public event Action? FailHit;

        private float DamageFor(BulletHit hit)
        {
            var mult = damageMultipliers.Aggregate(1f,
                (acc, mult) => acc * mult.CalcMultiplier(hit));
            return baseDamage.Value * mult;
        }

        private void Awake()
        {
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
            gameObject.SetColorType(colorType);
            baseDamage = GetComponent<BaseDamage>();
            jumpCount = GetComponent<JumpCount>();
            damageMultipliers = GetComponents<IDamageMultiplier>();
        }

        public void OnDespawnTimeReached()
        {
            Singletons.Require<BulletPickupHandler>()
                .OnBulletFailed(CurrentBulletType, ColorType);
            Despawn();
        }

        public void OnTriggerEnter2D(Collider2D other)
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

        public void Despawn()
        {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }
    }
}