using System;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK25
{
    [Serializable]
    public class SubWave
    {
        [SerializeField] private float durationSeconds;

        public TimeSpan Duration => TimeSpan.FromSeconds(durationSeconds);
    }

    [Serializable]
    public class Wave
    {
        [SerializeField] private SubWave[] subWaves = Array.Empty<SubWave>();

        [SerializeField] private float delaySeconds;

        public IReadOnlyList<SubWave> SubWaves => subWaves;

        public TimeSpan Delay => TimeSpan.FromSeconds(delaySeconds);
    }

    [CreateAssetMenu(fileName = "New Wave-description",
        menuName = "GMTK25/Wave-description")]
    public sealed class WaveDescription : ScriptableObject
    {
        [SerializeField] private Wave[] waves = Array.Empty<Wave>();

        public IReadOnlyList<Wave> Waves => waves;
    }
}