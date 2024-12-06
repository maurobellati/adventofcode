namespace AdventOfCode.Y2024.Day06;

using Tool;

public class Y2024Day06 : Solver
{
    public object PartOne(List<string> input)
    {
        var (grid, start) = Parse(input);
        var walk = Walk(grid, start);

        return walk.Path.Select(step => step.Cell).Distinct().Count();
    }

    public object PartTwo(List<string> input)
    {
        var (grid, start) = Parse(input);
        var walk = Walk(grid, start);

        var loops = 0;
        foreach (var cell in walk.Path.Select(step => step.Cell).Distinct().Where(cell => cell != start.Cell))
        {
            grid.SetValue(cell, '#');

            if (Walk(grid, start).IsLoop)
            {
                loops++;
            }

            grid.SetValue(cell, '.');
        }

        return loops;
    }

    private static (Grid<char> Grid, PathStep Start) Parse(List<string> input)
    {
        var grid = GridFactory.Create(input);
        var start = new PathStep(grid.Entries.Single(entry => entry.Value == '^').Cell, Direction.N);
        return (grid, start);
    }

    private static (HashSet<PathStep> Path, bool IsLoop) Walk(Grid<char> grid, PathStep start)
    {
        HashSet<PathStep> path = [];

        var current = start;
        while (true)
        {
            var added = path.Add(current);
            if (!added)
            {
                return (path, IsLoop: true); // If we've already visited this cell, we're in a loop
            }

            var next = current.Move();

            if (!grid.Contains(next.Cell))
            {
                return (path, IsLoop: false); // If we go off the grid, we're done, without a loop
            }

            var nextValue = grid.ValueAt(next.Cell);
            current = nextValue == '#' ? current.Rotate(Rotation.Clockwise) : next;
        }
    }
}
