namespace AdventOfCode;

public sealed record Direction(string Label)
{
    public static readonly Direction N = new("N");
    public static readonly Direction S = new("S");
    public static readonly Direction E = new("E");
    public static readonly Direction W = new("W");

    public static readonly Direction NE = new("NE");
    public static readonly Direction NW = new("NW");
    public static readonly Direction SE = new("SE");
    public static readonly Direction SW = new("SW");

    public static IEnumerable<Direction> NSEW => [N, S, E, W];
    public static IEnumerable<Direction> All => [N, NE, E, SE, S, SW, W, NW];
}
