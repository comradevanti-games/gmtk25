using UnityEngine;

namespace GMTK25
{
    public sealed class RevolverDrumKeeper : MonoBehaviour
    {
        [SerializeField] private BulletType initialBulletType = null!;

        public BulletType? ChamberedBulletType => initialBulletType;
    }
}