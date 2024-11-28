namespace AdventOfCode;

public sealed record Rotation(string Label)
{
    public static readonly Rotation Clockwise = new("Clockwise");
    public static readonly Rotation CounterClockwise = new("CounterClockwise");
}
