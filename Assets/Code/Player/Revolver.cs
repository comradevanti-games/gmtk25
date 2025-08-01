using UnityEngine;
using Random = UnityEngine.Random;

namespace GMTK25 {

    public class Revolver : MonoBehaviour {

        [SerializeField] private RevolverDrumKeeper drumKeeper = null!;
        [SerializeField] private GameObject bulletSpawnPoint = null!;
        [SerializeField] private float minMouseDistanceToShoot = 0;
        [SerializeField] private float shootCooldown = 0;
        [SerializeField] private AudioSource audioSrc = null!;

        private InputHandler? inputHandler;
        private float lastShotTime = 0;
        private ScreenShake? screenShaker;
        private ColorType? lastSuccessColorType;
        private BulletType? lastSuccessBulletType;

        private void Awake() {
            inputHandler = FindFirstObjectByType<InputHandler>(FindObjectsInactive.Exclude).GetComponent<InputHandler>();
            inputHandler.ShootInputHandled += OnShootInput;
            screenShaker = Singletons.Require<ScreenShake>();
        }

        private void OnShootInput() {

            if (Vector2.Distance(inputHandler!.MouseScreenPosition, transform.position) <
                minMouseDistanceToShoot) {
                return;
            }

            if (Time.time - lastShotTime >= shootCooldown) {
                Shoot();
                lastShotTime = Time.time;
            }

        }

        private void Shoot() {

            if (drumKeeper == null) {
                return;
            }

            BulletType? newBullet = drumKeeper.ChamberedBulletType;

            if (newBullet == null) {
                return;
            }

            GameObject bulletGameObject = Instantiate(newBullet.Prefab, bulletSpawnPoint.transform.position,
                bulletSpawnPoint.transform.rotation);

            IBullet bullet = bulletGameObject.GetComponent<IBullet>();
            bullet.CurrentBulletType = newBullet;
            bullet.LastHitBulletType = lastSuccessBulletType;
            bullet.LastHitColor = lastSuccessColorType;
            bullet.SuccessHit += OnSuccessHit;
            bullet.FailHit += OnFailHit;

            Rigidbody2D bulletBody = bulletGameObject.GetComponent<Rigidbody2D>();
            Vector2 shootDirection = inputHandler!.MouseScreenPosition - (Vector2)bulletSpawnPoint.transform.position;
            bulletBody.AddForce(newBullet.InitialSpeed * shootDirection.normalized);

            audioSrc.Play();
            audioSrc.pitch = Random.Range(0.9f, 1.1f);

            screenShaker!.Shake(0.1f, 1.2f, 1.2f, 5);

            drumKeeper.EjectBullet();
        }

        private void OnSuccessHit(BulletType successHitType, ColorType? colorType) {
            lastSuccessBulletType = successHitType;
            lastSuccessColorType = colorType;
            drumKeeper.PushBullet(successHitType);
        }

        private void OnFailHit() {
            lastSuccessBulletType = null;
            lastSuccessColorType = null;
        }

    }

}