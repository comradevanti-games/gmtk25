using System;
using System.Linq;
using GMTK25.Bullets;
using GMTK25.UI;
using TMPro;
using UnityEngine;

namespace GMTK25
{
    public sealed class RevolverDrumKeeper : MonoBehaviour
    {
        [SerializeField] private int drumSize;

        [SerializeField]
        private BulletType[] initialBulletTypes = Array.Empty<BulletType>();

        [SerializeField] private AudioSource audioSrc = null!;

        private LoopQueue<BulletType> bullets = null!;
        private DrumDisplay display = null!;

        public BulletType? ChamberedBulletType => bullets.Peek();


        /// <summary>
        /// Ejects the current <see cref="ChamberedBulletType"/> (if any).
        /// </summary>
        public void EjectBullet()
        {
            bullets.Dequeue();
            display.RemoveFirst();
        }

        /// <summary>
        /// Adds a bullet to this drum. This will eject the
        /// <see cref="ChamberedBulletType"/> if the drum is full.
        /// The pushed bullet will be the last in the drum.
        /// </summary>
        public void PushBullet(BulletType type)
        {
            var removed = bullets.Enqueue(type);

            if (removed) display.RemoveFirst();
            display.Push(type);
        }

        public void PlaySfx(string sfxName)
        {
            switch (sfxName)
            {
                case "Pickup":
                    audioSrc.Play();

                    break;
            }
        }

        private void Awake()
        {
            bullets = new LoopQueue<BulletType>(drumSize);
            display = FindAnyObjectByType<DrumDisplay>();
        }

        private void Start()
        {
            for (var i = 0; i < drumSize; i++)
                PushBullet(initialBulletTypes[i]);
        }
    }
}