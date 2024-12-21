namespace AdventOfCode.Y2024.Day21;

using System.Collections.Concurrent;
using Tool;
using Path = List<Direction>;

// using Paths = List<List<Direction>>;

public class Y2024Day21 : Solver
{
    private static readonly Keypad DirectionalKeypad = Keypad.FromRawString(
        """
         ^A
        <v>
        """);

    private static readonly Dictionary<Direction, char> DirectionToChar = new()
    {
        [Direction.N] = '^',
        [Direction.S] = 'v',
        [Direction.E] = '>',
        [Direction.W] = '<'
    };

    private static readonly Keypad NumericKeypad = Keypad.FromRawString(
        """
        789
        456
        123
         0A
        """);

    private static readonly ConcurrentDictionary<(char currentKey, char nextKey, int depth), long> Cache = new();

    public object PartOne(List<string> input) => Solve(input, 2);

    public object PartTwo(List<string> input) => Solve(input, 25);

    private static string AsKeypadSymbols(Path path) => new(path.Select(d => DirectionToChar[d]).ToArray());

    private long Complexity(string code, List<Keypad> keypads) => ShortestSequenceLength(code, keypads) * code.ExtractInts().Single();

    private long ShortestSequenceLength(string keys, List<Keypad> keypads)
    {
        if (keypads.IsEmpty())
        {
            return keys.Length;
        }

        return keys.Aggregate(
            (Key: 'A', Cost: 0L), // Initial state
            (state, nextKey) =>
            {
                var newCost = state.Cost + ShortestSequenceLength(state.Key, nextKey, keypads);
                return (nextKey, newCost);
            }
        ).Cost;
    }

    private long ShortestSequenceLength(char currentKey, char nextKey, List<Keypad> keypads) =>
        Cache.GetOrAdd( // Memoization
            (currentKey, nextKey, keypads.Count),
            _ => keypads.First()
                .GetPaths(currentKey, nextKey)
                .Min(
                    path =>
                        ShortestSequenceLength(
                            $"{AsKeypadSymbols(path)}A", // Always append 'A' to the end
                            keypads.Skip(1).ToList() // Skip the first keypad
                        )));

    private long Solve(List<string> input, int depth)
    {
        var keypads = Enumerable.Repeat(DirectionalKeypad, depth).Prepend(NumericKeypad).ToList();
        return input.Sum(line => Complexity(line, keypads));
    }

    private sealed class Keypad(Grid<char> grid)
    {
        private readonly ConcurrentDictionary<(char, char), List<Path>> bestPaths = [];

        public static Keypad FromRawString(string input) => new(GridFactory.Create(input.Lines()));

        public List<Path> GetPaths(char currentKey, char nextKey) =>
            bestPaths.GetOrAdd((currentKey, nextKey), _ => FindBestPaths(currentKey, nextKey));

        private List<Path> FindBestPaths(Cell start, Cell goal)
        {
            if (start == goal)
            {
                return [];
            }

            var deltaRow = goal.Row - start.Row;
            var deltaCol = goal.Col - start.Col;

            var rowDirection = deltaRow == 0 ? null : deltaRow > 0 ? Direction.S : Direction.N;
            var colDirection = deltaCol == 0 ? null : deltaCol > 0 ? Direction.E : Direction.W;

            List<Path> result = [];

            foreach (var dir in new[] { rowDirection, colDirection }.OfType<Direction>())
            {
                var next = start.Move(dir);
                if (next == goal)
                {
                    result.Add([dir]);
                }
                else if (grid.ValueAtOrDefault(next) != ' ')
                {
                    result.AddRange(FindBestPaths(next, goal).Select(children => children.Prepend(dir).ToList()));
                }
            }

            return result;
        }

        private List<Path> FindBestPaths(char currentKey, char nextKey)
        {
            if (currentKey == nextKey)
            {
                return [[]]; // Must be a list of empty lists
            }

            var currentCell = grid.Entries.Single(e => e.Value == currentKey).Cell;
            var nextCell = grid.Entries.Single(e => e.Value == nextKey).Cell;
            return FindBestPaths(currentCell, nextCell);
        }
    }
}
