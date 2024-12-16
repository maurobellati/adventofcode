namespace AdventOfCode;

public static class MultiValueDictionaryExtensions
{
    public static TCollection Add<TKey, TValue, TCollection>(this IDictionary<TKey, TCollection> dictionary, TKey key, TValue value)
        where TKey : notnull
        where TCollection : class, ICollection<TValue>, new()
    {
        if (!dictionary.TryGetValue(key, out var collection))
        {
            collection = [];
            dictionary.Add(key, collection);
        }

        collection.Add(value);
        return collection;
    }
}
