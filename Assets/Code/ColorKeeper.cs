using System;
using UnityEngine;

namespace GMTK25
{
    public sealed class ColorKeeper : MonoBehaviour
    {
        public ColorType ColorType { get; set; } = null!;
    }

    public static class ColorTypeExt
    {
        public static ColorType GetColorType(this GameObject g)
        {
            if (g.GetComponent<ColorKeeper>() is { } colorKeeper)
                return colorKeeper.ColorType;

            throw new Exception($"{g} does not have a {nameof(ColorKeeper)}");
        }

        public static void SetColorType(this GameObject g, ColorType colorType)
        {
            g.GetOrAdd<ColorKeeper>().ColorType = colorType;
        }
    }
}