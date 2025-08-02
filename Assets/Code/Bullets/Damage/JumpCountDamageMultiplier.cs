using UnityEngine;

namespace GMTK25.Bullets
{
    public class JumpCountDamageMultiplier : MonoBehaviour, IDamageMultiplier
    {
        [SerializeField] private float multiplierPerJump;

        private JumpCount jumpCount = null!;

        private void Awake()
        {
            jumpCount = GetComponent<JumpCount>();
        }

        public float CalcMultiplier(BulletHit hit)
        {
            return Mathf.Pow(multiplierPerJump, jumpCount.Value);
        }
    }
}