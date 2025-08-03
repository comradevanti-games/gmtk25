using UnityEngine;

namespace GMTK25.Bullets.Return
{
    public sealed class ConstantReturnFilter : MonoBehaviour, IReturnFilter
    {
        [SerializeField] private bool value;

        public bool Value
        {
            get => value;
            set => this.value = value;
        }

        public bool ShouldReturn(BulletHit hit)
        {
            return Value;
        }
    }
}