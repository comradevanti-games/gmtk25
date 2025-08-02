using System;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class NormalBullet : BulletBase
    {
        public override BulletType CurrentBulletType { get; set; } = null!;

        protected override void OnBulletHitEnemy(BulletHit hit)
        {
            base.OnBulletHitEnemy(hit);
            ReturnToPlayer();
        }
    }
}