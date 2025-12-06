namespace AdventOfCode.Y2023.Day14;

using System.Text;
using Tool;

public class Y2023Day14 : Solver
{
    public object PartOne(List<string> input)
    {
        var platform = Platform.Parse(input);
        platform.TiltNorth();
        return NorthLoad(platform);
    }

    public object PartTwo(List<string> input)
    {
        var platform = Platform.Parse(input);
        const int TargetCycle = 1_000_000_000;

        // Keep cycling the platform until the load shows a repeating pattern.
        // Use hash codes to detect repeating patterns instead of the loads since the loads can be the same for different platforms.

        List<int> loads = [0];
        List<int> hashCodes = [0];
        int hash;
        while (true)
        {
            // Console.Write($"Cycle {loads.Count + 1,4}: ");
            platform.Cycle();
            hash = platform.GetHashCode();

            if (hashCodes.Contains(hash))
            {
                // Console.WriteLine($"Found repeating hash {hash}");
                break;
            }

            var load = NorthLoad(platform);
            // Console.WriteLine($"Load{load,7} {hash,15} {(platformHashes.Contains(hash) ? "<==" : "")}");
            loads.Add(load);
            hashCodes.Add(hash);
        }

        var firstIndex = hashCodes.IndexOf(hash);
        var cycleLength = hashCodes.Count - firstIndex;
        var index = (TargetCycle - firstIndex).Mod(cycleLength);
        var result = loads[firstIndex + index];

        return result;
    }

    private static int NorthLoad(Platform platform) =>
        platform.Items.Where(it => it.IsRock)
            .Select(it => platform.RowCount - it.Row)
            .Sum();

#pragma warning disable CA1036 //Item should define operator(s) '==, !=, <, <=, >, >='
    public class Item : IComparable<Item>
#pragma warning restore CA1036
    {
        public int Col { get; set; }

        public bool IsRock => Symbol == 'O';

        public int Row { get; set; }

        public char Symbol { get; init; }

        public int CompareTo(Item? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return Row.NullableCompareTo(other.Row) ??
                   Col.NullableCompareTo(other.Col) ??
                   Symbol.CompareTo(other.Symbol);
        }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || (obj is Item other && Equals(other));

        public override int GetHashCode() => HashCode.Combine(Col, Row, Symbol);

        public override string ToString() => $"{{{Symbol}({Row},{Col})}}";

        private bool Equals(Item other) => Col == other.Col && Row == other.Row && Symbol == other.Symbol;
    }

    public class Platform
    {
        public required int ColumnCount { get; init; }

        public required List<Item> Items { get; init; }

        public required int RowCount { get; init; }

        public static Platform Parse(IEnumerable<string> lines)
        {
            // lines are in the form of:
            // O....#....
            // O.OO#....#
            // .....##...

            // O are rocks
            // # are cubes
            // . are empty spaces

            var grid = new Grid<char>(lines);
            var items = grid.GetEntries()
                .Where(entry => entry.Value != '.')
                .Select(
                    entry => new Item
                    {
                        Row = entry.Cell.Row,
                        Col = entry.Cell.Col,
                        Symbol = entry.Value
                    }).ToList();

            return new()
            {
                Items = items,
                RowCount = grid.Rows,
                ColumnCount = grid.Cols
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Platform other)
            {
                return false;
            }

            return ReferenceEquals(this, obj) || Equals(other);
        }

        public override int GetHashCode()
        {
            var result = new HashCode();
            result.Add(RowCount);
            result.Add(ColumnCount);
            result.AddRange(Items.Order());
            return result.ToHashCode();
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            var items = Items.ToDictionary(it => (it.Row, it.Col), it => it.Symbol);
            foreach (var row in Enumerable.Range(0, RowCount))
            {
                foreach (var col in Enumerable.Range(0, ColumnCount))
                {
                    result.Append(items.GetValueOrDefault((row, col), '.'));
                }

                result.AppendLine();
            }

            return result.ToString();
        }

        public void Cycle()
        {
            TiltNorth();
            TiltWest();
            TiltSouth();
            TiltEast();
        }

        public void TiltEast()
        {
            foreach (var row in Items.GroupBy(it => it.Row))
            {
                TiltEast(row);
            }
        }

        public void TiltNorth()
        {
            foreach (var column in Items.GroupBy(it => it.Col))
            {
                TiltNorth(column);
            }
        }

        public void TiltSouth()
        {
            foreach (var column in Items.GroupBy(it => it.Col))
            {
                TiltSouth(column);
            }
        }

        public void TiltWest()
        {
            foreach (var row in Items.GroupBy(it => it.Row))
            {
                TiltWest(row);
            }
        }

        protected bool Equals(Platform other) =>
            ColumnCount == other.ColumnCount &&
            RowCount == other.RowCount &&
            Items.Order().SequenceEqual(other.Items.Order());

        private void TiltEast(IEnumerable<Item> row)
        {
            var currentCol = ColumnCount;
            foreach (var item in row.OrderByDescending(it => it.Col))
            {
                if (item.IsRock)
                {
                    item.Col = --currentCol;
                }
                else
                {
                    currentCol = item.Col;
                }
            }
        }

        private void TiltNorth(IEnumerable<Item> column)
        {
            var currentRow = -1;
            foreach (var item in column.OrderBy(it => it.Row))
            {
                if (item.IsRock)
                {
                    item.Row = ++currentRow;
                }
                else
                {
                    currentRow = item.Row;
                }
            }
        }

        private void TiltSouth(IEnumerable<Item> column)
        {
            var currentRow = RowCount;
            foreach (var item in column.OrderByDescending(it => it.Row))
            {
                if (item.IsRock)
                {
                    item.Row = --currentRow;
                }
                else
                {
                    currentRow = item.Row;
                }
            }
        }

        private void TiltWest(IEnumerable<Item> row)
        {
            var currentCol = -1;
            foreach (var item in row.OrderBy(it => it.Col))
            {
                if (item.IsRock)
                {
                    item.Col = ++currentCol;
                }
                else
                {
                    currentCol = item.Col;
                }
            }
        }
    }
}
