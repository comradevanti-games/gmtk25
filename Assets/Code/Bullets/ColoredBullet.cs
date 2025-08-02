using System;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class ColoredBullet : MonoBehaviour, IBullet
    {
        [SerializeField] private ColorType colorType = null!;
        private BaseDamage baseDamage = null!;

        public ColorType ColorType => colorType;

        public BulletType CurrentBulletType { get; set; } = null!;

        public ColorType? LastHitColor { get; set; }

        public BulletType? LastHitBulletType { get; set; }

        public event Action<BulletType, ColorType?>? SuccessHit;

        public event Action? FailHit;

        private void Awake()
        {
            baseDamage = GetComponent<BaseDamage>();
            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
            gameObject.SetColorType(colorType);
        }

        public void OnDespawnTimeReached()
        {
            Singletons.Require<BulletPickupHandler>()
                .OnBulletFailed(CurrentBulletType, ColorType);
            Despawn();
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 8)
            {
                if (other.gameObject.CompareTag("ShopItem"))
                {
                    Despawn();

                    return;
                }

                Singletons.Require<BulletPickupHandler>()
                    .OnBulletFailed(CurrentBulletType, ColorType);
                FailHit?.Invoke();
                Despawn();
            }

            if (other.gameObject.layer == 9)
            {
                if (other.gameObject.GetColorType() ==
                    gameObject.GetColorType())
                {
                    other.GetComponent<HealthKeeper>()
                        .TakeDamage(baseDamage.Value * 2);
                    SuccessHit?.Invoke(CurrentBulletType,
                        gameObject.GetColorType());
                }
                else
                {
                    Singletons.Require<BulletPickupHandler>()
                        .OnBulletFailed(CurrentBulletType, ColorType);
                    FailHit?.Invoke();
                    other.GetComponent<HealthKeeper>()
                        .TakeDamage(baseDamage.Value);
                }

                Despawn();
            }
        }

        public void Despawn()
        {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }
    }
}