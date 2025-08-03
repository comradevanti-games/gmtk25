using UnityEngine;

namespace GMTK25.Bullets.Return
{
    public sealed class ColorMatchReturnFilter : MonoBehaviour, IReturnFilter
    {
        private ColorType ownColor = null!;

        public ReturnAction ShouldReturn(BulletHit hit)
        {
            return ownColor == hit.TargetColor
                ? ReturnAction.Return
                : ReturnAction.BecomePickup;
        }

        private void Start()
        {
            ownColor = gameObject.GetColorType();
        }
    }
}