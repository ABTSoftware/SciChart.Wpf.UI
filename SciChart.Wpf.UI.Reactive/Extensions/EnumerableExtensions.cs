using System;
using System.Collections.Generic;
using System.Linq;

namespace SciChart.Wpf.UI.Reactive.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool In<T>(this T value, params T[] collection)
        {
            return collection != null && collection.Contains(value);
        }

        public static bool In<T>(this T value, IEnumerable<T> collection)
        {
            return collection != null && collection.Contains(value);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static void ForEachDo<T>(this IEnumerable<T> collection, Action<T> operation)
        {
            if (collection == null) return;

            foreach (var item in collection)
            {
                operation(item);
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            int index = -1;
            if (source == null) return index;
            
            foreach (var item in source)
            {
                ++index;
                if (predicate(item)) 
                    return index;
            }

            return -1;
        }
    }
}