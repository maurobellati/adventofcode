namespace AdventOfCode;

public static class GridExtensions
{
    public static bool Contains<T>(this Grid<T> grid, Cell cell) => grid.Contains(cell.Row, cell.Col);

    public static bool Contains<T>(this Grid<T> grid, int row, int col) => row.Between(0, grid.Rows - 1) && col.Between(0, grid.Cols - 1);

    public static Box GetBox<T>(this Grid<T> grid) => new(Cell.Origin, new(grid.Rows - 1, grid.Cols - 1));

    public static IEnumerable<GridEntry<T>> GetEntries<T>(this Grid<T> grid)
    {
        for (var row = 0; row < grid.Rows; row++)
        {
            for (var col = 0; col < grid.Cols; col++)
            {
                yield return new(new(row, col), grid.Items[row][col]);
            }
        }
    }
}
