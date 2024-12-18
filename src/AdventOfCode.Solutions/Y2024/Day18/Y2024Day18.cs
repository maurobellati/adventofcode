namespace AdventOfCode.Y2024.Day18;

using Tool;
using static StringComparison;

public class Y2024Day18 : Solver
{
    public object PartOne(List<string> input)
    {
        var corruptions = Corruptions(input).Take(CorruptionsCount(input)).ToList();
        return FindShortestPathLength(corruptions, MemorySize(input)) ?? throw new InvalidOperationException("No path found");
    }

    public object PartTwo(List<string> input)
    {
        var corruptions = Corruptions(input);
        var result = FindFirstCellToBlockPath(corruptions, MemorySize(input));
        return $"{result.Col},{result.Row}";
    }

    private static List<Cell> Corruptions(List<string> input) => input.Where(line => !IsConfig(line)).Select(ParseCell).ToList();

    private static int CorruptionsCount(List<string> input) => input.FirstOrDefault(IsConfig)?.ExtractInts().Last() ?? 1024;

    private static Cell FindFirstCellToBlockPath(List<Cell> corruptions, int memorySize)
    {
        var hasPathCount = 0;
        var noPathCount = corruptions.Count;

        // Binary search for the first wall that blocks the path
        while (noPathCount - hasPathCount > 1) // Stop when found the edge between path and no path
        {
            var count = (hasPathCount + noPathCount) / 2; // Try the middle value
            var pathExists = FindShortestPathLength(corruptions.Take(count).ToList(), memorySize).HasValue;
            if (pathExists)
            {
                hasPathCount = count; // If path exists, try with more walls
            }
            else
            {
                noPathCount = count; // If path doesn't exist, try with fewer walls
            }
        }

        return corruptions[noPathCount - 1];
    }

    private static int? FindShortestPathLength(List<Cell> corruptions, int size)
    {
        var start = Cell.Origin;
        var goal = new Cell(size, size);
        var box = new Box(start, goal);

        HashSet<Cell> visited = [start];
        Queue<Cell> queue = new([start]);
        Dictionary<Cell, int> costs = new() { [start] = 0 };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == goal)
            {
                return costs[goal];
            }

            foreach (var next in current.MoveMany(Direction.NSEW).Where(box.Contains).Where(cell => !corruptions.Contains(cell)))
            {
                if (visited.Add(next))
                {
                    queue.Enqueue(next);
                    costs[next] = costs[current] + 1;
                }
            }
        }

        return null;
    }

    private static bool IsConfig(string line) => line.StartsWith("config:", InvariantCulture);

    private static int MemorySize(List<string> input) => input.FirstOrDefault(IsConfig)?.ExtractInts().First() ?? 70;

    private static Cell ParseCell(string line) => new(line.ExtractInts().Last(), line.ExtractInts().First());
}
