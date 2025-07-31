using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GMTK25
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float activationDelaySeconds;
        private StageLocationFinder locationFinder = null!;
        private EnemyColor[] colors = Array.Empty<EnemyColor>();

        private TimeSpan ActivationDelay =>
            TimeSpan.FromSeconds(activationDelaySeconds);

        private static Color WithAlpha(Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }

        private async Task DelayedActivate(GameObject enemy,
            CancellationToken ct)
        {
            var enemyRenderer = enemy.GetComponent<SpriteRenderer>();
            enemyRenderer.color = WithAlpha(enemyRenderer.color, 0.25f);

            await Task.Delay(ActivationDelay, ct);

            enemy.GetComponent<EnemyBrain>().enabled = true;
            enemy.GetComponent<Collider2D>().enabled = true;

            enemyRenderer.color = WithAlpha(enemyRenderer.color, 1);
            Debug.Log($"Enemy activated", enemy);
        }

        public void SpawnEnemy(EnemyType type)
        {
            var color = colors.GetRandom();
            var position = locationFinder.PickRandomFreeLocation();

            var enemy = Instantiate(type.Prefab, position, Quaternion.identity);
            enemy.name = $"{color.name} {type.name} {Random.Range(0, 1000)}";

            Debug.Log($"New enemy spawned 👶", enemy);
            enemy.GetComponent<SpriteRenderer>().color = color.Color;

            this.RunTask((ct) => DelayedActivate(enemy, ct));
        }

        private void Awake()
        {
            colors = Resources.LoadAll<EnemyColor>("EnemyColors");
            locationFinder = Singletons.Require<StageLocationFinder>();
        }
    }
}