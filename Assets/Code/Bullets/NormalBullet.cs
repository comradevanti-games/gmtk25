using System;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class NormalBullet : BulletBase
    {
        protected override void OnBulletHitEnemy(BulletHit hit)
        {
            base.OnBulletHitEnemy(hit);
            ReturnToPlayer();
        }
    }
}