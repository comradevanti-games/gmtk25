using System;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class NormalBullet : BulletBase
    {
        public override BulletType CurrentBulletType { get; set; } = null!;

        public override ColorType? LastHitColor { get; set; }

        public override BulletType? LastHitBulletType { get; set; }

        public override event Action<BulletType, ColorType?>? SuccessHit;

        public override void OnTriggerEnter2D(Collider2D other)
        {
            var hit = new BulletHit(other.gameObject);

            if (hit.Health is { } health)
            {
                var damage = DamageFor(hit);
                health.TakeDamage(damage);
            }

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
                    SuccessHit?.Invoke(CurrentBulletType,
                        gameObject.TryGetColorType());
                    Despawn();

                    break;
            }
        }
    }
}