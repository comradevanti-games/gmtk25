using UnityEngine;

namespace GMTK25.Bullets.Return
{
    public sealed class ColorMatchReturnFilter : MonoBehaviour, IReturnFilter
    {
        private ColorType ownColor = null!;

        public bool ShouldReturn(BulletHit hit)
        {
            return ownColor == hit.TargetColor;
        }

        private void Start()
        {
            ownColor = gameObject.GetColorType();
        }
    }
}