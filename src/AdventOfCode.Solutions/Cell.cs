namespace AdventOfCode;

public sealed record Cell(int Row, int Col) : IComparable<Cell>
{
    public static readonly Cell Origin = new(0, 0);
    public static readonly Cell Zero = new(0, 0);

    public int CompareTo(Cell? other) => other is null ? 1 : Row.NullableCompareTo(other.Row) ?? Col.CompareTo(other.Col);

    public static Cell operator +(Cell x, Cell y) => new(x.Row + y.Row, x.Col + y.Col);

    public static bool operator >(Cell x, Cell y) => x.CompareTo(y) > 0;

    public static bool operator >=(Cell x, Cell y) => x.CompareTo(y) >= 0;

    public static bool operator <(Cell x, Cell y) => x.CompareTo(y) < 0;

    public static bool operator <=(Cell x, Cell y) => x.CompareTo(y) <= 0;

    public static Cell operator *(Cell cell, int factor) => factor == 1 ? cell : new(cell.Row * factor, cell.Col * factor);

    public static Cell operator -(Cell x, Cell y) => new(x.Row - y.Row, x.Col - y.Col);

    public override string ToString() => $"({Row}, {Col})";
}
