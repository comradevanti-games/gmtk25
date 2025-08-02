using System;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class NormalBullet : BulletBase
    {
        public override BulletType CurrentBulletType { get; set; } = null!;

        public override event Action<BulletType, ColorType?>? SuccessHit;

        protected override void OnBulletHitEnemy(BulletHit hit)
        {
            base.OnBulletHitEnemy(hit);

            SuccessHit?.Invoke(CurrentBulletType,
                gameObject.TryGetColorType());
            Despawn();
        }
    }
}