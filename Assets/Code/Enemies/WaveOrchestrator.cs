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
        /// <summary>
        /// Called when the break starts. The payload is how many sub-waves
        /// the player has completed early.
        /// </summary>
        public UnityEvent<int> breakStarted = new UnityEvent<int>();

        public UnityEvent waveStarted = new UnityEvent();

        public UnityEvent wonEarly = new UnityEvent();

        public UnityEvent wonGame = new UnityEvent();

        [SerializeField] private WaveDescription waveDescription = null!;
        [SerializeField] private float breakTimeSeconds;

        private TimeSpan BreakTime => TimeSpan.FromSeconds(breakTimeSeconds);

        private EnemySpawner enemySpawner = null!;
        private EnemyTracker enemyTracker = null!;
        private int newGamePlusCounter = 1;

        private async Task RunWave(int waveIndex, CancellationToken ct)
        {
            var wave = waveDescription.Waves[waveIndex];
            var remainingSubWaves = wave.SubWaves.ToList();
            waveStarted.Invoke();

            var earlyWins = 0;
            while (remainingSubWaves.Count > 0)
            {
                var subWave = remainingSubWaves.RemoveRandom();

                foreach (var group in subWave.Groups)
                    for (var i = 0; i < group.Count * newGamePlusCounter; i++)
                        enemySpawner.SpawnEnemy(group.Type);

                await CustomTask.CancellableDelay(subWave.Duration, () =>
                {
                    var isDone = !enemyTracker.HasEnemies;

                    if (isDone)
                    {
                        wonEarly.Invoke();
                        earlyWins++;
                    }

                    return !isDone;
                }, ct);
            }

            while (enemyTracker.HasEnemies) await Task.Yield();

            if (waveIndex < waveDescription.Waves.Count - 1)
            {
                breakStarted.Invoke(earlyWins);
                await Task.Delay(BreakTime, ct);
                _ = RunWave(waveIndex + 1, ct);
            }
            else {
                wonGame.Invoke();
            }
        }

        public void OnNewGamePlusStarted() {
            Restart();
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