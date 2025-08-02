using UnityEngine;

namespace GMTK25
{
    public static class AlphaUtils
    {
        public static Color WithAlpha(this Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }
    }
}