using System.Collections.Generic;

namespace GMTK25
{
    public static class CollectionExt
    {
        public static int RandomIndex<T>(this IReadOnlyCollection<T> collection)
        {
            return UnityEngine.Random.Range(0, collection.Count);
        }

        public static int RandomIndex<T>(this ICollection<T> collection)
        {
            return UnityEngine.Random.Range(0, collection.Count);
        }

        public static T GetRandom<T>(this IReadOnlyList<T> list)
        {
            var index = list.RandomIndex();
            return list[index];
        }

        public static T RemoveRandom<T>(this IList<T> list)
        {
            var index = list.RandomIndex();
            var item = list[index];
            list.RemoveAt(index);
            return item;
        }
    }
}