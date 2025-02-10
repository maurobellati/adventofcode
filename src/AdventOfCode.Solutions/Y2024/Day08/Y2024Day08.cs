namespace AdventOfCode.Y2024.Day08;

using Tool;

public class Y2024Day08 : Solver
{
    public object PartOne(List<string> input) => Solve(input, GetAntinodesPartOne);

    public object PartTwo(List<string> input) => Solve(input, GetAntinodesPartTwo);

    private static IEnumerable<Cell> GetAntinodes(ICollection<Cell> cells, Box box, Func<Pair<Cell>, Box, IEnumerable<Cell>> antinodesGenerator) =>
        cells.GetSymmetricPairs().Where(p => p.A != p.B).SelectMany(pair => antinodesGenerator(pair, box));

    private static IEnumerable<Cell> GetAntinodesPartOne(Pair<Cell> pair, Box box) => new[] { pair.B + (pair.B - pair.A) }.Where(box.Contains);

    private static IEnumerable<Cell> GetAntinodesPartTwo(Pair<Cell> pair, Box box)
    {
        var step = pair.B - pair.A;
        var next = pair.B;
        while (box.Contains(next))
        {
            yield return next;
            next += step;
        }
    }

    private static int Solve(List<string> input, Func<Pair<Cell>, Box, IEnumerable<Cell>> antinodesGenerator)
    {
        var grid = GridFactory.Create(input);
        return grid.Entries
            .Where(x => x.Value != '.')
            .GroupBy(entry => entry.Value, entry => entry.Cell)
            .SelectMany(cells => GetAntinodes(cells.ToList(), grid, antinodesGenerator))
            .Distinct()
            .Count();
    }
}
