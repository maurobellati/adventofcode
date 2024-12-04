namespace AdventOfCode;

public class Grid<T>
{
    public Grid(IEnumerable<IEnumerable<T>> items) : this(items.Select(it => it.ToList()).ToList())
    {
    }

    public Grid(List<List<T>> items)
    {
        Items = items.ToList();
        Cols = items[0].Count;
        Rows = items.Count;
    }

    public int Cols { get; }

    public IEnumerable<GridEntry<T>> Entries => Items.SelectMany((row, rowIndex) => row.Select((value, colIndex) => new GridEntry<T>(new(rowIndex, colIndex), value)));

    public List<List<T>> Items { get; }

    public int Rows { get; }

    public virtual GridEntry<T> EntryAt(Cell cell) => new(cell, ValueAt(cell));

    public virtual T ValueAt(Cell cell) => Items[cell.Row][cell.Col];

    public virtual T? ValueAtOrDefault(Cell cell) => this.Contains(cell) ? ValueAt(cell) : default;

    public virtual T ValueAt(int row, int col) => Items[row][col];
}

public static class GridFactory
{
    public static Grid<T> Create<T>(IEnumerable<IEnumerable<T>> items) => new(items);
}

public record GridEntry<T>(Cell Cell, T Value);
