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

            public sealed record GoTo(Vector2 Target) : EnemyAction;
        }

        [SerializeField] private float minActionDurationSeconds;
        [SerializeField] private float maxActionDurationSeconds;

        private TargetMover targetMover = null!;
        private TargetFollower targetFollower = null!;
        private PlayerPredictor playerPredictor = null!;
        private StageLocationFinder stageLocationFinder = null!;

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

            if (action is EnemyAction.GoTo(var target))
                targetMover.TargetPosition = target;
        }

        private void PickAction()
        {
            var duration = PickActionDuration();
            var r = Random.Range(0f, 100);
            EnemyAction action = r switch
            {
                < 15 => new EnemyAction.Idle(),
                < 30 => new EnemyAction.GoTo(stageLocationFinder
                    .PickRandomFreeLocation()),
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
            targetMover = GetComponent<TargetMover>();
            targetFollower = GetComponent<TargetFollower>();
            playerPredictor = GetComponent<PlayerPredictor>();
            stageLocationFinder = Singletons.Require<StageLocationFinder>();
        }
    }
}