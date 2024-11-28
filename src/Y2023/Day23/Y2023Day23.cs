namespace AdventOfCode.Y2023.Day23;

using AdventOfCodeDotNet;
using Path = Stack<Cell>;

public class Y2023Day23 : Solver
{
    private static readonly char Forest = '#';
    private static readonly char Open = '.';
    private static readonly char SlopeUp = '^';
    private static readonly char SlopeDown = 'v';
    private static readonly char SlopeRight = '>';
    private static readonly char SlopeLeft = '<';

    private static readonly Dictionary<Direction, char> DirToSlope = new()
    {
        [Direction.N] = SlopeUp,
        [Direction.S] = SlopeDown,
        [Direction.E] = SlopeRight,
        [Direction.W] = SlopeLeft
    };

    private static Grid<char> grid;
    private static Cell goal;

    public object PartOne(List<string> input) => SolveWith(input, BuildGetCandidates(true));

    public object PartTwo(List<string> input) => SolveWith(input, BuildGetCandidates(false));

    private static Func<Cell, Path, Grid<char>, IEnumerable<Cell>> BuildGetCandidates(bool considerSlope) =>
        (cell, path, grid) => Direction.GetAll()
            .Select(
                direction => new
                {
                    Direction = direction,
                    Candidate = cell.Move(direction)
                })
            .Where(move => grid.Contains(move.Candidate))
            .Where(move => grid.ValueAt(move.Candidate) != Forest)
            .Where(move => !considerSlope || grid.ValueAt(move.Candidate) == Open || grid.ValueAt(move.Candidate) == DirToSlope[move.Direction])
            .Where(move => !path.Contains(move.Candidate))
            .Select(move => move.Candidate);

    private static void DfsIterative(
        Func<Cell, Path, Grid<char>, IEnumerable<Cell>> getCandidates,
        Action<Path> processSolution,
        Cell start)
    {
        // path is the full path from start to goal
        Path path = new([start]);

        Path choices = new([start]);

        // forks contains just the nodes where there is a choice
        Path forks = new([start]);

        while (choices.Count != 0)
        {
            var currentCell = choices.Pop();
            path.Push(currentCell);

            if (currentCell == goal)
            {
                processSolution(path);
                while (path.Peek() != forks.Peek())
                {
                    path.Pop();
                }
            }

            var candidates = getCandidates(currentCell, path, grid).ToList();
            while (candidates.Count == 1)
            {
                currentCell = candidates[0];
                path.Push(currentCell);
                if (currentCell == goal)
                {
                    processSolution(path);
                    while (path.Peek() != forks.Peek())
                    {
                        path.Pop();
                    }
                }

                candidates = getCandidates(currentCell, path, grid).ToList();
            }

            forks.Push(currentCell);
            foreach (var candidate in candidates)
            {
                choices.Push(candidate);
            }
        }
    }

    private static Cell FindOpenCell(List<string> lines, int row) => new(row, lines[row].IndexOf(Open, StringComparison.Ordinal));

    private static int SolveWith(List<string> input, Func<Cell, Path, Grid<char>, IEnumerable<Cell>> getCandidates)
    {
        grid = GridFactory.Create(input);
        var start = FindOpenCell(input, 0);
        goal = FindOpenCell(input, input.Count - 1);

        Path path = new([start]);

        List<int> solutionLengths = [];
        Action<Path> goalReached = solution =>
        {
            solutionLengths.Add(solution.Count - 1);
            // Console.WriteLine($"Goal reached in {solution.Count} steps");
            // Console.WriteLine($"Path: {string.Join(" -> ", solution)}");
        };

        // DfsRecursive(getCandidates, goalReached, path);
        DfsIterative(getCandidates, goalReached, start);

        return solutionLengths.Max();
    }
}
