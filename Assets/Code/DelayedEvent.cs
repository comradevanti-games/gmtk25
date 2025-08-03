using System;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK25
{
    public sealed class DelayedEvent : MonoBehaviour
    {
        public UnityEvent delayReached = new UnityEvent();

        public UnityEvent<TimeSpan> remainingDelayChanged =
            new UnityEvent<TimeSpan>();

        [SerializeField] private float delaySeconds;

        public TimeSpan Delay => TimeSpan.FromSeconds(delaySeconds);

        private void Start()
        {
            this.RunTask(async (ct) =>
            {
                await CustomTask.DelayWithUpdate(Delay,
                    passedTime =>
                        remainingDelayChanged.Invoke(Delay - passedTime), ct);

                delayReached.Invoke();
                Destroy(this);
            });
        }
    }
}