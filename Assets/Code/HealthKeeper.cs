using System;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK25
{
    public sealed class HealthKeeper : MonoBehaviour
    {
        public UnityEvent<float> changed = new UnityEvent<float>();
        public UnityEvent died = new UnityEvent();

        [SerializeField] private float initialHealth;

        private float health;

        public float Health
        {
            get => health;
            private set
            {
                var newHealth = Mathf.Max(value, 0);

                if (Mathf.Approximately(newHealth, health)) return;
                health = newHealth;

                changed.Invoke(health);

                if (health != 0) return;

                Debug.Log("I died 💀", this);
                died.Invoke();
            }
        }

        public void TakeDamage(float amount)
        {
            Debug.Log("I took damage 🤕", this);
            Health -= amount;
        }

        private void Awake()
        {
            Health = initialHealth;
        }
    }
}