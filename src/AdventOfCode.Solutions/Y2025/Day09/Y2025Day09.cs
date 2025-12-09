namespace AdventOfCode.Y2025.Day09;

using Tool;
using Point2D = (long X, long Y);

public class Y2025Day09 : Solver
{
    public object PartOne(List<string> input) =>
        ParsePoints(input)
            .GetUnorderedUniquePairs()
            .Select(pair => CalculateArea(pair.Item1, pair.Item2))
            .Max();

    private static IEnumerable<Point2D> ParsePoints(List<string> input) =>
        input.Select(line => line.ExtractLongs().ToList())
            .Select(ints => new Point2D(ints[0], ints[1]));

    private static long CalculateArea(Point2D p1, Point2D p2)
    {
        var width = Math.Abs(p2.X - p1.X) + 1;
        var height = Math.Abs(p2.Y - p1.Y) + 1;
        return width * height;
    }

    public object PartTwo(List<string> input) => 0;
}
