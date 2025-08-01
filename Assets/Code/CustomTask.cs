using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GMTK25
{
    public static class CustomTask
    {
        public static async Task CancellableDelay(TimeSpan duration,
            Func<bool> shouldContinue, CancellationToken ct)
        {
            var passedTime = TimeSpan.Zero;

            while (passedTime < duration)
            {
                ct.ThrowIfCancellationRequested();
                await Task.Yield();

                if (!shouldContinue()) return;

                passedTime += TimeSpan.FromSeconds(Time.deltaTime);
            }
        }
    }
}