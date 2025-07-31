using UnityEngine;

namespace GMTK25
{
    public sealed class RevolverDrumKeeper : MonoBehaviour
    {
        [SerializeField] private int drumSize;
        [SerializeField] private BulletType initialBulletType = null!;

        private LoopQueue<BulletType> bullets = null!;

        public BulletType? ChamberedBulletType => bullets.Peek();

        /// <summary>
        /// Ejects the current <see cref="ChamberedBulletType"/> (if any).
        /// </summary>
        public void EjectBullet()
        {
            bullets.Dequeue();
        }

        /// <summary>
        /// Adds a bullet to this drum. This will eject the
        /// <see cref="ChamberedBulletType"/> if the drum is full.
        /// The pushed bullet will be the last in the drum.
        /// </summary>
        public void PushBullet(BulletType type)
        {
            bullets.Enqueue(type);
        }

        private void Awake()
        {
            bullets = new LoopQueue<BulletType>(drumSize);
            for (var i = 0; i < drumSize; i++)
                bullets.Enqueue(initialBulletType);
        }
    }
}