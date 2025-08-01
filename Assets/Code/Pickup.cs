using System.Collections;
using UnityEngine;

namespace GMTK25 {

    public class Pickup : MonoBehaviour {

        [SerializeField] private Transform indicator = null!;

        public BulletType BulletType { get; set; } = null!;

        private void Awake() {
            StartCoroutine(Indicate(3, 0.15f));
        }

        private void OnTriggerEnter2D(Collider2D other) {

            if (other.gameObject.CompareTag("Player")) {
                RevolverDrumKeeper rdk = other.GetComponentInChildren<RevolverDrumKeeper>();
                rdk.PushBullet(BulletType);
                rdk.PlaySfx("Pickup");
                Destroy(gameObject);
            }

        }

        private IEnumerator Indicate(float speed, float maxDeviation) {
            Vector2 startPosition = indicator.position;

            while (true) {
                float newY = startPosition.y + Mathf.Sin(Time.time * speed) * maxDeviation;
                indicator.position = new Vector2(startPosition.x, newY);

                yield return null;
            }
        }

    }

}