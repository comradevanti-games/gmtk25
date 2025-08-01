using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GMTK25
{
    public static class MonoExt
    {
        public static T GetOrAdd<T>(this GameObject g) where T : Component
        {
            var component = g.GetComponent<T>();
            return component ? component : g.AddComponent<T>();
        }

        public static async void RunTask(this MonoBehaviour mono,
            Func<CancellationToken, Task> startTask)
        {
            try
            {
                if (!mono) return;
                var token = mono.destroyCancellationToken;
                await startTask(token);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                Debug.LogException(e, mono);
            }
        }
    }
}