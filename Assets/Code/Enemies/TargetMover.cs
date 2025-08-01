using UnityEngine;

namespace GMTK25.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(MovementForce))]
    public sealed class TargetMover : MonoBehaviour
    {
        private new Rigidbody2D rigidbody = null!;
        private MovementForce movementForce = null!;

        public Vector3? TargetPosition { get; set; }


        private void FixedUpdate()
        {
            if (TargetPosition is not { } target) return;
            var dir = (target - transform.position).normalized;
            rigidbody.AddForce(dir * movementForce.Force);
        }

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            movementForce = GetComponent<MovementForce>();
        }
    }
}