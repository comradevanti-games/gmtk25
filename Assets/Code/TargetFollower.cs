using UnityEngine;

namespace GMTK25
{
    [RequireComponent(typeof(TargetMover))]
    public sealed class TargetFollower : MonoBehaviour
    {
        [SerializeField] private string targetTag = "";

        private Transform targetTransform = null!;
        private TargetMover mover = null!;


        private void OnEnable()
        {
            if (!targetTransform) return;
            Debug.Log(
                $"Following {targetTransform.name} 🏃‍",
                this);
            mover.TargetPosition = targetTransform.position;
        }

        private void OnDisable()
        {
            mover.TargetPosition = null;
            if (!targetTransform) return;
            Debug.Log(
                $"Stopped following {targetTransform.name} 🖐",
                this);
        }

        private void FixedUpdate()
        {
            mover.TargetPosition = targetTransform.position;
        }

        private void Awake()
        {
            mover = GetComponent<TargetMover>();
            targetTransform =
                GameObject.FindGameObjectWithTag(targetTag).transform;
        }
    }
}