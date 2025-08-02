using System;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class ColoredBullet : BulletBase
    {
        [SerializeField] private ColorType colorType = null!;

        public override BulletType CurrentBulletType { get; set; } = null!;


        protected override void Awake()
        {
            base.Awake();

            gameObject.SetColorType(colorType);
        }

        protected override void OnBulletHitEnemy(BulletHit hit)
        {
            base.OnBulletHitEnemy(hit);

            if (hit.TargetColor == gameObject.GetColorType())
                ReturnToPlayer();
            else
                Miss(true);
        }
    }
}