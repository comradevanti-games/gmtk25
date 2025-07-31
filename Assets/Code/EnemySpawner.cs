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

            Debug.Log(
                $"New enemy spawned at {position} ⚡ ({color.name}, {type.name})",
                this);

            var enemy = Instantiate(type.Prefab, position, Quaternion.identity);

            enemy.GetComponent<SpriteRenderer>().color = color.Color;
        }

        private void Awake()
        {
            colors = Resources.LoadAll<EnemyColor>("EnemyColors");
        }
    }
}