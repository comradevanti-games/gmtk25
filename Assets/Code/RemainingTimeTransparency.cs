using System;
using UnityEngine;

namespace GMTK25
{
    [RequireComponent(typeof(DelayedEvent))]
    public sealed class RemainingTimeTransparency : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = null!;

        private DelayedEvent delayedEvent = null!;


        private void OnRemainingDelayChanged(TimeSpan remainingTime)
        {
            var t = Mathf.InverseLerp(0f,
                (float)delayedEvent.Delay.TotalSeconds,
                (float)remainingTime.TotalSeconds);
            spriteRenderer.SetAlpha(t);
        }

        private void Awake()
        {
            delayedEvent = GetComponent<DelayedEvent>();

            delayedEvent.remainingDelayChanged.AddListener(
                OnRemainingDelayChanged);
        }
    }
}