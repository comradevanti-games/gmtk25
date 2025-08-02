using UnityEngine;

namespace GMTK25.Bullets
{
    public sealed class BaseDamage : MonoBehaviour
    {
        [SerializeField] private float value;

        public float Value => value;
    }
}