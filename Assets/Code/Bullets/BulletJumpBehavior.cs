using GMTK25.Bullets.Return;
using GMTK25.Enemies;
using UnityEngine;

namespace GMTK25.Bullets
{
    public sealed class BulletJumpBehavior : MonoBehaviour, IReturnBehavior
    {
        [SerializeField] private BulletType jumpBulletType = null!;


        private GameObject CreateJumpBullet(Quaternion rotation)
        {
            var jump = Instantiate(jumpBulletType.Prefab, transform.position,
                rotation);

            jump.GetComponent<BaseDamage>().Value /= 2;
            jump.GetComponent<Bullet>().Type = jumpBulletType;

            // Prevent spawned bullet from returning to player
            jump.AddComponent<ConstantReturnFilter>().Value = false;

            return jump;
        }

        private void ShootLinkBullet(Vector2 enemyPos)
        {
            var shootDirection = enemyPos - (Vector2)transform.position;
            var bulletRotation =
                Quaternion.Euler(0, 0,
                    Mathf.Atan2(shootDirection.y, shootDirection.x) *
                    Mathf.Rad2Deg - 90);

            var jump = CreateJumpBullet(bulletRotation);
            jump.GetComponent<Rigidbody2D>().AddForce(
                jumpBulletType.InitialSpeed *
                shootDirection.normalized,
                ForceMode2D.Impulse);
        }

        public void OnBulletReturnsToPlayer(BulletHit hit)
        {
            var enemyPos = Singletons.Require<EnemyTracker>()
                .GetClosestEnemyPosition(
                    hit.Target.transform.position, 2);

            if (enemyPos.x < 25) ShootLinkBullet(enemyPos);
        }
    }
}