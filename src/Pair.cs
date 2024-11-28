namespace AdventOfCode;

using System.Collections;

public record Pair<T>(T A, T B) : IEnumerable<T>
{
    public T this[int index] => index switch
    {
        0 => A,
        1 => B,
        _ => throw new ArgumentOutOfRangeException(nameof(index))
    };

    public IEnumerator<T> GetEnumerator()
    {
        yield return A;
        yield return B;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class PairExtensions
{
    public static bool Contains<T>(this Pair<T> pair, T item)
        => EqualityComparer<T>.Default.Equals(pair.A, item) || EqualityComparer<T>.Default.Equals(pair.B, item);

    public static T Other<T>(this Pair<T> pair, T item)
        => EqualityComparer<T>.Default.Equals(item, pair.A) ? pair.B :
            EqualityComparer<T>.Default.Equals(item, pair.B) ? pair.A :
            throw new ArgumentOutOfRangeException(nameof(item));

    public static Pair<T> Swap<T>(this Pair<T> pair)
        => new(pair.B, pair.A);
}
