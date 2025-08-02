using System;
using System.Linq;
using UnityEngine;

namespace GMTK25.Bullets
{
    public abstract class BulletBase : MonoBehaviour, IBullet
    {
        private BaseDamage baseDamage = null!;

        private IDamageMultiplier[] damageMultipliers =
            Array.Empty<IDamageMultiplier>();

        protected virtual void Awake()
        {
            baseDamage = GetComponent<BaseDamage>();
            damageMultipliers = GetComponents<IDamageMultiplier>();
        }

        protected float DamageFor(BulletHit hit)
        {
            var mult = damageMultipliers.Aggregate(1f,
                (acc, mult) => acc * mult.CalcMultiplier(hit));
            return baseDamage.Value * mult;
        }

        public abstract ColorType ColorType { get; }

        public abstract BulletType CurrentBulletType { get; set; }

        public abstract ColorType? LastHitColor { get; set; }

        public abstract BulletType? LastHitBulletType { get; set; }

        public abstract event Action<BulletType, ColorType?>? SuccessHit;

        public abstract event Action? FailHit;

        public abstract void OnDespawnTimeReached();

        public abstract void OnTriggerEnter2D(Collider2D other);

        public abstract void Despawn();
    }
}