using UnityEngine;

namespace GMTK25.Bullets
{
    public sealed class JumpCountDamageMultiplier : MonoBehaviour,
        IDamageMultiplier
    {
        [SerializeField] private float multiplierPerJump;

        private BulletJumpBehavior jumpBehavior = null!;

        private void Awake()
        {
            jumpBehavior = GetComponent<BulletJumpBehavior>();
        }

        public float CalcMultiplier(BulletHit hit)
        {
            return Mathf.Pow(multiplierPerJump, jumpBehavior.JumpCount);
        }
    }
}