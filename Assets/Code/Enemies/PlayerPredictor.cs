using UnityEngine;

namespace GMTK25.Enemies
{
    [RequireComponent(typeof(TargetMover))]
    public sealed class PlayerPredictor : MonoBehaviour
    {
        [SerializeField] private float predictionTimeSeconds;

        private PlayerMovement player = null!;
        private TargetMover mover = null!;


        private void OnEnable()
        {
            if (!player) return;
            Debug.Log($"Predicting {player.name} 🏃‍", this);
        }

        private void OnDisable()
        {
            mover.TargetPosition = null;
            if (!player) return;
            Debug.Log($"Stopped predicting {player.name} 🖐", this);
        }

        private void FixedUpdate()
        {
            var predictedPosition = (Vector2)player.transform.position +
                                    player.Velocity *
                                    predictionTimeSeconds;

            mover.TargetPosition = predictedPosition;
        }

        private void Awake()
        {
            mover = GetComponent<TargetMover>();
            player = FindAnyObjectByType<PlayerMovement>();
        }
    }
}