using UnityEngine;

namespace GMTK25.Enemies
{
    [CreateAssetMenu(fileName = "New EnemyColor",
        menuName = "GMTK25/Enemy-color")]
    public sealed class EnemyColor : ScriptableObject
    {
        [SerializeField] private Color color;

        public Color Color => color;
    }
}