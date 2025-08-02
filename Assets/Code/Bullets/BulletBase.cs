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

        public abstract ColorType? LastHitColor { get; set; }

        public abstract BulletType? LastHitBulletType { get; set; }

        public abstract event Action<BulletType, ColorType?>? SuccessHit;


        public void OnDespawnTimeReached()
        {
            Singletons.Require<BulletPickupHandler>()
                .SpawnPickupLike(gameObject);
            Despawn();
        }

        public abstract void OnTriggerEnter2D(Collider2D other);

        protected void Despawn()
        {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }
    }
}