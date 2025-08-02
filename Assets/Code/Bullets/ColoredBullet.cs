using System;
using System.Linq;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class ColoredBullet : BulletBase
    {
        [SerializeField] private ColorType colorType = null!;

        private IDamageMultiplier[] damageMultipliers =
            Array.Empty<IDamageMultiplier>();

        public override ColorType ColorType => colorType;

        public override BulletType CurrentBulletType { get; set; } = null!;

        public override ColorType? LastHitColor { get; set; }

        public override BulletType? LastHitBulletType { get; set; }

        public override event Action<BulletType, ColorType?>? SuccessHit;

        public override event Action? FailHit;

        private float DamageFor(BulletHit hit)
        {
            var mult = damageMultipliers.Aggregate(1f,
                (acc, mult) => acc * mult.CalcMultiplier(hit));
            return baseDamage.Value * mult;
        }

        protected override void Awake()
        {
            base.Awake();

            baseDamage = GetComponent<BaseDamage>();
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
            gameObject.SetColorType(colorType);
            damageMultipliers = GetComponents<IDamageMultiplier>();
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

            if (other.gameObject.layer == 8)
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
                        .OnBulletFailed(CurrentBulletType, ColorType);
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