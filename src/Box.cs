namespace AdventOfCode;

using System.Collections;

public sealed record Box(Cell TopLeft, Cell BottomRight) : IEnumerable<Cell>
{
    public IEnumerator<Cell> GetEnumerator()
    {
        for (var row = TopLeft.Row; row <= BottomRight.Row; row++)
        {
            for (var col = TopLeft.Col; col <= BottomRight.Col; col++)
            {
                yield return new(row, col);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static Box operator *(Box box, int factor) => new(box.TopLeft * factor, box.BottomRight * factor);
}

public static class BoxExtensions
{
    public static int Area(this Box input) => (input.BottomRight.Row - input.TopLeft.Row + 1) * (input.BottomRight.Col - input.TopLeft.Col + 1);

    public static bool Contains(this Box input, Cell cell) =>
        cell.Row.Between(input.TopLeft.Row, input.BottomRight.Row) &&
        cell.Col.Between(input.TopLeft.Col, input.BottomRight.Col);

    public static Box Expand(this Box box, int rows, int cols) =>
        new(box.TopLeft - new Cell(rows, cols), box.BottomRight + new Cell(rows, cols));

    public static Box? Intersect(this Box input, Box other)
    {
        var topLeft = new Cell(Math.Max(input.TopLeft.Row, other.TopLeft.Row), Math.Max(input.TopLeft.Col, other.TopLeft.Col));
        var bottomRight = new Cell(Math.Min(input.BottomRight.Row, other.BottomRight.Row), Math.Min(input.BottomRight.Col, other.BottomRight.Col));
        return topLeft.Row <= bottomRight.Row && topLeft.Col <= bottomRight.Col ? new Box(topLeft, bottomRight) : null;
    }
}
