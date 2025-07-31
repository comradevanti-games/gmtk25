using System;
using UnityEngine;

namespace GMTK25 {

    public class Revolver : MonoBehaviour {

        [SerializeField] private RevolverDrumKeeper drumKeeper = null!;
        [SerializeField] private GameObject bulletSpawnPoint = null!;
        [SerializeField] private float minMouseDistanceToShoot = 0;
        private InputHandler? inputHandler;

        private void Awake() {
            inputHandler = FindFirstObjectByType<InputHandler>(FindObjectsInactive.Exclude).GetComponent<InputHandler>();
            inputHandler.ShootInputHandled += OnShootInput;
        }

        private void OnShootInput() {

            if (Vector2.Distance(inputHandler!.MouseScreenPosition, transform.position) <
                minMouseDistanceToShoot) {
                return;
            }

            Shoot();
        }

        private void Shoot() {

            if (drumKeeper == null) {
                return;
            }

            BulletType? newBullet = drumKeeper.ChamberedBulletType;

            if (newBullet == null) {
                return;
            }

            GameObject bullet = Instantiate(newBullet.Prefab, bulletSpawnPoint.transform.position,
                bulletSpawnPoint.transform.rotation);
            bullet.GetComponent<Bullet>().SetBulletType(newBullet);
            Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
            Vector2 shootDirection = inputHandler!.MouseScreenPosition - (Vector2)bulletSpawnPoint.transform.position;
            bulletBody.AddForce(newBullet.InitialSpeed * shootDirection.normalized);

            drumKeeper.EjectBullet();
        }

    }

}