using System;
using UnityEngine;

namespace GMTK25
{
    public sealed class DashBehavior : MonoBehaviour
    {
        private RevolverDrumKeeper drumKeeper = null!;

        private void Dash()
        {
            drumKeeper.Spin();
        }

        private void OnDashInput()
        {
            Dash();
        }

        private void Awake()
        {
            Singletons.Require<InputHandler>().DashInput += OnDashInput;
            drumKeeper = FindAnyObjectByType<RevolverDrumKeeper>();
        }
    }
}