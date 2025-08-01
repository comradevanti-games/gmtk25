using System;
using UnityEngine;

namespace GMTK25
{
    public sealed class PickupSpawner : MonoBehaviour
    {
        public record Request(BulletType Type, Vector2? Location);

        [SerializeField] private GameObject pickupPrefab = null!;

        private StageLocationFinder locationFinder = null!;

        public GameObject SpawnPickup(Request request)
        {
            var location = request.Location ??
                           locationFinder.PickRandomFreeLocation();

            var pickup =
                Instantiate(pickupPrefab, location, Quaternion.identity);

            pickup.GetComponent<Pickup>().BulletType = request.Type;

            var bulletPrefab = request.Type.Prefab;
            var bullet = bulletPrefab.GetComponent<ColoredBullet>();
            var color = bullet?.ColorType.Color;

            if (color != null) pickup.Tint(color.Value);

            return pickup;
        }

        private void Awake()
        {
            locationFinder = Singletons.Require<StageLocationFinder>();
        }
    }
}