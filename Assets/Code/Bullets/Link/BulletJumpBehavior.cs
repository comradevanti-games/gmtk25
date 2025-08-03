using GMTK25.Enemies;
using UnityEngine;

namespace GMTK25.Bullets
{
    public sealed class BulletJumpBehavior : MonoBehaviour, IHitBehavior
    {
        [SerializeField] private BulletType jumpBulletType = null!;


        private GameObject CreateJumpBullet(Quaternion rotation)
        {
            var jump = Instantiate(jumpBulletType.Prefab, transform.position,
                rotation);

            jump.GetComponent<BaseDamage>().Value /= 2;

            // When the jump bullet hits we want to get back an instance
            // of this link bullet
            jump.GetComponent<Bullet>().Type = GetComponent<Bullet>().Type;

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

        public void OnHit(BulletHit hit)
        {
            var enemyPos = Singletons.Require<EnemyTracker>()
                .GetClosestEnemyPosition(
                    hit.Target.transform.position, 2);

            if (enemyPos.x < 25) ShootLinkBullet(enemyPos);
        }
    }
}