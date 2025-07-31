using UnityEngine;

namespace GMTK25 {

    public class BulletPickupHandler : MonoBehaviour {

        public void OnBulletFailed(BulletType hitType) {
            SpawnPickup(hitType);
        }

        public void SpawnPickup(BulletType type) { }

    }

}