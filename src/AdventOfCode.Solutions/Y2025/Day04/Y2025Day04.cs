namespace AdventOfCode.Y2025.Day04;

using Tool;

public class Y2025Day04 : Solver
{
    public object PartOne(List<string> input)
    {
        var grid = GridFactory.Create(input);
        return GetRemovableEntries(grid).Count();
    }

    public object PartTwo(List<string> input)
    {
        var grid = GridFactory.Create(input);
        List<GridEntry<char>> removed;
        do
        {
            removed = GetRemovableEntries(grid).ToList();
            removed.ForEach(it => grid.SetValue(it.Cell, 'X'));
        } while (removed.Count > 0);

        return grid.Entries.Count(entry => entry.Value == 'X');
    }

    private static int CountNeighbors(GridEntry<char> entry, Grid<char> grid, char c) =>
        entry.Cell.MoveMany(Direction.All).Count(cell => grid.ValueAtOrDefault(cell) == c);

    private static IEnumerable<GridEntry<char>> GetRemovableEntries(Grid<char> grid) =>
        grid.Entries
            .Where(entry => entry.Value == '@')
            .Where(entry => CountNeighbors(entry, grid, '@') < 4);
}
