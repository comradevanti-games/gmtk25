using System.Linq;
using TMPro;
using UnityEngine;

namespace GMTK25
{
    public sealed class RevolverDrumKeeper : MonoBehaviour
    {
        [SerializeField] private int drumSize;
        [SerializeField] private BulletType initialBulletType = null!;
        [SerializeField] private TMP_Text displayText = null!;

        private LoopQueue<BulletType> bullets = null!;

        public BulletType? ChamberedBulletType => bullets.Peek();

        private void UpdateDisplay()
        {
            var head = ChamberedBulletType is { } chambered
                ? $"<u>{char.ToUpper(chambered.name[0])}</u> "
                : "";

            var tail = string.Join(" ",
                bullets
                    .Skip(1)
                    .Select(type => type.name[0])
                    .Select(char.ToUpper));

            displayText.text = head + tail;
        }

        /// <summary>
        /// Ejects the current <see cref="ChamberedBulletType"/> (if any).
        /// </summary>
        public void EjectBullet()
        {
            bullets.Dequeue();
            UpdateDisplay();
        }

        /// <summary>
        /// Adds a bullet to this drum. This will eject the
        /// <see cref="ChamberedBulletType"/> if the drum is full.
        /// The pushed bullet will be the last in the drum.
        /// </summary>
        public void PushBullet(BulletType type)
        {
            bullets.Enqueue(type);
            UpdateDisplay();
        }

        private void Awake()
        {
            bullets = new LoopQueue<BulletType>(drumSize);
            for (var i = 0; i < drumSize; i++)
                bullets.Enqueue(initialBulletType);
        }

        private void Start()
        {
            UpdateDisplay();
        }
    }
}