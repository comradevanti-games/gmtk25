using UnityEngine;

namespace GMTK25
{
    public sealed class HealthKeeper : MonoBehaviour
    {
        [SerializeField] private float initialHealth;

        public float Health { get; private set; }

        public void TakeDamage(float amount)
        {
            Health = Mathf.Max(Health - amount, 0);
        }

        private void Awake()
        {
            Health = initialHealth;
        }
    }
}