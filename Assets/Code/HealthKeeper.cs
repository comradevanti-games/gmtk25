using System;
using UnityEngine;

namespace GMTK25
{
    public sealed class HealthKeeper : MonoBehaviour
    {
        public event Action? Died;

        [SerializeField] private float initialHealth;
        private float health;

        public float Health
        {
            get => health;
            private set
            {
                if (Mathf.Approximately(value, health)) return;

                health = Mathf.Max(value, 0);

                if (health == 0) Died?.Invoke();
            }
        }

        public void TakeDamage(float amount)
        {
            Health -= amount;
        }

        private void Awake()
        {
            Health = initialHealth;
        }
    }
}