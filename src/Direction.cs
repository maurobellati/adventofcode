namespace AdventOfCode;

public sealed record Direction(string Label)
{
    public static readonly Direction N = new("N");
    public static readonly Direction S = new("S");
    public static readonly Direction E = new("E");
    public static readonly Direction W = new("W");

    public static IEnumerable<Direction> GetAll() => [N, S, E, W];
}
