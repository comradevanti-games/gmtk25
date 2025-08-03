using System;
using GMTK25.Bullets;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK25.UI
{
    public sealed class BulletDisplay : MonoBehaviour
    {
        private RectTransform rectTransform = null!;
        private Image image = null!;

        public float T { get; set; }

        public float TargetT { get; set; }

        private void UpdateAlpha()
        {
            var distToBack = Mathf.Min(Mathf.Abs(T - 0.5f), 0.2f);
            var alpha = Mathf.InverseLerp(0, 0.2f, distToBack);
            image.SetAlpha(alpha);
        }

        public Vector2 Position
        {
            set
            {
                rectTransform.anchoredPosition = value;
                UpdateAlpha();
            }
        }

        public void Display(BulletType? bulletType)
        {
            image.enabled = bulletType != null;

            if (bulletType != null)
            {
                image.sprite = bulletType.BackSprite;
                image.color = bulletType.ColorType != null
                    ? bulletType.ColorType.Color
                    : Color.white;
            }
            else
            {
                image.sprite = null!;
            }
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
        }
    }
}