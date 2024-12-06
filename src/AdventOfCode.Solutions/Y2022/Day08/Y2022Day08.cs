namespace AdventOfCode.Y2022.Day08;

using AdventOfCodeDotNet;

public class Y2022Day08 : Solver
{
    public object PartOne(List<string> input)
    {
        var grid = GridFactory.Create(input, @char => @char - '0');
        return grid.Entries.Count(entry => CanSeeOutside(grid, entry));
    }

    public object PartTwo(List<string> input)
    {
        var grid = GridFactory.Create(input, @char => @char - '0');
        return grid.Entries.Select(entry => ScenicScore(grid, entry)).Max();
    }

    private static bool CanSeeOutside(Grid<int> grid, GridEntry<int> entry) =>
        Direction.NSEW.Any(direction => Walk(grid, entry, direction).Escaped);

    private static int ScenicScore(Grid<int> grid, GridEntry<int> entry) =>
        Direction.NSEW.Select(direction => Walk(grid, entry, direction).Distance).Aggregate(1, (acc, distance) => acc * distance);

    private static (int Distance, bool Escaped) Walk(Grid<int> grid, GridEntry<int> start, Direction direction)
    {
        var steps = 0;

        var cell = start.Cell;
        while (true)
        {
            var nextCell = cell.Move(direction);
            if (!grid.Contains(nextCell))
            {
                return (steps, Escaped: true);
            }

            steps++;
            if (grid.ValueAt(nextCell) >= start.Value)
            {
                return (steps, Escaped: false);
            }

            cell = nextCell;
        }
    }
}
