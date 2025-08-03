using UnityEngine;

namespace GMTK25.Bullets.Return
{
    public sealed class ConstantReturnFilter : MonoBehaviour, IReturnFilter
    {
        [SerializeField] private ReturnAction action;

        public ReturnAction Action
        {
            get => action;
            set => action = value;
        }

        public ReturnAction ShouldReturn(BulletHit hit)
        {
            return Action;
        }
    }
}