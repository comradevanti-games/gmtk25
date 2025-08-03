using System;
using UnityEngine;

namespace GMTK25.Bullets
{
    [RequireComponent(typeof(PierceCount))]
    public sealed class PierceCountContinueFilter : MonoBehaviour,
        IContinueFilter
    {
        [SerializeField] private int maxPierces;

        private PierceCount pierceCount = null!;

        public bool ShouldContinue(BulletHit hit)
        {
            return pierceCount.Value < maxPierces;
        }

        private void Awake()
        {
            pierceCount = GetComponent<PierceCount>();
        }
    }
}