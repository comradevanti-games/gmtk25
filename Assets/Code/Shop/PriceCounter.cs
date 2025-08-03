using TMPro;
using UnityEngine;

namespace GMTK25.Shop
{
    public sealed class PriceCounter : MonoBehaviour
    {
        private TMP_Text priceText = null!;
        private GameObject pickup = null!;
        private int remainingPrice;

        private void UnlockPickup()
        {
            pickup.GetComponent<Collider2D>().enabled = true;
            Destroy(gameObject);
        }

        private void UpdateRemainingPrice(int value)
        {
            remainingPrice = value;
            priceText.text = value.ToString();

            if (remainingPrice <= 0) UnlockPickup();
        }

        public void Init(GameObject pickup, int price)
        {
            this.pickup = pickup;
            pickup.GetComponent<Collider2D>().enabled = false;
            UpdateRemainingPrice(price);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var isBullet = other.gameObject.layer ==
                           LayerMask.NameToLayer("Projectile");
            if (!isBullet) return;

            UpdateRemainingPrice(remainingPrice - 1);
        }

        private void Awake()
        {
            priceText = GetComponentInChildren<TMP_Text>();
        }
    }
}