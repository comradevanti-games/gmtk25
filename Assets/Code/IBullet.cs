using System;
using UnityEngine;

namespace GMTK25 {

    public interface IBullet {

        public BulletType CurrentBulletType { get; set; }

        public event Action<BulletType> SuccessHit;

        public void OnDespawnTimeReached();

        public void OnTriggerEnter2D(Collider2D other);

        public void Despawn();

    }

}