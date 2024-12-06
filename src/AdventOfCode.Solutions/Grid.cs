namespace AdventOfCode;

public sealed class Grid<T>(List<List<T>> items)
{
    public Grid(IEnumerable<IEnumerable<T>> items) : this(items.Select(it => it.ToList()).ToList())
    {
    }

    public int Cols { get; } = items[0].Count;

    public IEnumerable<GridEntry<T>> Entries => Items.SelectMany((row, rowIndex) => row.Select((value, colIndex) => new GridEntry<T>(new(rowIndex, colIndex), value)));

    public List<List<T>> Items { get; } = items.ToList();

    public int Rows { get; } = items.Count;

    public override string ToString() => $"Grid({Rows}x{Cols})";

    public GridEntry<T> EntryAt(Cell cell) => new(cell, this.ValueAt(cell));

    public void SetValue(int row, int col, T value) => Items[row][col] = value;

    public void SetValue(Cell cell, T value) => SetValue(cell.Row, cell.Col, value);
}

public static class GridFactory
{
    public static Grid<T> Create<T, TIn>(IEnumerable<IEnumerable<TIn>> items, Func<TIn, T> parser) =>
        new(items.Select(row => row.Select(parser)));

    public static Grid<T> Create<T>(IEnumerable<IEnumerable<T>> items) => new(items);

    public static Grid<T> Empty<T>(int rowCount, int columnCount, T value) =>
        new(Enumerable.Range(0, rowCount).Select(_ => Enumerable.Repeat(value, columnCount)));
}

public record GridEntry<T>(Cell Cell, T Value)
{
    public override string ToString() => $"GridEntry({Cell}, {Value})";
}
