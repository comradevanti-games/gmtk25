using GMTK25.Bullets.Return;
using GMTK25.Enemies;
using UnityEngine;

namespace GMTK25.Bullets
{
    public sealed class BulletJumpBehavior : MonoBehaviour, IReturnBehavior
    {
        private Revolver revolver = null!;
        private BulletJumpBehavior jumpCount = null!;
        private Bullet bullet = null!;

        public int JumpCount { get; private set; } = 1;

        private void Awake()
        {
            jumpCount = GetComponent<BulletJumpBehavior>();
            revolver = FindAnyObjectByType<Revolver>();
            bullet = GetComponent<Bullet>();
        }

        private GameObject CreateClone(Quaternion rotation)
        {
            var clone = Instantiate(bullet.Type.Prefab,
                transform.position, rotation);

            clone.GetComponent<Bullet>().Type = bullet.Type;
            clone.GetComponent<BulletJumpBehavior>().JumpCount =
                jumpCount.JumpCount + 1;

            return clone;
        }

        private void ShootLinkBullet(Vector2 enemyPos)
        {
            var shootDirection = enemyPos - (Vector2)transform.position;
            var bulletRotation =
                Quaternion.Euler(0, 0,
                    Mathf.Atan2(shootDirection.y, shootDirection.x) *
                    Mathf.Rad2Deg - 90);

            var clone = CreateClone(bulletRotation);
            clone.GetComponent<Rigidbody2D>().AddForce(
                bullet.Type.InitialSpeed *
                shootDirection.normalized,
                ForceMode2D.Impulse);
        }

        public void OnBulletReturnsToPlayer(BulletHit hit)
        {
            if (revolver.LastSuccessColorType != hit.TargetColor) return;

            var enemyPos = Singletons.Require<EnemyTracker>()
                .GetClosestEnemyPosition(
                    hit.Target.transform.position, 2);

            if (enemyPos.x < 25) ShootLinkBullet(enemyPos);
        }
    }
}