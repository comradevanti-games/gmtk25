using System;
using System.Collections.Generic;
using System.Linq;
using GMTK25.Bullets;
using UnityEngine;

namespace GMTK25.Shop {

    public sealed class ShopKeeper : MonoBehaviour {

        [SerializeField] private Vector2 shopLocation;
        [SerializeField] private float itemGap;
        [SerializeField] private GameObject priceCounterPrefab = null!;
        [SerializeField] private BulletType normalBulletType = null!;

        private PickupSpawner pickupSpawner = null!;
        private BulletType[] allBulletTypes = Array.Empty<BulletType>();

        private readonly IList<GameObject> shopObjects = new List<GameObject>();

        private void OpenShop(int itemCount) {
            var offers = Enumerable.Range(0, itemCount)
                .Select((_) => allBulletTypes.GetRandom()).ToArray();

            var leftX = shopLocation.x - offers.Length * itemGap / 2;
            var free = pickupSpawner.SpawnPickup(new PickupSpawner.Request(normalBulletType,
                new Vector2(leftX, shopLocation.y), normalBulletType.ColorType));
            var freeSign = Instantiate(priceCounterPrefab, new Vector2(leftX, shopLocation.y) + Vector2.up * 2,
                Quaternion.identity);
            freeSign.GetComponent<PriceCounter>().Init(free, normalBulletType.Price);

            shopObjects.Add(free);
            shopObjects.Add(freeSign);

            for (var i = 1; i < itemCount + 1; i++) {
                var x = leftX + i * itemGap;
                var pos = new Vector2(x, shopLocation.y);
                var pickup = pickupSpawner.SpawnPickup(new PickupSpawner.Request(offers[i - 1], pos,
                    offers[i - 1].ColorType));
                shopObjects.Add(pickup);

                var counter = Instantiate(priceCounterPrefab, pos + Vector2.up * 2, Quaternion.identity);
                shopObjects.Add(counter);

                counter.GetComponent<PriceCounter>().Init(pickup, offers[i - 1].Price);
            }
        }

        public void OnBreakStarted(int earlyWins) {
            // We always have at least 1 item for sale, but increase with
            // early wins.
            OpenShop(earlyWins + 1);
        }

        public void OnWaveStarted() {
            foreach (var it in shopObjects) Destroy(it);
            shopObjects.Clear();
        }

        private void Awake() {
            allBulletTypes = Resources.LoadAll<BulletType>("BulletTypes");
            pickupSpawner = Singletons.Require<PickupSpawner>();
        }

    }

}