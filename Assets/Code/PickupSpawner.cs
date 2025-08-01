using System;
using UnityEngine;

namespace GMTK25 {

    public sealed class PickupSpawner : MonoBehaviour {

        public record Request(BulletType Type, Vector2? Location, ColorType? colorType);

        [SerializeField] private GameObject pickupPrefab = null!;

        private StageLocationFinder locationFinder = null!;

        public GameObject SpawnPickup(Request request) {

            var location = request.Location ?? locationFinder.PickRandomFreeLocation();
            var pickup = Instantiate(pickupPrefab, location, Quaternion.identity);

            pickup.GetComponent<Pickup>().BulletType = request.Type;
            pickup.GetComponent<SpriteRenderer>().sprite = request.Type.PickupSprite;

            if (request.colorType) {
                pickup.Tint(request.colorType.Color);
            }

            return pickup;
        }

        private void Awake() {
            locationFinder = Singletons.Require<StageLocationFinder>();
        }

    }

}