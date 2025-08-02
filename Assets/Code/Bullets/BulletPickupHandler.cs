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

        public void OnBulletFailed(BulletType hitType, ColorType? colorType)
        {
            jukebox.Play(missSfx);
            spawner.SpawnPickup(
                new PickupSpawner.Request(hitType, null, colorType));
        }
    }
}