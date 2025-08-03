using UnityEngine;

namespace GMTK25.Bullets
{
    public sealed class ColorMatchDamageMultiplier : MonoBehaviour,
        IDamageMultiplier
    {
        [SerializeField] private float multiplier;

        private ColorType ownColor = null!;

        public float CalcMultiplier(BulletHit hit)
        {
            var matches = ownColor == hit.TargetColor;
            return matches ? multiplier : 1;
        }

        private void Start()
        {
            ownColor = gameObject.GetColorType();
        }
    }
}