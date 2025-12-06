namespace AdventOfCode;

public record Range(long From, long To)
{
    public long End => To;

    public long Start => From;

    public static Range FromStartAndLength(long start, long length) => new(start, start + length - 1);

    public override string ToString() => $"[{From}, {To}]";
}

public static class RangeExtensions
{
    /// <summary>
    ///     Returns true if the input is within the range
    /// </summary>
    public static bool Contains(this Range range, long input) =>
        input.Between(range.From, range.To);

    /// <summary>
    ///     Returns true if the range provided is fully contained within this range
    /// </summary>
    public static bool Includes(this Range range, Range other) =>
        range.From <= other.From && range.To >= other.To;

    /// <summary>
    ///     Returns a new range that is the intersection of the two ranges
    /// </summary>
    public static Range Intersect(this Range range, Range other) =>
        new(Math.Max(range.From, other.From), Math.Min(range.To, other.To));

    /// <summary>
    ///     Return a new range that is the merge of the two ranges if they overlap.
    ///     It throws if they do not overlap.
    /// </summary>
    /// <returns></returns>
    public static Range Merge(this Range range, Range other)
    {
        if (range.Includes(other))
        {
            return range;
        }

        if (other.Includes(range))
        {
            return other;
        }

        if (range.Overlaps(other))
        {
            return new(Math.Min(range.From, other.From), Math.Max(range.To, other.To));
        }

        throw new InvalidOperationException("Ranges do not overlap.");
    }

    /// <summary>
    ///     Returns true if the two ranges overlap each other in at least one value
    /// </summary>
    public static bool Overlaps(this Range range, Range other) =>
        other.To >= range.From && other.From <= range.To;

    public static Range Shift(this Range range, long shift) =>
        new(range.From + shift, range.To + shift);
}
