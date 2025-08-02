using System;
using System.Linq;
using GMTK25.Bullets.Return;
using UnityEngine;

namespace GMTK25.Bullets
{
    public sealed class Bullet : MonoBehaviour
    {
        public event Action? FailHit;

        private BaseDamage baseDamage = null!;

        private IDamageMultiplier[] damageMultipliers =
            Array.Empty<IDamageMultiplier>();

        private IReturnFilter[] returnFilters =
            Array.Empty<IReturnFilter>();

        public BulletType Type { get; set; } = null!;

        private void Awake()
        {
            baseDamage = GetComponent<BaseDamage>();
            damageMultipliers = GetComponents<IDamageMultiplier>();
            returnFilters = GetComponents<IReturnFilter>();

            GetComponent<TimedDespawner>().Elapsed += OnDespawnTimeReached;
        }

        private void ReturnToPlayer()
        {
            FindAnyObjectByType<Revolver>().ReturnBulletLike(gameObject);
            Despawn();
        }

        private void Miss(bool respawnAsPickup)
        {
            if (respawnAsPickup)
                Singletons.Require<BulletPickupHandler>()
                    .SpawnPickupLike(gameObject);

            FailHit?.Invoke();

            Despawn();
        }

        private float DamageFor(BulletHit hit)
        {
            var mult = damageMultipliers.Aggregate(1f,
                (acc, mult) => acc * mult.CalcMultiplier(hit));
            return baseDamage.Value * mult;
        }

        private void OnDespawnTimeReached()
        {
            Singletons.Require<BulletPickupHandler>()
                .SpawnPickupLike(gameObject);
            Despawn();
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            var isEnemy = other.gameObject.layer == 9;
            if (!isEnemy)
            {
                var respawnAsPickup =
                    !other.gameObject.CompareTag("ShopItem");
                Miss(respawnAsPickup);
                return;
            }

            var hit = new BulletHit(other.gameObject);
            if (hit.Health is { } health)
            {
                var damage = DamageFor(hit);
                health.TakeDamage(damage);
            }

            if (!returnFilters.All(filter => filter.ShouldReturn(hit)))
            {
                Miss(true);
                return;
            }

            foreach (var behavior in
                     GetComponents<IReturnBehavior>())
                behavior.OnBulletReturnsToPlayer(hit);

            ReturnToPlayer();
        }

        private void Despawn()
        {
            GetComponent<TimedDespawner>().Elapsed -= OnDespawnTimeReached;
            Destroy(gameObject);
        }
    }
}