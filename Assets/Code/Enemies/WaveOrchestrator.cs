using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK25.Enemies
{
    public sealed class WaveOrchestrator : MonoBehaviour
    {
        public UnityEvent breakStarted = new UnityEvent();
        public UnityEvent waveStarted = new UnityEvent();

        [SerializeField] private WaveDescription waveDescription = null!;
        [SerializeField] private float breakTimeSeconds;

        private TimeSpan BreakTime => TimeSpan.FromSeconds(breakTimeSeconds);

        private EnemySpawner enemySpawner = null!;
        private EnemyTracker enemyTracker = null!;
        private int newGamePlusCounter = 1;

        private async Task RunWave(int waveIndex, CancellationToken ct)
        {
            Debug.Log($"Start wave {waveIndex}", this);
            var wave = waveDescription.Waves[waveIndex];
            var remainingSubWaves = wave.SubWaves.ToList();
            waveStarted.Invoke();

            while (remainingSubWaves.Count > 0)
            {
                var subWave = remainingSubWaves.RemoveRandom();
                Debug.Log($"Start sub-wave ({subWave.Duration} until next)",
                    this);

                foreach (var group in subWave.Groups)
                    for (var i = 0; i < group.Count * newGamePlusCounter; i++)
                        enemySpawner.SpawnEnemy(group.Type);

                await Task.Delay(subWave.Duration, ct);
            }

            Debug.Log($"Waiting for all enemies to be defeated 🫷");
            while (enemyTracker.HasEnemies) await Task.Yield();

            Debug.Log($"End wave {waveIndex}", this);

            if (waveIndex < waveDescription.Waves.Count - 1)
            {
                Debug.Log($"Next wave starts in {BreakTime} ⏰");
                breakStarted.Invoke();
                await Task.Delay(BreakTime, ct);
                _ = RunWave(waveIndex + 1, ct);
            }
            else
            {
                Debug.Log("Last wave completed. We are done 🥳. Or are we?",
                    this);
                Restart();
            }
        }

        private void Restart()
        {
            newGamePlusCounter++;
            this.RunTask((ct) => RunWave(0, ct));
        }

        private void Start()
        {
            this.RunTask((ct) => RunWave(0, ct));
        }

        private void Awake()
        {
            enemySpawner = Singletons.Require<EnemySpawner>();
            enemyTracker = Singletons.Require<EnemyTracker>();
        }
    }
}