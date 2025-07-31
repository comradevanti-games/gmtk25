using System;
using UnityEngine;

namespace GMTK25
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(MovementForce))]
    public sealed class TargetFollower : MonoBehaviour
    {
        [SerializeField] private string targetTag = "";

        private new Rigidbody2D rigidbody = null!;
        private Transform targetTransform = null!;
        private MovementForce movementForce = null!;


        private void OnEnable()
        {
            Debug.Log(
                $"{gameObject.name} is following {targetTransform.name} 🏃‍");
        }

        private void OnDisable()
        {
            Debug.Log(
                $"{gameObject.name} stopped following {targetTransform.name} 🖐");
        }

        private void FixedUpdate()
        {
            var dir = (targetTransform.position - transform.position)
                .normalized;
            rigidbody.AddForce(dir * movementForce.Force);
        }

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            movementForce = GetComponent<MovementForce>();
            targetTransform =
                GameObject.FindGameObjectWithTag(targetTag).transform;
        }
    }
}