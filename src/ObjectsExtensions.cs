namespace AdventOfCode;

public static class ObjectsExtensions
{
    public static void AddRange<T>(this ref HashCode hashCode, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            hashCode.Add(item);
        }
    }

    public static bool Between<T>(this T value, T min, T max) where T : IComparable<T>
    {
        if (min.CompareTo(max) <= 0)
        {
            return min.CompareTo(value) <= 0 && value.CompareTo(max) <= 0;
        }

        return Between(value, max, min);
    }

    /// <summary>
    ///     Same as CompareTo but returns null instead of 0 if both items are equal.
    /// </summary>
    /// <typeparam name="T">IComparable type.</typeparam>
    /// <param name="this">This instance.</param>
    /// <param name="other">The other instance.</param>
    /// <returns>Lexical relation between this and the other instance or null if both are equal.</returns>
    public static int? NullableCompareTo<T>(this T @this, T other) where T : IComparable
    {
        var result = @this.CompareTo(other);
        return result != 0 ? result : null;
    }
}
