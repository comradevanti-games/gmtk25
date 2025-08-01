using System.Collections.Generic;
using UnityEngine;

namespace GMTK25.Enemies
{
    public sealed class EnemyTracker : MonoBehaviour
    {
        private readonly ISet<GameObject> enemies = new HashSet<GameObject>();

        public int EnemiesCount => enemies.Count;

        public bool HasEnemies => enemies.Count > 0;

        public void RegisterEnemy(GameObject enemy)
        {
            enemies.Add(enemy);

            enemy.GetComponent<HealthKeeper>().died.AddListener(() =>
            {
                enemies.Remove(enemy);
            });
        }
    }
}