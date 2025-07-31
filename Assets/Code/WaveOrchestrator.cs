using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GMTK25
{
    public sealed class WaveOrchestrator : MonoBehaviour
    {
        [SerializeField] private WaveDescription waveDescription = null!;

        private EnemySpawner enemySpawner = null!;

        private async Task RunWave(int waveIndex)
        {
            Debug.Log($"Start wave {waveIndex}", this);
            var wave = waveDescription.Waves[waveIndex];
            var remainingSubWaves = wave.SubWaves.ToList();

            while (remainingSubWaves.Count > 0)
            {
                var subWave = remainingSubWaves.RemoveRandom();
                Debug.Log($"Start sub-wave ({subWave.Duration} until next)",
                    this);

                foreach (var group in subWave.Groups)
                    for (var i = 0; i < group.Count; i++)
                        enemySpawner.SpawnEnemy(group.Type);

                await Task.Delay(subWave.Duration, destroyCancellationToken);
            }

            Debug.Log($"End wave {waveIndex}", this);

            if (waveIndex < waveDescription.Waves.Count - 1)
            {
                Debug.Log($"Next wave starts in {wave.Delay}");
                await Task.Delay(wave.Delay, destroyCancellationToken);
                _ = RunWave(waveIndex + 1);
            }
            else
            {
                Debug.Log("Last wave completed. We are done 🥳", this);
            }
        }

        private async void Start()
        {
            try
            {
                await RunWave(0);
            }
            catch (OperationCanceledException)
            {
                // Nothing
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Awake()
        {
            enemySpawner = Singletons.Require<EnemySpawner>();
        }
    }
}