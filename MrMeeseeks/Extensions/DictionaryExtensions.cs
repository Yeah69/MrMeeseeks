using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MrMeeseeks.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddToKey<TKey, TItem>(this IDictionary<TKey, ICollection<TItem>> @this, TKey key, TItem item)
        {
            if (!@this.TryGetValue(key, out var list) || list is null)
            {
                @this[key] = new List<TItem> { item };
            }
            else
            {
                list.Add(item);
            }
        }
        public static void AddToKey<TKey, TItem>(this IDictionary<TKey, IList<TItem>> @this, TKey key, TItem item)
        {
            if (!@this.TryGetValue(key, out var list) || list is null)
            {
                @this[key] = new List<TItem> { item };
            }
            else
            {
                list.Add(item);
            }
        }

        public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> @this) =>
            new ReadOnlyDictionary<TKey, TValue>(@this);
    }
}
