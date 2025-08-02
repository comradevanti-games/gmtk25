using System;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class ColoredBullet : BulletBase
    {
        [SerializeField] private ColorType colorType = null!;

        public override BulletType CurrentBulletType { get; set; } = null!;

        public override ColorType? LastHitColor { get; set; }

        public override BulletType? LastHitBulletType { get; set; }

        public override event Action<BulletType, ColorType?>? SuccessHit;

        public override event Action? FailHit;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetColorType(colorType);
        }

        public override void OnDespawnTimeReached()
        {
            Singletons.Require<BulletPickupHandler>()
                .OnBulletFailed(CurrentBulletType, colorType);
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

            if (other.gameObject.layer == 8)
            {
                if (other.gameObject.CompareTag("ShopItem"))
                {
                    Despawn();

                    return;
                }

                Singletons.Require<BulletPickupHandler>()
                    .OnBulletFailed(CurrentBulletType, colorType);
                FailHit?.Invoke();
                Despawn();
            }

            if (other.gameObject.layer == 9)
            {
                if (other.gameObject.GetColorType() ==
                    gameObject.GetColorType())
                {
                    SuccessHit?.Invoke(CurrentBulletType,
                        gameObject.GetColorType());
                }
                else
                {
                    Singletons.Require<BulletPickupHandler>()
                        .OnBulletFailed(CurrentBulletType, colorType);
                    FailHit?.Invoke();
                }

                Despawn();
            }
        }

        public override void Despawn()
        {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }
    }
}