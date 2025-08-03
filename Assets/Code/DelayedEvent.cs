using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK25
{
    public sealed class DelayedEvent : MonoBehaviour
    {
        public UnityEvent delayReached = new UnityEvent();

        [SerializeField] private float delaySeconds = 0f;

        private void Start()
        {
            this.RunTask(async (ct) =>
            {
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds), ct);

                delayReached.Invoke();
                Destroy(this);
            });
        }
    }
}