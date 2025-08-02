using UnityEngine;

namespace GMTK25.Bullets {

    [CreateAssetMenu(fileName = "New Bullet-type",
        menuName = "GMTK25/Bullet-type")]
    public sealed class BulletType : ScriptableObject {

        [SerializeField] private GameObject prefab = null!;
        [SerializeField] private Sprite backSprite = null!;
        [SerializeField] private float initialSpeed = 0;
        [SerializeField] private int price;
        [SerializeField] private Sprite pickupSprite = null!;
        [SerializeField] private ColorType colorType = null!;

        public GameObject Prefab => prefab;

        public Sprite BackSprite => backSprite;

        public float InitialSpeed => initialSpeed;

        public int Price => price;

        public Sprite PickupSprite => pickupSprite;

        public ColorType ColorType => colorType;

    }

}