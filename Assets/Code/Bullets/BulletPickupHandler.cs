using UnityEngine;

namespace GMTK25.Bullets
{
    public class BulletPickupHandler : MonoBehaviour
    {
        [SerializeField] private AudioClip missSfx = null!;

        private PickupSpawner spawner = null!;
        private Jukebox jukebox = null!;

        private void Awake()
        {
            jukebox = Singletons.Require<Jukebox>();
            spawner = Singletons.Require<PickupSpawner>();
        }

        public void SpawnPickupLike(GameObject bullet)
        {
            jukebox.Play(missSfx);

            var bulletType = bullet.GetComponent<Bullet>().Type;
            var bulletColor = bullet.TryGetColorType();

            spawner.SpawnPickup(
                new PickupSpawner.Request(bulletType, null, bulletColor));
        }
    }
}