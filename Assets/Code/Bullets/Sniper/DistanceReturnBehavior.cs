using System;
using GMTK25.Bullets.Return;
using UnityEngine;

namespace GMTK25.Bullets
{
    public sealed class DistanceReturnBehavior : MonoBehaviour, IReturnFilter
    {
        [SerializeField] private float minDistance;

        private Vector2 startPosition;

        private void Start()
        {
            startPosition = transform.position;
        }

        public ReturnAction ShouldReturn(BulletHit hit)
        {
            var endPosition = transform.position;
            var distance = Vector2.Distance(startPosition, endPosition);
            return distance >= minDistance
                ? ReturnAction.Return
                : ReturnAction.BecomePickup;
        }
    }
}