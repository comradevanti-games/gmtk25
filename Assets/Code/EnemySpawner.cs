using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GMTK25
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        private EnemyColor[] colors = Array.Empty<EnemyColor>();

        private Vector2 PickSpawnPosition()
        {
            return new Vector2(
                Random.Range(-10f, 10),
                Random.Range(-10f, 10)
            );
        }

        public void SpawnEnemy(EnemyType type)
        {
            var color = colors.GetRandom();
            var position = PickSpawnPosition();

            var enemy = Instantiate(type.Prefab, position, Quaternion.identity);
            enemy.name = $"{color.name} {type.name} {Random.Range(0, 1000)}";

            Debug.Log($"New enemy spawned 👶", enemy);

            enemy.GetComponent<SpriteRenderer>().color = color.Color;
        }

        private void Awake()
        {
            colors = Resources.LoadAll<EnemyColor>("EnemyColors");
        }
    }
}