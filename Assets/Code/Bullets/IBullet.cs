using System;
using UnityEngine;

namespace GMTK25.Bullets
{
    public interface IBullet
    {
        public BulletType Type { get; set; }

        public event Action? FailHit;
    }
}