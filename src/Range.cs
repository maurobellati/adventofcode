namespace AdventOfCode;

public record Range(long From, long To)
{
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
    ///     Returns true if the two ranges overlap each other in at least one value
    /// </summary>
    public static bool Overlaps(this Range range, Range other) =>
        other.To >= range.From && other.From <= range.To;

    public static Range Shift(this Range range, long shift) =>
        new(range.From + shift, range.To + shift);
}
