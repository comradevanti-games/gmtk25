using UnityEngine;

namespace GMTK25.Bullets
{
    public record BulletHit(GameObject Target)
    {
        public ColorType TargetColor => Target.GetColorType();

        public HealthKeeper? Health => Target.GetComponent<HealthKeeper>();
    }
}