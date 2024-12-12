namespace AdventOfCode.Y2024.Day12;

using Tool;
using Region = (char Name, HashSet<Cell> Cells);
using Fence = (Cell Cell, Direction Direction);

public class Y2024Day12 : Solver
{
    public object PartOne(List<string> input) => ExtractRegions(input).Sum(region => GetArea(region) * GetPerimeter(region));

    public object PartTwo(List<string> input) => ExtractRegions(input).Sum(region => GetArea(region) * GetSidesCount(region));

    private static List<Region> ExtractRegions(List<string> input)
    {
        var grid = GridFactory.Create(input);
        var allCells = grid.Entries.Select(it => it.Cell).ToHashSet();

        List<Region> regions = [];
        while (allCells.Count > 0)
        {
            var region = FloodFrom(grid, allCells.First());
            allCells.ExceptWith(region.Cells);
            regions.Add(region);
        }

        return regions;
    }

    private static Region FloodFrom(Grid<char> grid, Cell start)
    {
        var startValue = grid.ValueAt(start);

        var result = new Region(startValue, []);

        Queue<Cell> queue = new();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            var added = result.Cells.Add(current);
            if (!added) // Already visited, no need to explore its neighbors.
            {
                continue;
            }

            foreach (var next in current.MoveMany(Direction.NSEW).Where(next => grid.ValueAtOrDefault(next) == startValue))
            {
                queue.Enqueue(next);
            }
        }

        return result;
    }

    private static long GetArea(Region region) => region.Cells.Count;

    private static IEnumerable<Fence> GetFences(Cell cell, Region region) =>
        from direction in Direction.NSEW
        let next = cell.Move(direction)
        where !region.Cells.Contains(next)
        select (cell, direction);

    private static int GetPerimeter(Cell cell, Region region) => GetFences(cell, region).Count();

    private static long GetPerimeter(Region region) =>
        // Just sum the perimeter of each cell in the region.
        region.Cells.Select(cell => GetPerimeter(cell, region)).Sum();

    private static int GetSidesCount(List<Fence> fences, Direction dir, Func<Fence, int> groupSelector, Func<Fence, int> blockCounter) =>
        fences.Where(fence => fence.Direction == dir)
            .OrderBy(blockCounter) // Order by Row or Col so the contiguous blocks are together
            .GroupBy(groupSelector) // Group by Row or Col
            .Select(
                group => group.Zip(group.Skip(1)) // check contiguous fences
                    .Count(cellPair => blockCounter(cellPair.Item2) - blockCounter(cellPair.Item1) != 1)) // if the difference is not 1, it's a new block
            .Select(gapsCount => gapsCount + 1) // gapsCount + 1 = blocksCount
            .Sum();

    private static long GetSidesCount(Region region)
    {
        /*
         * Get all the fences in the region, group them by Direction and Row (if N or S) or Col (if E or W).
         * Then for each group, count how many blocks of contiguous fences there are.
         *
         * Example: for those fences facing N, we group them by Row.
         *
         *            +-+-+-+
         * row 0:     |X X X|
         *        +-+-+     +-+-+-+-+
         * row 1: |X X X X X X X X X|
         *
         * We have 2 groups of fences facing N:
         *   - row 0: 3 fences: 1 block of 3                 => count as 1 side
         *   - row 1: 6 fences: 1 block of 2 + 1 block of 4  => count as 2 sides
         */

        var fences = region.Cells.SelectMany(cell => GetFences(cell, region)).ToList();

        Func<Fence, int> byRow = fence => fence.Cell.Row;
        Func<Fence, int> byCol = fence => fence.Cell.Col;
        return GetSidesCount(fences, Direction.N, byRow, byCol) +
               GetSidesCount(fences, Direction.S, byRow, byCol) +
               GetSidesCount(fences, Direction.E, byCol, byRow) +
               GetSidesCount(fences, Direction.W, byCol, byRow);
    }
}
