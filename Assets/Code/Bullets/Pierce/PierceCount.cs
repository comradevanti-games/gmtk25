using UnityEngine;

namespace GMTK25.Bullets
{
    public sealed class PierceCount : MonoBehaviour, IContinueBehavior
    {
        public int Value { get; private set; }

        public void OnBulletContinues(BulletHit hit)
        {
            Value++;
        }
    }
}