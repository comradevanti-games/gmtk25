using UnityEngine;

namespace GMTK25.Bullets
{
    public sealed class ConstantContinueFilter : MonoBehaviour, IContinueFilter
    {
        [SerializeField] private bool shouldContinue;

        public bool ShouldContinue(BulletHit hit)
        {
            return shouldContinue;
        }
    }
}