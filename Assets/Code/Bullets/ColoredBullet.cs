using System;
using System.Linq;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class ColoredBullet : MonoBehaviour, IBullet
    {
        [SerializeField] private ColorType colorType = null!;
        private BaseDamage baseDamage = null!;

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
            baseDamage = GetComponent<BaseDamage>();
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
            gameObject.SetColorType(colorType);
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

        public void Despawn()
        {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }
    }
}