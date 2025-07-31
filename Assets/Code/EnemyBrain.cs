using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GMTK25
{
    [RequireComponent(typeof(TargetFollower))]
    public sealed class EnemyBrain : MonoBehaviour
    {
        [SerializeField] private float thinkingTimeSeconds;

        private TargetFollower targetFollower = null!;

        private TimeSpan ThinkingTime =>
            TimeSpan.FromSeconds(thinkingTimeSeconds);

        private async void PickAction()
        {
            try
            {
                Debug.Log("Thinking about next action 🤔", this);
                await Task.Delay(ThinkingTime, destroyCancellationToken);

                // For now, we just always follow the player
                targetFollower.enabled = true;

                PickAction();
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
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