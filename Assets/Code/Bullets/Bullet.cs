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

        private IContinueFilter[] continueFilters =
            Array.Empty<IContinueFilter>();

        private IReturnFilter returnFilter = null!;

        public BulletType Type { get; set; } = null!;

        private void Awake()
        {
            baseDamage = GetComponent<BaseDamage>();
            damageMultipliers = GetComponents<IDamageMultiplier>();
            returnFilter = GetComponent<IReturnFilter>();
            continueFilters = GetComponents<IContinueFilter>();
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

        public void OnDespawnTimeReached()
        {
            Singletons.Require<BulletPickupHandler>()
                .SpawnPickupLike(gameObject);
            Despawn();
        }

        private void OnTriggerEnter2D(Collider2D other)
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

            var shouldContinue =
                continueFilters.All(it => it.ShouldContinue(hit));
            if (shouldContinue)
            {
                foreach (var it in GetComponents<IContinueBehavior>())
                    it.OnBulletContinues(hit);
                return;
            }

            foreach (var behavior in
                     GetComponents<IHitBehavior>())
                behavior.OnHit(hit);

            var returnAction = returnFilter.ShouldReturn(hit);

            switch (returnAction)
            {
                case ReturnAction.Consume:
                    Miss(false);
                    break;
                case ReturnAction.BecomePickup:
                    Miss(true);
                    break;
                case ReturnAction.Return:
                    foreach (var behavior in
                             GetComponents<IReturnBehavior>())
                        behavior.OnBulletReturnsToPlayer(hit);

                    ReturnToPlayer();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Despawn()
        {
            Destroy(gameObject);
        }
    }
}