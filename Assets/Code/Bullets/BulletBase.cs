using System;
using System.Linq;
using UnityEngine;

namespace GMTK25.Bullets
{
    public abstract class BulletBase : MonoBehaviour, IBullet
    {
        public event Action? FailHit;

        private BaseDamage baseDamage = null!;

        private IDamageMultiplier[] damageMultipliers =
            Array.Empty<IDamageMultiplier>();

        protected virtual void Awake()
        {
            baseDamage = GetComponent<BaseDamage>();
            damageMultipliers = GetComponents<IDamageMultiplier>();

            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
        }

        protected void Miss(bool respawnAsPickup)
        {
            if (respawnAsPickup)
                Singletons.Require<BulletPickupHandler>()
                    .SpawnPickupLike(gameObject);

            FailHit?.Invoke();

            Despawn();
        }

        protected float DamageFor(BulletHit hit)
        {
            var mult = damageMultipliers.Aggregate(1f,
                (acc, mult) => acc * mult.CalcMultiplier(hit));
            return baseDamage.Value * mult;
        }

        public abstract BulletType CurrentBulletType { get; set; }

        public abstract event Action<BulletType, ColorType?>? SuccessHit;

        public void OnDespawnTimeReached()
        {
            Singletons.Require<BulletPickupHandler>()
                .SpawnPickupLike(gameObject);
            Despawn();
        }

        protected virtual void OnBulletHitEnemy(BulletHit hit)
        {
            if (hit.Health is { } health)
            {
                var damage = DamageFor(hit);
                health.TakeDamage(damage);
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.layer)
            {
                case 8:
                {
                    if (other.gameObject.CompareTag("ShopItem"))
                        Miss(false);
                    else
                        Miss(true);
                    break;
                }

                case 9:
                    var hit = new BulletHit(other.gameObject);
                    OnBulletHitEnemy(hit);
                    break;
            }
        }


        protected void Despawn()
        {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }
    }
}