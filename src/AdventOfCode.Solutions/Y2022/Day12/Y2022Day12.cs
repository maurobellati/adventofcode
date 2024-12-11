namespace AdventOfCode.Y2022.Day12;

using Tool;

public class Y2022Day12 : Solver
{
    public object PartOne(List<string> input)
    {
        var (grid, start, goal) = ParseInput(input);
        return GetCost(grid, start, goal);
    }

    public object PartTwo(List<string> input)
    {
        var (grid, start, goal) = ParseInput(input);
        return grid.Entries
            .Where(entry => entry.Cell == start || entry.Value is 'a')
            .Min(cell => GetCost(grid, cell.Cell, goal))!;
    }

    private static Cell FindCell(Grid<char> grid, char value) => grid.Entries.Single(entry => entry.Value == value).Cell;

    private static int GetCost(Grid<char> grid, Cell start, Cell goal)
    {
        Dictionary<Cell, int> costs = new() { [start] = 0 };
        PriorityQueue<Cell, int> queue = new([(start, 0)]);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == goal)
            {
                // Console.WriteLine($"Goal found at {currentCell} with cost {costs[currentCell]}");
                return costs[current];
            }

            var nextCost = costs[current] + 1;
            foreach (var next in Direction.NSEW
                         .Select(dir => current.Move(dir))
                         .Where(grid.Contains) // OK if the cell is inside the grid
                         .Where(next => grid.ValueAt(next) - grid.ValueAt(current) <= 1) // OK if the step is not higher than 1
                         .Where(next => !costs.TryGetValue(next, out var value) || nextCost < value) // OK if 'next' is never visited (no values in costs[]) or visited but with higher cost
                    )
            {
                queue.Enqueue(next, nextCost);
                costs[next] = nextCost;
            }
        }

        return int.MaxValue;
    }

    private static (Grid<char>, Cell, Cell) ParseInput(List<string> input)
    {
        var grid = GridFactory.Create(input);
        var startCell = FindCell(grid, 'S');
        var goalCell = FindCell(grid, 'E');

        grid.SetValue(startCell, 'a');
        grid.SetValue(goalCell, 'z');

        return (grid, startCell, goalCell);
    }
}
