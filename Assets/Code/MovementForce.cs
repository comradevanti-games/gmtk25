using UnityEngine;

namespace GMTK25
{
    public sealed class MovementForce : MonoBehaviour
    {
        [SerializeField] private float baseForce;

        public float Force => baseForce;
    }
}