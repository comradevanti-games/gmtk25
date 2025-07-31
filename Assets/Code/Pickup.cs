using System;
using UnityEngine;

namespace GMTK25 {

    public class Pickup : MonoBehaviour {

        public BulletType BulletType { get; set; } = null!;

        private void OnTriggerEnter2D(Collider2D other) {

            if (other.gameObject.CompareTag("Player")) {
                Debug.Log("Collected your bullet again! Phew! 😰");
                RevolverDrumKeeper rdk = other.GetComponentInChildren<RevolverDrumKeeper>();
                rdk.PushBullet(BulletType);
                rdk.PlaySfx("Pickup");
                Destroy(gameObject);
            }

        }

    }

}