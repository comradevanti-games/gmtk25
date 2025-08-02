using System;
using UnityEngine;

namespace GMTK25.Bullets
{
    public abstract class BulletBase : MonoBehaviour, IBullet
    {
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