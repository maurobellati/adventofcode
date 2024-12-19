namespace AdventOfCode;

public static class CellExtensions
{
    public static Cell Abs(this Cell cell) => new(Math.Abs(cell.Row), Math.Abs(cell.Col));

    public static int CompareTo(this Cell cell, Cell other) => cell.Row.NullableCompareTo(other.Row) ?? cell.Col.CompareTo(other.Col);

    public static int ManhattanDistance(this Cell cell, Cell other) => Math.Abs(cell.Row - other.Row) + Math.Abs(cell.Col - other.Col);

    public static Cell Mod<T>(this Cell cell, Grid<T> grid) => cell.Mod(grid.Rows, grid.Cols);

    public static Cell Mod(this Cell cell, Cell size) => cell.Mod(size.Row, size.Col);

    public static Cell Mod(this Cell cell, int rows, int cols) => new(cell.Row.Mod(rows), cell.Col.Mod(cols));

    public static Cell Move(this Cell input, Direction direction, int size = 1) => input + direction.ToStep(size);

    public static Cell Move(this Cell input, int deltaRow, int deltaCol) => new(input.Row + deltaRow, input.Col + deltaCol);

    public static IEnumerable<Cell> MoveMany(this Cell input, IEnumerable<Direction> directions, int size = 1) => directions.Select(d => input.Move(d, size));

    public static PathStep Towards(this Cell cell, Direction direction) => new(cell, direction);
}
