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

        private int waveIndex = 0;

        private async Task RunWave(Wave wave)
        {
            Debug.Log($"Start wave {waveIndex}", this);
            var remainingGroups = wave.EnemyGroups.ToList();

            while (remainingGroups.Count > 0)
            {
                var group = remainingGroups.RemoveRandom();
                Debug.Log($"Start group ({group.Duration} until next)",this);

                await Task.Delay(group.Duration, destroyCancellationToken);
            }

            Debug.Log($"End wave {waveIndex}",this);
        }

        private async void StartWave()
        {
            try
            {
                var wave = waveDescription.Waves[waveIndex];
                await RunWave(wave);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Start()
        {
            StartWave();
        }
    }
}