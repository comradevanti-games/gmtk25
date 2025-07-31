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

        private void Awake()
        {
            bullets = new LoopQueue<BulletType>(drumSize);
            for (var i = 0; i < drumSize; i++)
                bullets.Enqueue(initialBulletType);
        }
    }
}