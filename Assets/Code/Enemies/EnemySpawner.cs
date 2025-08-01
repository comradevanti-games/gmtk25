using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GMTK25.Enemies
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float activationDelaySeconds;

        private EnemyTracker enemyTracker = null!;
        private StageLocationFinder locationFinder = null!;
        private ColorType[] colors = Array.Empty<ColorType>();

        private TimeSpan ActivationDelay =>
            TimeSpan.FromSeconds(activationDelaySeconds);


        private async Task DelayedActivate(GameObject enemy,
            CancellationToken ct)
        {
            var enemyRenderer = enemy.GetComponent<SpriteRenderer>();
            enemyRenderer.color = enemyRenderer.color.WithAlpha(0.25f);

            await Task.Delay(ActivationDelay, ct);

            // Handle case where enemy has despawned in the meantime
            // for whatever reason.
            if (!enemy) return;

            enemy.GetComponent<EnemyBrain>().enabled = true;
            enemy.GetComponent<Collider2D>().enabled = true;

            enemyRenderer.color = enemyRenderer.color.WithAlpha(1);
        }

        public void SpawnEnemy(EnemyType type)
        {
            var color = colors.GetRandom();
            var position = locationFinder.PickRandomFreeLocation();

            var enemy = Instantiate(type.Prefab, position, Quaternion.identity);
            enemy.name = $"{color.name} {type.name} {Random.Range(0, 1000)}";
            enemy.SetColorType(color);
            enemy.Tint(color.Color);

            enemyTracker.RegisterEnemy(enemy);

            this.RunTask((ct) => DelayedActivate(enemy, ct));
        }

        private void Awake()
        {
            colors = Resources.LoadAll<ColorType>("ColorTypes");
            enemyTracker = Singletons.Require<EnemyTracker>();
            locationFinder = Singletons.Require<StageLocationFinder>();
        }
    }
}