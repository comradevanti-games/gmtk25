using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK25.Enemies
{
    public sealed class EnemyTracker : MonoBehaviour
    {
        public UnityEvent allEnemiesDead = new UnityEvent();

        private readonly ISet<GameObject> enemies = new HashSet<GameObject>();

        public int EnemiesCount => enemies.Count;

        public bool HasEnemies => enemies.Count > 0;

        public void RegisterEnemy(GameObject enemy)
        {
            enemies.Add(enemy);

            enemy.GetComponent<HealthKeeper>().died.AddListener(() =>
            {
                enemies.Remove(enemy);

                if (enemies.Count == 0) allEnemiesDead.Invoke();
            });
        }
    }
}