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

    public static List<List<T>> Rotate45<T>(this Grid<T> grid, Rotation rotation) =>
        rotation switch
        {
            Rotation.Clockwise => grid.Rotate45(),
            Rotation.CounterClockwise => grid.RotateMinus45(),
            _ => throw new ArgumentOutOfRangeException(nameof(rotation))
        };

    public static List<List<T>> Rotate45<T>(this Grid<T> grid)
    {
        var rows = grid.Rows;
        var cols = grid.Cols;
        var result = new List<List<T>>();

        for (var sum = 0; sum < rows + cols - 1; sum++)
        {
            var diagonal = new List<T>();
            for (var row = 0; row <= sum; row++)
            {
                var col = sum - row;
                if (row < rows && col < cols)
                {
                    diagonal.Add(grid.Items[row][col]);
                }
            }

            result.Add(diagonal);
        }

        return result;
    }

    public static Grid<T> Rotate90<T>(this Grid<T> grid, Rotation rotation) =>
        rotation switch
        {
            Rotation.Clockwise => grid.Rotate90(),
            Rotation.CounterClockwise => grid.RotateMinus90(),
            _ => throw new ArgumentOutOfRangeException(nameof(rotation))
        };

    public static Grid<T> Rotate90<T>(this Grid<T> grid)
    {
        var result = new List<List<T>>();
        for (var col = 0; col < grid.Cols; col++)
        {
            var rotatedRow = new List<T>();
            for (var row = grid.Rows - 1; row >= 0; row--)
            {
                rotatedRow.Add(grid.Items[row][col]);
            }

            result.Add(rotatedRow);
        }

        return new(result);
    }

    public static List<List<T>> RotateMinus45<T>(this Grid<T> grid)
    {
        var rows = grid.Rows;
        var cols = grid.Cols;
        var result = new List<List<T>>();

        for (var diff = 1 - rows; diff < cols; diff++)
        {
            var diagonal = new List<T>();
            for (var row = 0; row < rows; row++)
            {
                var col = row + diff;
                if (col >= 0 && col < cols)
                {
                    diagonal.Add(grid.Items[row][col]);
                }
            }

            result.Add(diagonal);
        }

        return result;
    }

    public static Grid<T> RotateMinus90<T>(this Grid<T> grid)
    {
        var result = new List<List<T>>();
        for (var col = grid.Cols - 1; col >= 0; col--)
        {
            var rotatedRow = new List<T>();
            for (var row = 0; row < grid.Rows; row++)
            {
                rotatedRow.Add(grid.Items[row][col]);
            }

            result.Add(rotatedRow);
        }

        return new(result);
    }
}
