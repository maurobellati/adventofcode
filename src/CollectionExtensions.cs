namespace AdventOfCode;

public static class CollectionExtensions
{
    public static IEnumerable<(T, T)> CartesianProduct<T>(this IEnumerable<T> input)
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

    public static bool IsEmpty<T>(this IEnumerable<T> input) => !input.Any();

    public static bool IsNotEmpty<T>(this IEnumerable<T> input) => input.Any();

    public static string Join<T>(this IEnumerable<T> values, string? separator = null) => string.Join(separator, values);

    public static void Pop<T>(this Stack<T> input, int count)
    {
        for (var i = 0; i < count; i++)
        {
            input.Pop();
        }
    }

    public static void PushRange<T>(this Stack<T> input, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            input.Push(item);
        }
    }

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
