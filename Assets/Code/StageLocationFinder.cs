using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GMTK25
{
    public sealed class StageLocationFinder : MonoBehaviour
    {
        [SerializeField] private float width;
        [SerializeField] private float height;
        [SerializeField] private Transform player = null!;
        [SerializeField] private float minFreeDistance;

        /// <summary>
        /// Pick a random location anywhere in the stage.
        /// </summary>
        public Vector2 PickRandomLocation()
        {
            return new Vector2(
                Random.Range(-width / 2, width / 2),
                Random.Range(-height / 2, height / 2)
            );
        }

        /// <summary>
        /// Pick a random location in the stage which is free (No other
        /// entity is nearby).
        /// </summary>
        public Vector2 PickRandomFreeLocation()
        {
            var count = 0;
            while (count < 1000)
            {
                var location = PickRandomLocation();

                if (Vector2.Distance(location, player.position) >
                    minFreeDistance)
                    return location;

                count++;
            }

            throw new Exception("Did not find a free location");
        }
    }
}