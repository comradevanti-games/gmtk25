using System;
using UnityEngine;

namespace GMTK25 {

    public class PlayerCollision : MonoBehaviour {

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.layer == 9) { }
        }

    }

}