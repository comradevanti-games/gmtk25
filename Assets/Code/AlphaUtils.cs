using UnityEngine;
using UnityEngine.UI;

namespace GMTK25
{
    public static class AlphaUtils
    {
        public static Color WithAlpha(this Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }

        public static void SetAlpha(this Image image, float a)
        {
            image.color = image.color.WithAlpha(a);
        }
    }
}