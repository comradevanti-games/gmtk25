using System;
using GMTK25.Enemies;
using UnityEngine;

namespace GMTK25.Bullets
{
    public class ColoredLinkBullet : BulletBase
    {
        [SerializeField] private ColorType colorType = null!;
        [SerializeField] private BulletType linkBulletType = null!;

        private Revolver revolver = null!;

        private JumpCount jumpCount = null!;

        public override BulletType CurrentBulletType { get; set; } = null!;

        public override event Action<BulletType, ColorType?>? SuccessHit;


        protected override void Awake()
        {
            base.Awake();

            gameObject.SetColorType(colorType);
            jumpCount = GetComponent<JumpCount>();
            revolver = FindAnyObjectByType<Revolver>();
        }

        protected override void OnBulletHitEnemy(BulletHit hit)
        {
            base.OnBulletHitEnemy(hit);

            if (hit.TargetColor == colorType)
            {
                if (revolver.LastSuccessColorType == hit.TargetColor)
                {
                    var enemyPos = Singletons.Require<EnemyTracker>()
                        .GetClosestEnemyPosition(
                            hit.Target.transform.position, 2);

                    if (enemyPos.x < 25) ShootLinkBullet(enemyPos);
                }

                SuccessHit?.Invoke(CurrentBulletType,
                    gameObject.GetColorType());
                Despawn();
            }
            else
            {
                Miss(true);
            }
        }

        private void ShootLinkBullet(Vector2 enemyPos)
        {
            var shootDirection = enemyPos - (Vector2)transform.position;
            var bulletRotation =
                Quaternion.Euler(0, 0,
                    Mathf.Atan2(shootDirection.y, shootDirection.x) *
                    Mathf.Rad2Deg - 90);
            var bulletGameObject = Instantiate(linkBulletType.Prefab,
                transform.position, bulletRotation);

            var bullet = bulletGameObject.GetComponent<ColoredLinkBullet>();
            bullet.CurrentBulletType = linkBulletType;
            bullet.jumpCount.Value = jumpCount.Value + 1;

            var bulletBody = bulletGameObject.GetComponent<Rigidbody2D>();
            bulletBody.AddForce(linkBulletType.InitialSpeed *
                                shootDirection.normalized);
        }
    }
}