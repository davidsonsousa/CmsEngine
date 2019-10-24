using System;
using System.Collections.Generic;
using System.Linq;

namespace CmsEngine.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, Func<T, TKey> getKeyFunc)
        {
            if (other == null)
            {
                return items;
            }

            return items
                .GroupJoin(other, getKeyFunc, getKeyFunc, (item, tempItems) => new { item, tempItems })
                .SelectMany(t => t.tempItems.DefaultIfEmpty(), (t, temp) => new { t, temp })
                .Where(t => t.temp == null || t.temp.Equals(default(T)))
                .Select(t => t.t.item);
        }
    }
}
