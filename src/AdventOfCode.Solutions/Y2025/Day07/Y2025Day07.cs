namespace AdventOfCode.Y2025.Day07;

using Tool;

public class Y2025Day07 : Solver
{
    private const char Beam = '|';
    private const char Splitter = '^';
    private const char Start = 'S';

    public object PartOne(List<string> input)
    {
        var grid = input.Select(line => line.ToCharArray().ToList()).ToList();

        var totalSplitCount = 0;
        for (var row = 1; row < grid.Count; row++)
        {
            var step = CalculateStep(grid, row);
            ApplyStep(step, grid, row);

            var splits = step.Count(columns => columns.Count == 2);
            totalSplitCount += splits;
        }

        return totalSplitCount;
    }

    public object PartTwo(List<string> input)
    {
        var grid = input.Select(line => line.ToCharArray().ToList()).ToList();
        var rows = grid.Count;
        var cols = grid[0].Count;
        var timeline = new long[rows, cols];

        for (var row = 1; row < rows; row++)
        {
            var step = CalculateStep(grid, row);
            ApplyStep(step, grid, row);
        }

        // Initialize last row based on grid
        for (var c = 0; c < cols; c++)
        {
            if (grid[^1][c] == Beam)
            {
                timeline[rows - 1, c] = 1;
            }
        }

        // Backward DP to count timelines
        for (var r = rows - 2; r >= 0; r--)
        {
            for (var c = 0; c < cols; c++)
            {
                var hereAndBelow = (grid[r][c], grid[r + 1][c]);
                var timelineValue = hereAndBelow switch
                {
                    (Beam, Beam) or (Start, Beam) => timeline[r + 1, c],
                    (Beam, Splitter) => timeline[r + 1, c - 1] + timeline[r + 1, c + 1],
                    (_, _) => 0
                };
                timeline[r, c] = timelineValue;
            }
        }

        var sIndex = grid[0].IndexOf(Start);
        return timeline[0, sIndex];
    }

    private static void ApplyStep(List<List<int>> step, List<List<char>> grid, int row)
    {
        foreach (var beamColumn in step.SelectMany(columns => columns))
        {
            grid[row][beamColumn] = Beam;
        }
    }

    private static List<List<int>> CalculateStep(List<List<char>> grid, int row) =>
        Enumerable.Range(0, grid[0].Count)
            .Select(col =>
            {
                var aboveAndHere = (grid[row - 1][col], grid[row][col]);
                return (List<int>)(aboveAndHere switch
                {
                    (Start, _) => [col],
                    (Beam, Splitter) => [col - 1, col + 1],
                    (Beam, _) => [col],
                    _ => []
                });
            })
            .ToList();
}
