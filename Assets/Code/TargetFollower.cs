using System;
using UnityEngine;

namespace GMTK25
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class TargetFollower : MonoBehaviour
    {
        [SerializeField] private string targetTag = "";
        [SerializeField] private float force;

        private new Rigidbody2D rigidbody = null!;
        private Transform targetTransform = null!;


        private void FixedUpdate()
        {
            var dir = (targetTransform.position - transform.position)
                .normalized;
            rigidbody.AddForce(dir * force);
        }

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            targetTransform =
                GameObject.FindGameObjectWithTag(targetTag).transform;
        }
    }
}