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

        public Vector2 Position
        {
            set => rectTransform.anchoredPosition = value;
        }

        public void Display(BulletType? bulletType)
        {
            image.enabled = bulletType != null;
            if (bulletType != null) image.sprite = bulletType.BackSprite;
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
        }
    }
}