using System;
using UnityEngine;

namespace GMTK25
{
    public sealed class PickupSpawner : MonoBehaviour
    {
        public record Request(BulletType Type, Vector2? Location);

        [SerializeField] private GameObject pickupPrefab = null!;

        private StageLocationFinder locationFinder = null!;

        public void SpawnPickup(Request request)
        {
            var location = request.Location ??
                           locationFinder.PickRandomFreeLocation();
            Instantiate(pickupPrefab, location, Quaternion.identity)
                .GetComponent<Pickup>().BulletType = request.Type;
        }

        private void Awake()
        {
            locationFinder = Singletons.Require<StageLocationFinder>();
        }
    }
}