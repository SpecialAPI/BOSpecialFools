using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialFools.Tools
{
    public static class LocalCollectionExtensions
    {
        public static T GetAndRemoveRandomElement<T>(this IList<T> list)
        {
            if (list.IsReadOnly)
                return default;

            var idx = Random.Range(0, list.Count);
            var elem = list[idx];
            list.RemoveAt(idx);

            return elem;
        }
    }
}
