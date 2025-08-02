using GMTK25.Enemies;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class ColoredLinkBullet : Bullet
    {
        [SerializeField] private ColorType colorType = null!;

        private Revolver revolver = null!;
        private JumpCount jumpCount = null!;


        protected override void Awake()
        {
            base.Awake();

            gameObject.SetColorType(colorType);
            jumpCount = GetComponent<JumpCount>();
            revolver = FindAnyObjectByType<Revolver>();
        }

        protected override void OnReturnsToPlayer(BulletHit hit)
        {
            if (revolver.LastSuccessColorType != hit.TargetColor) return;

            var enemyPos = Singletons.Require<EnemyTracker>()
                .GetClosestEnemyPosition(
                    hit.Target.transform.position, 2);

            if (enemyPos.x < 25) ShootLinkBullet(enemyPos);
        }

        private void ShootLinkBullet(Vector2 enemyPos)
        {
            var shootDirection = enemyPos - (Vector2)transform.position;
            var bulletRotation =
                Quaternion.Euler(0, 0,
                    Mathf.Atan2(shootDirection.y, shootDirection.x) *
                    Mathf.Rad2Deg - 90);
            var bulletGameObject = Instantiate(Type.Prefab,
                transform.position, bulletRotation);

            var bullet = bulletGameObject.GetComponent<ColoredLinkBullet>();
            bullet.Type = Type;
            bullet.jumpCount.Value = jumpCount.Value + 1;

            var bulletBody = bulletGameObject.GetComponent<Rigidbody2D>();
            bulletBody.AddForce(Type.InitialSpeed * shootDirection.normalized);
        }
    }
}