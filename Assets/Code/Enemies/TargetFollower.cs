using UnityEngine;

namespace GMTK25.Enemies {

    [RequireComponent(typeof(TargetMover))]
    public sealed class TargetFollower : MonoBehaviour {

        [SerializeField] private string targetTag = "";

        private Transform targetTransform = null!;
        private TargetMover mover = null!;

        private void OnEnable() {
            if (!targetTransform) return;
            mover.TargetPosition = targetTransform.position;
        }

        private void OnDisable() {
            mover.TargetPosition = null;

            if (!targetTransform) return;
        }

        private void FixedUpdate() {
            mover.TargetPosition = targetTransform.position;
        }

        private void Awake() {
            mover = GetComponent<TargetMover>();
            targetTransform =
                GameObject.FindGameObjectWithTag(targetTag).transform;
        }

    }

}