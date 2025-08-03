using System.Collections.Generic;
using System.Collections.Immutable;
using GMTK25.Bullets;
using UnityEngine;

namespace GMTK25.UI
{
    public sealed class DrumDisplay : MonoBehaviour
    {
        private readonly ImmutableArray<float> ts =
            new[] { 0, 0.1f, 0.25f, 0.4f, 0.6f, 0.75f, 0.9f }
                .ToImmutableArray();

        private RectTransform container = null!;

        private IList<BulletDisplay> activeDisplays = new List<BulletDisplay>();

        private IList<BulletDisplay> inactiveDisplays =
            new List<BulletDisplay>();

        private float Width => container.sizeDelta.x - 50;

        private float Height => container.sizeDelta.y - 50;

        private Vector2 PositionForT(float t)
        {
            var loopT = t * 2 * Mathf.PI;
            var x = Mathf.Sin(loopT) / 2f * Width;
            var y = Mathf.Sin(loopT) * Mathf.Cos(loopT) * Height;

            return new Vector2(x, y);
        }

        private void UpdateTargets()
        {
            for (var i = 0; i < activeDisplays.Count; i++)
            {
                var targetT = ts[i];
                var display = activeDisplays[i];
                display.TargetT = targetT;
            }
        }

        public void Push(BulletType bulletType)
        {
            var display = inactiveDisplays[0];
            inactiveDisplays.RemoveAt(0);
            activeDisplays.Add(display);

            display.Display(bulletType);
            UpdateTargets();
        }

        public void RemoveFirst()
        {
            var display = activeDisplays[0];
            display.Display(null);
            activeDisplays.RemoveAt(0);
            inactiveDisplays.Add(display);

            UpdateTargets();
        }

        public void Cycle()
        {
            var display = activeDisplays[0];
            activeDisplays.RemoveAt(0);
            activeDisplays.Add(display);

            UpdateTargets();
        }

        private static float MoveDownTo(float f, float target, float maxDelta)
        {
            return Mathf.Repeat(
                Mathf.MoveTowards(f < target ? f + 1 : f, target, maxDelta),
                1f);
        }

        private void Update()
        {
            var speed = Time.deltaTime * 2;
            foreach (var display in activeDisplays)
            {
                display.T = MoveDownTo(display.T, display.TargetT, speed);
                display.Position = PositionForT(display.T);
            }
        }

        private void Awake()
        {
            container = GetComponent<RectTransform>();
            foreach (var display in gameObject
                         .GetComponentsInChildren<BulletDisplay>())
                inactiveDisplays.Add(display);
        }
    }
}