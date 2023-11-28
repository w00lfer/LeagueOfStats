using System.Collections.Immutable;

namespace LeagueOfStats.Infrastructure.Extensions;

public static class ImmutableDictionaryExtensions
{
    public static IEnumerable<TValue> GetMultiple<TKey, TValue>(this ImmutableDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keysToSelect)
    {
        foreach (var key in keysToSelect)
            if (dictionary.TryGetValue(key, out TValue value))
            {
                yield return value;
            }
    }
}