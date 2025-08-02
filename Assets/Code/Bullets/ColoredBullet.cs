using UnityEngine;

namespace GMTK25.Bullets
{
    public class ColoredBullet : BulletBase
    {
        [SerializeField] private ColorType colorType = null!;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetColorType(colorType);
        }
    }
}