using System.Collections.Immutable;
using UnityEngine;

namespace GMTK25.UI
{
    public sealed class DrumDisplay : MonoBehaviour
    {
        private readonly ImmutableArray<float> ts =
            new[] { 0, 0.1f, 0.25f, 0.4f, 0.6f, 0.75f, 0.9f }
                .ToImmutableArray();

        private RectTransform container = null!;

        private ImmutableArray<BulletDisplay> bulletDisplays =
            ImmutableArray<BulletDisplay>.Empty;

        private int firstIndex;
        private int nextIndex;

        private float Width => container.sizeDelta.x - 50;

        private float Height => container.sizeDelta.y - 50;

        private Vector2 PositionForT(float t)
        {
            var loopT = t * 2 * Mathf.PI;
            var x = Mathf.Sin(loopT) / 2f * Width;
            var y = Mathf.Sin(loopT) * Mathf.Cos(loopT) * Height;

            return new Vector2(x, y);
        }

        private int Loop(int i)
        {
            return i switch
            {
                > 6 => Loop(i - 7),
                < 0 => Loop(i + 7),
                _ => i
            };
        }

        private int IncLooping(int i)
        {
            return Loop(i + 1);
        }

        private void UpdateTargets()
        {
            for (var i = 0; i < 7; i++)
            {
                var targetT = ts[i];
                var display = bulletDisplays[Loop(firstIndex + i)];
                display.TargetT = targetT;
            }
        }

        public void Push(BulletType bulletType)
        {
            bulletDisplays[nextIndex].Display(bulletType);

            nextIndex = IncLooping(nextIndex);
            UpdateTargets();
        }

        public void RemoveFirst()
        {
            bulletDisplays[firstIndex].Display(null);

            firstIndex = IncLooping(firstIndex);
            UpdateTargets();
        }

        private float MoveDown(float f, float target, float maxDelta)
        {
            if (f < target) f += 1;
            var diff = target - f;
            Debug.Assert(diff <= 0);

            if (-diff < maxDelta) return target;
            return f - maxDelta;
        }

        private void Update()
        {
            for (var i = 0; i < 7; i++)
            {
                var display = bulletDisplays[i];

                display.T = MoveDown(display.T, display.TargetT,
                    Time.deltaTime);
                display.Position = PositionForT(display.T);
            }
        }

        private void Awake()
        {
            container = GetComponent<RectTransform>();
            bulletDisplays = gameObject.GetComponentsInChildren<BulletDisplay>()
                .ToImmutableArray();
        }
    }
}