using System;
using UnityEngine;

namespace GMTK25.Bullets {

    public interface IBullet {

        public float Damage { get; set; }

        public ColorType ColorType { get; }

        public BulletType CurrentBulletType { get; set; }

        public ColorType? LastHitColor { get; set; }

        public BulletType? LastHitBulletType { get; set; }

        public event Action<BulletType, ColorType?> SuccessHit;

        public event Action? FailHit;

        public void OnDespawnTimeReached();

        public void OnTriggerEnter2D(Collider2D other);

        public void Despawn();

    }

}