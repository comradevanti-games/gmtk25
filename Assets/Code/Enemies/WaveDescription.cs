using System;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK25.Enemies
{
    [Serializable]
    public class EnemyGroup
    {
        [SerializeField] private EnemyType type = null!;
        [SerializeField] private int count;

        public EnemyType Type => type;

        public int Count => count;
    }

    [Serializable]
    public class SubWave
    {
        [SerializeField]
        private EnemyGroup[] groups = Array.Empty<EnemyGroup>();

        [SerializeField] private float durationSeconds;

        public IReadOnlyCollection<EnemyGroup> Groups => groups;

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