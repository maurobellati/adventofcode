namespace AdventOfCode.Y2024.Day20;

using System.Diagnostics;
using Tool;

public class Y2024Day20 : Solver
{
    public object PartOne(List<string> input) => Solve(input, 2);

    public object PartTwo(List<string> input) => Solve(input, 20);

    private static List<Cell> FindPath(List<string> input)
    {
        var grid = GridFactory.Create(input);
        var start = grid.Entries.Single(e => e.Value == 'S').Cell;
        var goal = grid.Entries.Single(e => e.Value == 'E').Cell;

        List<Cell> path = [start];
        while (path.Last() != goal)
        {
            var current = path.Last();
            var next = current.MoveMany(Direction.NSEW).Single(it => grid.ValueAtOrDefault(it) != '#' && !path.Contains(it));
            path.Add(next);
        }

        return path;
    }

    private static string? GetConfig(List<string> input) => input.SingleOrDefault(line => line.StartsWith("config", InvariantCulture));

    private static IEnumerable<Cell> GetManhattanNeighbours(Cell center, int maxManhattanDistance)
    {
        for (var distance = 1; distance <= maxManhattanDistance; distance++)
        {
            foreach (var cell in center.ManhattanRing(distance))
            {
                yield return cell;
            }
        }
    }

    private static int Solve(List<string> input, int maxShortcutLength)
    {
        var minShortcutSaving = GetConfig(input)?.ExtractInts().First() ?? 100;
        var path = FindPath(input);
        var pathToIndex = path.Select((cell, index) => (cell, index)).ToDictionary(it => it.cell, it => it.index);

        return path.SelectMany(
            shortcutStart => GetManhattanNeighbours(shortcutStart, maxShortcutLength)
                .Where(pathToIndex.ContainsKey)
                .Where(
                    shortcutEnd =>
                    {
                        var pathSaved = pathToIndex[shortcutEnd] - pathToIndex[shortcutStart];
                        var shortcutLength = shortcutStart.ManhattanDistance(shortcutEnd);
                        var saving = pathSaved - shortcutLength;
                        return saving >= minShortcutSaving;
                    })
        ).Count();
    }
}
