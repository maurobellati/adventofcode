namespace AdventOfCode;

public sealed record Cell(int Row, int Col)
{
    public static Cell Origin => new(0, 0);

    public static Cell operator +(Cell x, Cell y) => new(x.Row + y.Row, x.Col + y.Col);

    public static bool operator >(Cell x, Cell y) => x.CompareTo(y) > 0;

    public static bool operator >=(Cell x, Cell y) => x.CompareTo(y) >= 0;

    public static bool operator <(Cell x, Cell y) => x.CompareTo(y) < 0;

    public static bool operator <=(Cell x, Cell y) => x.CompareTo(y) <= 0;

    public static Cell operator *(Cell cell, int factor) => factor == 1 ? cell : new(cell.Row * factor, cell.Col * factor);

    public static Cell operator -(Cell x, Cell y) => new(x.Row - y.Row, x.Col - y.Col);
}
