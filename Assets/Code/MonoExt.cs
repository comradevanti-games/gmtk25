using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GMTK25
{
    public static class MonoExt
    {
        public static async void RunTask(this MonoBehaviour mono,
            Func<Task> startTask)
        {
            try
            {
                await startTask();
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