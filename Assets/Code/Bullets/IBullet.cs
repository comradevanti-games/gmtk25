using System;
using UnityEngine;

namespace GMTK25.Bullets
{
    public interface IBullet
    {
        public BulletType CurrentBulletType { get; set; }

        public event Action<BulletType, ColorType?> SuccessHit;

        public event Action? FailHit;
    }
}