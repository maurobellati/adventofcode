namespace AdventOfCode.Y2024.Day08;

using Tool;

public class Y2024Day08 : Solver
{
    public object PartOne(List<string> input) => Solve(input, GetAntinodesPartOne);

    public object PartTwo(List<string> input) => Solve(input, GetAntinodesPartTwo);

    private static IEnumerable<Pair<Cell>> GetAllPairs(ICollection<Cell> cells) =>
        from a in cells
        from b in cells
        where a != b
        select new Pair<Cell>(a, b);

    private static IEnumerable<Cell> GetAntinodes(ICollection<Cell> cells, Box box, Func<Pair<Cell>, Box, IEnumerable<Cell>> antinodesGenerator) =>
        GetAllPairs(cells).SelectMany(pair => antinodesGenerator(pair, box));

    private static IEnumerable<Cell> GetAntinodesPartOne(Pair<Cell> pair, Box box) => new[] { GetNext(pair.A, pair.B) }.Where(box.Contains);

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

    private static Cell GetNext(Cell a, Cell b) => b + (b - a);

    private static object Solve(List<string> input, Func<Pair<Cell>, Box, IEnumerable<Cell>> antinodedGenerator)
    {
        var grid = GridFactory.Create(input);
        return grid.Entries
            .Where(x => x.Value != '.')
            .GroupBy(entry => entry.Value, entry => entry.Cell)
            .SelectMany(entries => GetAntinodes(entries.ToList(), grid.GetBox(), antinodedGenerator))
            .Distinct()
            .Count();
    }
}
