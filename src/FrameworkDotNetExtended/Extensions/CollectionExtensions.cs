using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkDotNetExtended.Extensions
{
    public static class CollectionExtensions
    {
        public static void Add<T>(this ICollection<T> source, ICollection<T> enumerable)
        {
            foreach (var item in enumerable)
            {
                source.Add(item);
            }
        }

        public static string[] ToLower(this string[] source)
        {
            if (source != null && source.Any())
            {
                return source.Select(item => (item != null ? item.ToLower() : item)).ToArray();
            }
            return source;
        }

        public static void Remove<T>(this ICollection<T> source, ICollection<T> enumerable)
        {
            foreach (var item in enumerable)
            {
                source.Remove(item);
            }
        }

        public static void Remove<T>(this ICollection<T> source, Func<T, bool> predicate)
        {
            for (int i = 0; i < source.Count; i++)
            {
                T entity = source.ElementAt(i);

                if (predicate(entity))
                {
                    source.Remove(entity);
                    i--;
                }
            }
        }
    }
}
