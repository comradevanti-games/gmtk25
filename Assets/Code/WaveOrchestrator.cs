using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GMTK25
{
    public sealed class WaveOrchestrator : MonoBehaviour
    {
        [SerializeField] private WaveDescription waveDescription = null!;


        private async Task RunWave(int waveIndex)
        {
            Debug.Log($"Start wave {waveIndex}", this);
            var wave = waveDescription.Waves[waveIndex];
            var remainingGroups = wave.EnemyGroups.ToList();

            while (remainingGroups.Count > 0)
            {
                var group = remainingGroups.RemoveRandom();
                Debug.Log($"Start group ({group.Duration} until next)",
                    this);

                await Task.Delay(group.Duration, destroyCancellationToken);
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
    }
}