using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GMTK25
{
    public sealed class DashBehavior : MonoBehaviour
    {
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDurationSeconds;
        [SerializeField] private float dashCooldownSeconds;

        private RevolverDrumKeeper drumKeeper = null!;
        private PlayerMovement movement = null!;
        private bool isCoolingDown = false;

        private TimeSpan DashDuration =>
            TimeSpan.FromSeconds(dashDurationSeconds);

        private TimeSpan DashCooldown =>
            TimeSpan.FromSeconds(dashCooldownSeconds);

        private void Dash()
        {
            drumKeeper.Spin();

            movement.SpeedOverride = movement.MovementDirection * dashSpeed;
            this.RunTask(async (ct) =>
            {
                await Task.Delay(DashDuration, ct);
                movement.SpeedOverride = null;
            });

            isCoolingDown = true;
            this.RunTask(async (ct) =>
            {
                await Task.Delay(DashCooldown, ct);
                isCoolingDown = false;
            });
        }

        private void OnDashInput()
        {
            // Prevent dash from standstill
            if (movement.MovementDirection == Vector2.zero) return;

            if (isCoolingDown) return;

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