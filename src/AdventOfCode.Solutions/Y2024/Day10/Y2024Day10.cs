namespace AdventOfCode.Y2024.Day10;

using Tool;

public class Y2024Day10 : Solver
{
    private const int TrailheadValue = 0;
    private const int PeekValue = 9;
    private const int MaxStep = 1;

    public object PartOne(List<string> input) => Solve(input, GetDistinctPeeksCount);

    public object PartTwo(List<string> input) => Solve(input, GetRatings);

    private static IEnumerable<Cell> GetAllPeeks(Grid<int> grid, Cell cell)
    {
        var value = grid.ValueAt(cell);
        return value == PeekValue
            ? [cell]
            : Direction.NSEW
                .Select(dir => cell.Move(dir))
                .Where(next => grid.ValueAtOrDefault(next) - value == MaxStep)
                .SelectMany(next => GetAllPeeks(grid, next));
    }

    private static int GetDistinctPeeksCount(Grid<int> grid, Cell cell) => GetAllPeeks(grid, cell).Distinct().Count();

    private static int GetRatings(Grid<int> grid, Cell cell) => GetAllPeeks(grid, cell).Count();

    private static object Solve(List<string> input, Func<Grid<int>, Cell, int> counter)
    {
        var grid = GridFactory.Create(input, @char => @char - '0');
        return grid.Entries.Where(entry => entry.Value == TrailheadValue)
            .Select(trailHead => counter(grid, trailHead.Cell))
            .Sum();
    }
}
