namespace AdventOfCode.Y2022.Day18;

using Tool;

public class Y2022Day18 : Solver
{
    public object PartOne(List<string> input) =>
        input.Select(ParseCube)
            .SelectMany(cube => cube.Sides.ToList())
            .GroupBy(side => side)
            .Where(g => g.Count() == 1).Count();

    public object PartTwo(List<string> input) => 0;

    private static Cube ParseCube(string line)
    {
        var parts = line.Split(",", StringSplitOptions.TrimEntries).Select(decimal.Parse).ToList();
        return new(new(parts[0], parts[1], parts[2]));
    }

    private sealed record Point3D(decimal X, decimal Y, decimal Z)
    {
        public override string ToString() => $"({X}, {Y}, {Z})";
    }

    private sealed record Cube(Point3D Point)
    {
        public IEnumerable<Point3D> Sides
        {
            get
            {
                yield return Point with { X = Point.X - 0.5m };
                yield return Point with { X = Point.X + 0.5m };
                yield return Point with { Y = Point.Y - 0.5m };
                yield return Point with { Y = Point.Y + 0.5m };
                yield return Point with { Z = Point.Z - 0.5m };
                yield return Point with { Z = Point.Z + 0.5m };
            }
        }
    }
}
