namespace AdventOfCode.Y2023.Day10;

using Tool;

public class Y2023Day10 : Solver
{
    public object PartOne(List<string> input)
    {
        var grid = GridFactory.Create(input);
        var start = FindStart(input);
        var loop = FindLoop(grid, start);

        return loop.Count / 2;
    }

    public object PartTwo(List<string> input)
    {
        var grid = GridFactory.Create(input);
        var start = FindStart(input);
        var loop = FindLoop(grid, start);

        // double the size of the grid, so the flood fill can go between contiguous segments
        var box = grid.GetBox() * 2;

        // double the resolution of the loop, so there are no gaps between loop segments
        var tightLoop = loop
            .Select(segment => segment with { Cell = segment.Cell * 2 })
            .SelectMany(
                segment => new[] { segment.Cell, segment.NextCell() })
            .ToHashSet();

        HashSet<Cell> visited = [..tightLoop];
        HashSet<Cell> inside = [];
        foreach (var cell in box.Where(it => !visited.Contains(it)))
        {
            var (floodedCells, isInside) = Flood(cell, tightLoop, box);
            visited.UnionWith(floodedCells);
            if (isInside)
            {
                // select only the flooded cells that are on the main grid (the ones that have even coordinates)
                var mainGridFloodedCells = floodedCells.Where(it => it.Row % 2 == 0 && it.Col % 2 == 0).ToHashSet();
                inside.UnionWith(mainGridFloodedCells);
            }
        }

        return inside.Count;
    }

    public static Segment? NextSegment(Grid<char> grid, Segment input)
    {
        var next = input.NextCell();
        if (!grid.Contains(next))
        {
            return null;
        }

        var symbol = grid.ValueAt(next);
        var pipeType = PipeType.FromSymbol(symbol);
        if (pipeType is null)
        {
            return null;
        }

        var inDirection = input.Direction.Opposite();
        if (!pipeType.Directions.Contains(inDirection))
        {
            return null;
        }

        return new(next, symbol, pipeType.Directions.Other(inDirection));
    }

    private static List<Segment> FindLoop(Grid<char> grid, Cell start)
    {
        // find the first segment that is not a dead end
        var startSegment = Direction.NSEW
            .Select(direction => new Segment(start, 'S', direction))
            .First(segment => NextSegment(grid, segment) != null);

        List<Segment> loop = [startSegment];

        var current = startSegment;
        do
        {
            current = NextSegment(grid, current);
            loop.Add(current ?? throw new InvalidOperationException($"the loop is not closed: [{string.Join(", ", loop)}]"));
        } while (current.NextCell() != startSegment.Cell);

        return loop;
    }

    private static Cell FindStart(List<string> lines) =>
        Enumerable.Range(0, lines.Count)
            .Select(row => new Cell(row, lines[row].IndexOf('S', InvariantCulture)))
            .Single(cell => cell.Col >= 0);

    private static (ISet<Cell> Visited, bool IsInside) Flood(Cell cell, HashSet<Cell> loop, Box box)
    {
        HashSet<Cell> visitedCells = [cell];
        var floodFrontier = new Queue<Cell>([cell]);

        var isInside = true;
        while (floodFrontier.IsNotEmpty())
        {
            var currentCell = floodFrontier.Dequeue();
            foreach (var nextCell in currentCell.MoveMany(Direction.NSEW))
            {
                if (visitedCells.Contains(nextCell) || loop.Contains(nextCell))
                {
                    continue;
                }

                isInside &= box.Contains(nextCell);
                if (!isInside)
                {
                    continue;
                }

                visitedCells.Add(nextCell);
                floodFrontier.Enqueue(nextCell);
            }
        }

        return (visitedCells, isInside);
    }

    public sealed record PipeType(char Symbol, Pair<Direction> Directions)
    {
        private static readonly Dictionary<char, PipeType> All = new PipeType[]
        {
            new('|', new(Direction.N, Direction.S)),
            new('-', new(Direction.E, Direction.W)),
            new('L', new(Direction.N, Direction.E)),
            new('J', new(Direction.N, Direction.W)),
            new('7', new(Direction.S, Direction.W)),
            new('F', new(Direction.S, Direction.E))
        }.ToDictionary(it => it.Symbol, it => it);

        public static PipeType? FromSymbol(char symbol)
        {
            All.TryGetValue(symbol, out var result);
            return result;
        }
    }

    public sealed record Segment(Cell Cell, char Symbol, Direction Direction)
    {
        public Cell NextCell() => Cell.Move(Direction);
    }
}
