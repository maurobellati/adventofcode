namespace AdventOfCode.Y2023.Day21;

using System.Diagnostics;
using Tool;

public class Y2023Day21 : Solver
{
    private const char Rock = '#';

    public object PartOne(List<string> input)
    {
        var grid = ParseGrid(input);
        var start = FindStart(grid);

        return Bfs(grid, start, 64);
    }

    public object PartTwo(List<string> input)
    {
        var steps = 26501365;

        var grid = ParseGrid(input);
        var start = FindStart(grid);

        Debug.Assert(grid.Rows == grid.Cols, "grid must be a square");
        var size = grid.Rows;
        Debug.Assert(size % 2 == 1, "size of the grid must be odd");
        var halfSize = (grid.Rows - 1) / 2;

        // the steps grow with quadratic rate. We can use three points to compute the quadratic function
        // and then use the function to compute the number of steps for the input value
        var p1 = Bfs(grid, start, halfSize);
        var p2 = Bfs(grid, start, size + halfSize);
        var p3 = Bfs(grid, start, (2 * size) + halfSize);

        var n = ((steps * 1L) - halfSize) / size;
        var a = (p3 - (2 * p2) + p1) / 2;
        var b = p2 - p1 - a;
        var c = p1;
        return c + (b * n) + (a * n * n);
    }

    private static int Bfs(Grid<char> grid, Cell start, int steps)
    {
        // store the frontier for each step
        List<HashSet<Cell>> frontierByStep =
        [
            [start]
        ];

        // store ALL visited cells to avoid visiting them again
        HashSet<Cell> visited = [start];

        for (var i = 1; i <= steps; i++)
        {
            // use the frontier from the previous step
            var frontier = frontierByStep[i - 1];

            HashSet<Cell> discovered = [];
            foreach (var candidate in frontier.SelectMany(it => it.MoveMany(Direction.NSEW)))
            {
                var cell = candidate.Mod(grid);
                if (grid.ValueAt(cell) == Rock)
                {
                    continue;
                }

                if (!visited.Add(candidate))
                {
                    // Console.WriteLine($"Cell {candidate} already visited");
                    continue;
                }

                // Console.WriteLine($"Cell {candidate} added to visited");
                discovered.Add(candidate);
            }

            frontierByStep.Add(discovered);
        }

        // sum the frontier for each step, taking just the cells at the same parity of the input steps
        var result = frontierByStep.Where((_, step) => step % 2 == steps % 2).Sum(it => it.Count);
        // Console.WriteLine($"steps: {steps}, result: {result}");
        return result;
    }

    private static Cell FindStart(Grid<char> grid) => grid.GetEntries().Single(it => it.Value == 'S').Cell;

    private static Grid<char> ParseGrid(List<string> input) => GridFactory.Create(input);
}
