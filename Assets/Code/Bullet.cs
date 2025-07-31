using System;
using UnityEngine;

namespace GMTK25 {

    public class Bullet : MonoBehaviour {

        public BulletType? CurrentBulletType { get; private set; }

        public event Action<BulletType>? HasDespawned;

        public event Action<BulletType>? HasHitWall;

        public event Action<BulletType>? HasHitEnemy;

        private void Awake() {
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
        }

        public void SetBulletType(BulletType type) {
            CurrentBulletType = type;
        }

        private void OnDespawnTimeReached() {
            HasDespawned?.Invoke(CurrentBulletType!);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.layer == 8) {
                Debug.Log("Hit the wall! 🧱");
                HasHitWall?.Invoke(CurrentBulletType!);
            }

            if (other.gameObject.layer == 9) {
                Debug.Log("Hit the enemy! 👽");
                HasHitEnemy?.Invoke(CurrentBulletType!);
            }
        }

    }

}