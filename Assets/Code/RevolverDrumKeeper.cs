using UnityEngine;

namespace GMTK25
{
    public sealed class RevolverDrumKeeper : MonoBehaviour
    {
        [SerializeField] private BulletType initialBulletType = null!;

        public BulletType? ChamberedBulletType { get; private set; }

        /// <summary>
        /// Ejects the current <see cref="ChamberedBulletType"/> (if any).
        /// </summary>
        public void EjectBullet()
        {
            ChamberedBulletType = null;
        }

        private void Awake()
        {
            ChamberedBulletType = initialBulletType;
        }
    }
}