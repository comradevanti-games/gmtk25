using System;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK25
{
    [Serializable]
    public class EnemyGroup
    {
        [SerializeField] private float durationSeconds;

        public TimeSpan Duration => TimeSpan.FromSeconds(durationSeconds);
    }

    [Serializable]
    public class Wave
    {
        [SerializeField]
        private EnemyGroup[] enemyGroups = Array.Empty<EnemyGroup>();

        [SerializeField] private float delaySeconds;

        public IReadOnlyList<EnemyGroup> EnemyGroups => enemyGroups;
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