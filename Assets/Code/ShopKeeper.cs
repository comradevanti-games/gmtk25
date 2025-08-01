using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GMTK25
{
    public sealed class ShopKeeper : MonoBehaviour
    {
        [SerializeField] private Vector2 shopLocation;
        [SerializeField] private float itemGap;

        private PickupSpawner pickupSpawner = null!;
        private BulletType[] allBulletTypes = Array.Empty<BulletType>();

        private readonly IList<GameObject> offeredPickups =
            new List<GameObject>();

        private void OpenShop(int itemCount)
        {
            var offers = Enumerable.Range(0, itemCount)
                .Select((_) => allBulletTypes.GetRandom()).ToArray();

            var leftX = shopLocation.x - offers.Length * itemGap / 2;
            for (var i = 0; i < itemCount; i++)
            {
                var x = leftX + i * itemGap;
                var pos = new Vector2(x, shopLocation.y);
                var pickup = pickupSpawner.SpawnPickup(
                    new PickupSpawner.Request(offers[i], pos));
                offeredPickups.Add(pickup);
            }
        }

        public void OnBreakStarted(int earlyWins)
        {
            // We always have at least 1 item for sale, but increase with
            // early wins.
            OpenShop(earlyWins + 1);
        }

        public void OnWaveStarted()
        {
            foreach (var pickup in offeredPickups)
                Destroy(pickup);

            offeredPickups.Clear();
        }

        private void Awake()
        {
            allBulletTypes = Resources.LoadAll<BulletType>("BulletTypes");
            pickupSpawner = Singletons.Require<PickupSpawner>();
        }
    }
}