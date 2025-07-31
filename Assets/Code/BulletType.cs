using UnityEngine;

namespace GMTK25
{
    [CreateAssetMenu(fileName = "New Bullet-type", menuName = "GTMK25/Bullet-type")]
    public sealed class BulletType : ScriptableObject
    {
        [SerializeField] private GameObject prefab = null!;
        [SerializeField] private Sprite backSprite = null!;
        
        public GameObject Prefab => prefab;
        
        public Sprite BackSprite => backSprite;
    }
}