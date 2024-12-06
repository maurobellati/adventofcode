namespace AdventOfCode;

using System.Numerics;

public sealed record Direction(string Label, Complex Vector)
{
    public static readonly Direction N = new("N", Complex.ImaginaryOne);
    public static readonly Direction S = new("S", -Complex.ImaginaryOne);
    public static readonly Direction E = new("E", 1);
    public static readonly Direction W = new("W", -1);

    public static readonly Direction NE = new("NE", Complex.ImaginaryOne + 1);
    public static readonly Direction NW = new("NW", Complex.ImaginaryOne - 1);
    public static readonly Direction SE = new("SE", -Complex.ImaginaryOne + 1);
    public static readonly Direction SW = new("SW", -Complex.ImaginaryOne - 1);

    public static IEnumerable<Direction> All => [N, NE, E, SE, S, SW, W, NW];

    public static IEnumerable<Direction> NSEW => [N, S, E, W];

    public override string ToString() => Label;
}
