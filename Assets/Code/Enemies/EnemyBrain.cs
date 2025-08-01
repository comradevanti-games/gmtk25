using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GMTK25.Enemies
{
    [RequireComponent(typeof(TargetFollower))]
    public sealed class EnemyBrain : MonoBehaviour
    {
        private abstract record EnemyAction
        {
            public sealed record Idle : EnemyAction;

            public sealed record FollowPlayer : EnemyAction;

            public sealed record PredictPlayer : EnemyAction;
        }

        [SerializeField] private float minActionDurationSeconds;
        [SerializeField] private float maxActionDurationSeconds;

        private TargetFollower targetFollower = null!;
        private PlayerPredictor playerPredictor = null!;

        private TimeSpan PickActionDuration()
        {
            return TimeSpan.FromSeconds(Random.Range(minActionDurationSeconds,
                maxActionDurationSeconds));
        }

        private void ExecuteAction(EnemyAction action)
        {
            if (!targetFollower || !playerPredictor) return;

            targetFollower.enabled = action is EnemyAction.FollowPlayer;
            playerPredictor.enabled = action is EnemyAction.PredictPlayer;
        }

        private void PickAction()
        {
            var duration = PickActionDuration();
            var r = Random.Range(0f, 100);
            EnemyAction action = r switch
            {
                < 20 => new EnemyAction.Idle(),
                < 60 => new EnemyAction.PredictPlayer(),
                _ => new EnemyAction.FollowPlayer()
            };
            
            ExecuteAction(action);

            this.RunTask(async (ct) =>
            {
                await Task.Delay(duration, ct);
                PickAction();
            });
        }

        private void Start()
        {
            PickAction();
        }

        private void Awake()
        {
            targetFollower = GetComponent<TargetFollower>();
            playerPredictor = GetComponent<PlayerPredictor>();
        }
    }
}