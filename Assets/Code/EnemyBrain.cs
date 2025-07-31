using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GMTK25
{
    [RequireComponent(typeof(TargetFollower))]
    public sealed class EnemyBrain : MonoBehaviour
    {
        private abstract record EnemyAction
        {
            public sealed record Idle : EnemyAction;

            public sealed record FollowPlayer : EnemyAction;
        }

        [SerializeField] private float minActionDurationSeconds;
        [SerializeField] private float maxActionDurationSeconds;

        private TargetFollower targetFollower = null!;

        private TimeSpan PickActionDuration()
        {
            return TimeSpan.FromSeconds(Random.Range(minActionDurationSeconds,
                maxActionDurationSeconds));
        }

        private void ExecuteAction(EnemyAction action)
        {
            if (!targetFollower) return;
            targetFollower.enabled = action switch
            {
                EnemyAction.Idle => false,
                EnemyAction.FollowPlayer => true,
                _ => throw new ArgumentOutOfRangeException(nameof(action),
                    action, null)
            };
        }

        private void PickAction()
        {
            var duration = PickActionDuration();
            EnemyAction action = Random.Range(0, 1f) < 0.25
                ? new EnemyAction.Idle()
                : new EnemyAction.FollowPlayer();

            Debug.Log(
                $"Executing action {action.GetType().Name} for {duration} 🫡",
                this);
            ExecuteAction(action);

            this.RunTask(async () =>
            {
                await Task.Delay(duration, destroyCancellationToken);
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
        }
    }
}