using System.Collections.Generic;

namespace GMTK25
{
    public static class CollectionExt
    {

        public static int RandomIndex<T>(this ICollection<T> collection)
        {
            return UnityEngine.Random.Range(0, collection.Count);
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