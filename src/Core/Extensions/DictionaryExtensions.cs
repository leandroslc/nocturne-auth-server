using System.Collections.Generic;

namespace Nocturne.Auth.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrReplace<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key,
            TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}
