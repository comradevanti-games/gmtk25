using GMTK25.Bullets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GMTK25
{
    public class Revolver : MonoBehaviour
    {
        [SerializeField] private RevolverDrumKeeper drumKeeper = null!;
        [SerializeField] private GameObject bulletSpawnPoint = null!;
        [SerializeField] private float minMouseDistanceToShoot = 0;
        [SerializeField] private float shootCooldown = 0;
        [SerializeField] private AudioSource audioSrc = null!;
        [SerializeField] private AudioClip bulletReturned = null!;
        [SerializeField] private GameObject gamePadCrosshair = null!;

        private InputHandler? inputHandler;
        private float lastShotTime = 0;
        private ScreenShake? screenShaker;

        public ColorType? LastSuccessColorType { get; private set; }

        private void Awake()
        {
            inputHandler =
                FindFirstObjectByType<InputHandler>(FindObjectsInactive.Exclude)
                    .GetComponent<InputHandler>();
            inputHandler.ShootInputHandled += OnShootInput;
            screenShaker = Singletons.Require<ScreenShake>();
        }

        private void OnShootInput()
        {
            if (Vector2.Distance(inputHandler!.MouseScreenPosition,
                    transform.position) <
                minMouseDistanceToShoot)
                return;

            if (Time.time - lastShotTime >= shootCooldown)
            {
                if (inputHandler.CurrentControlScheme == "MKB")
                    Shoot(inputHandler!.MouseScreenPosition);

                if (inputHandler.CurrentControlScheme == "Gamepad")
                    Shoot(gamePadCrosshair.transform.position);

                lastShotTime = Time.time;
            }
        }

        private void Shoot(Vector2 dir)
        {
            if (drumKeeper == null) return;

            var newBullet = drumKeeper.ChamberedBulletType;

            if (newBullet == null) return;

            var bulletGameObject = Instantiate(newBullet.Prefab,
                bulletSpawnPoint.transform.position,
                bulletSpawnPoint.transform.rotation);

            var bullet = bulletGameObject.GetComponent<Bullet>();
            bullet.Type = newBullet;
            bullet.FailHit += OnFailHit;

            var bulletBody = bulletGameObject.GetComponent<Rigidbody2D>();
            var shootDirection =
                dir - (Vector2)bulletSpawnPoint.transform.position;
            bulletBody.AddForce(newBullet.InitialSpeed *
                                shootDirection.normalized,
                ForceMode2D.Impulse);

            audioSrc.Play();
            audioSrc.pitch = Random.Range(0.9f, 1.1f);

            screenShaker!.Shake(0.1f, 1.2f, 1.2f, 5);

            drumKeeper.EjectBullet();
        }

        public void ReturnBulletLike(GameObject bullet)
        {
            var type = bullet.GetComponent<Bullet>().Type;
            var color = bullet.TryGetColorType();

            LastSuccessColorType = color;
            drumKeeper.PushBullet(type);

            audioSrc.PlayOneShot(bulletReturned, 0.25f);
        }

        private void OnFailHit()
        {
            LastSuccessColorType = null;
        }
    }
}