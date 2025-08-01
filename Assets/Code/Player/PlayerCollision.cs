using System;
using UnityEngine;

namespace GMTK25 {

    public class PlayerCollision : MonoBehaviour {

        private HealthKeeper? health;

        private void Awake() {
            health = GetComponent<HealthKeeper>();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.layer == 9) {
                health!.TakeDamage(1);
            }
        }

    }

}