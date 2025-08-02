using System;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class NormalBullet : BulletBase
    {
        private BaseDamage baseDamage = null!;

        private void Awake()
        {
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
            baseDamage = GetComponent<BaseDamage>();
        }

        public override ColorType ColorType { get; } = null!;

        public override BulletType CurrentBulletType { get; set; } = null!;

        public override ColorType? LastHitColor { get; set; }

        public override BulletType? LastHitBulletType { get; set; }

        public override event Action<BulletType, ColorType?>? SuccessHit;

        public override event Action? FailHit;

        public override void OnDespawnTimeReached()
        {
            Singletons.Require<BulletPickupHandler>()
                .OnBulletFailed(CurrentBulletType, ColorType);
            Despawn();
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.layer)
            {
                case 8:
                {
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
                }

                case 9:
                    other.GetComponent<HealthKeeper>()
                        .TakeDamage(baseDamage.Value);
                    SuccessHit?.Invoke(CurrentBulletType,
                        gameObject.TryGetColorType());
                    Despawn();

                    break;
            }
        }

        public override void Despawn()
        {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }
    }
}