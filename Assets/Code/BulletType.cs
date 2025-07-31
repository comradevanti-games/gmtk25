using UnityEngine;

namespace GMTK25 {

    [CreateAssetMenu(fileName = "New Bullet-type", menuName = "GMTK25/Bullet-type")]
    public sealed class BulletType : ScriptableObject {

        [SerializeField] private GameObject prefab = null!;
        [SerializeField] private Sprite backSprite = null!;
        [SerializeField] private float initialSpeed = 0;

        public GameObject Prefab => prefab;

        public Sprite BackSprite => backSprite;

        public float InitialSpeed => initialSpeed;

    }

}