namespace AdventOfCode;

using System.Numerics;

public static class CollectionExtensions
{
    public static IEnumerable<IList<T>> CartesianPower<T>(this IList<T> items, int positions)
    {
        // Start recursion with an empty LinkedList
        return Generate(positions, []);

        // Helper method for recursive generation
        IEnumerable<List<T>> Generate(int depth, LinkedList<T> current)
        {
            if (depth == 0)
            {
                yield return [..current]; // Yield a copy of the current LinkedList
                yield break;
            }

            foreach (var item in items)
            {
                current.AddFirst(item); // Add item to the beginning

                foreach (var result in Generate(depth - 1, current))
                {
                    yield return result;
                }

                current.RemoveFirst(); // Backtrack by removing the head
            }
        }
    }

    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        // base case:
        IEnumerable<IEnumerable<T>> result = new[] { Enumerable.Empty<T>() };
        return sequences.Aggregate(
            result,
            (current, s) =>
                from seq in current
                from item in s
                select seq.Concat(new[] { item }));
    }

    /// <summary>
    ///     Splits the input sequence into chunks of the specified size.
    ///     The last chunk may contain fewer elements if the total number of elements is not divisible by the chunk size.
    /// </summary>
    public static IEnumerable<List<T>> Chunk<T>(this IEnumerable<T> input, int size)
    {
        var chunk = new List<T>(size);

        foreach (var item in input)
        {
            chunk.Add(item);

            if (chunk.Count != size)
            {
                continue;
            }

            yield return chunk;
            chunk = new(size);
        }

        if (chunk.Count != 0)
        {
            yield return chunk;
        }
    }

    public static IEnumerable<Pair<T>> GetSymmetricPairs<T>(this IEnumerable<T> input)
    {
        var list = input.ToList();
        return
            from a in list
            from b in list
            select new Pair<T>(a, b);
    }

    public static IEnumerable<(T, T)> GetUnorderedUniquePairs<T>(this IEnumerable<T> input)
    {
        var list = input.ToList();
        for (var i = 0; i < list.Count; i++)
        {
            for (var j = i + 1; j < list.Count; j++)
            {
                yield return (list[i], list[j]);
            }
        }
    }

    public static bool IsEmpty<T>(this IEnumerable<T> input) => !input.Any();

    public static bool IsNotEmpty<T>(this IEnumerable<T> input) => input.Any();

    public static string Join<T>(this IEnumerable<T> values, string? separator = null) => string.Join(separator, values);

    public static IEnumerable<T> Pop<T>(this Stack<T> input, int count)
    {
        for (var i = 0; i < count; i++)
        {
            yield return input.Pop();
        }
    }

    public static void PushRange<T>(this Stack<T> input, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            input.Push(item);
        }
    }

    /// <summary>
    ///     Creates a sliding window of the specified size over the input sequence.
    ///     Each window is a consecutive sublist of elements, and windows overlap by size - 1 elements.
    /// </summary>
    public static IEnumerable<List<T>> Slide<T>(this IEnumerable<T> source, int size)
    {
        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "Size must be greater than zero.");
        }

        var window = new LinkedList<T>();
        foreach (var item in source)
        {
            window.AddLast(item);
            if (window.Count == size)
            {
                yield return [..window];
                window.RemoveFirst();
            }
        }
    }

    public static void SetOrIncrement<TKey, TValue>(this Dictionary<TKey, TValue> input, TKey key, TValue increment)
        where TKey : notnull
        where TValue : IAdditionOperators<TValue, TValue, TValue>, IAdditiveIdentity<TValue, TValue>
        => input[key] = (input.GetValueOrDefault(key) ?? TValue.AdditiveIdentity) + increment;

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> input, Predicate<T> separator)
    {
        List<T> group = [];

        foreach (var item in input)
        {
            if (separator(item))
            {
                if (group.Count != 0)
                {
                    yield return group;
                    group = [];
                }
            }
            else
            {
                group.Add(item);
            }
        }

        if (group.Count != 0)
        {
            yield return group;
        }
    }
}
