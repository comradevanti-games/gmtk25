using UnityEngine;

namespace GMTK25
{
    [CreateAssetMenu(fileName = "New Color-type",
        menuName = "GMTK25/Color-type")]
    public sealed class ColorType : ScriptableObject
    {
        [SerializeField] private Color color;

        public Color Color => color;
    }
}