using UnityEngine;

namespace GMTK25 {

    public class BulletPickupHandler : MonoBehaviour {

        [SerializeField] private StageLocationFinder locationFinder = null!;
        [SerializeField] private GameObject pickupPrefab = null!;

        public void OnBulletFailed(BulletType hitType) {
            SpawnPickup(hitType);
        }

        private void SpawnPickup(BulletType type) {

            Vector2 location = locationFinder.PickRandomLocation();
            Instantiate(pickupPrefab, location, Quaternion.identity).GetComponent<Pickup>().BulletType = type;

        }

    }

}