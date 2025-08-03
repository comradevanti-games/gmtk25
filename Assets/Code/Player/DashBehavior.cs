using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GMTK25
{
    public sealed class DashBehavior : MonoBehaviour
    {
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDurationSeconds;

        private RevolverDrumKeeper drumKeeper = null!;
        private PlayerMovement movement = null!;

        private TimeSpan DashDuration =>
            TimeSpan.FromSeconds(dashDurationSeconds);

        private void Dash()
        {
            drumKeeper.Spin();

            movement.SpeedOverride = movement.MovementDirection * dashSpeed;
            this.RunTask(async (ct) =>
            {
                await Task.Delay(DashDuration, ct);
                movement.SpeedOverride = null;
            });
        }

        private void OnDashInput()
        {
            Dash();
        }

        private void Awake()
        {
            Singletons.Require<InputHandler>().DashInput += OnDashInput;
            movement = GetComponent<PlayerMovement>();
            drumKeeper = FindAnyObjectByType<RevolverDrumKeeper>();
        }
    }
}