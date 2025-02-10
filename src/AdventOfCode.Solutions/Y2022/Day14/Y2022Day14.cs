namespace AdventOfCode.Y2022.Day14;

using Tool;
using static Direction;

public class Y2022Day14 : Solver
{
    public object PartOne(List<string> input)
    {
        var sandbox = BuildSandbox(input);
        FillUntil(sandbox, grain => sandbox.FallOff(grain));
        return sandbox.Grains.Count - 1;
    }

    public object PartTwo(List<string> input)
    {
        var sandbox = BuildSandbox(input);
        FillUntil(sandbox, grain => grain == sandbox.Source);
        return sandbox.Grains.Count;
    }

    private static Sandbox BuildSandbox(List<string> input) => new(input.SelectMany(ParseWall).ToHashSet());

    private static void FillUntil(Sandbox sandbox, Func<Cell, bool> stopCondition)
    {
        Cell grain;
        do
        {
            grain = sandbox.InsertGrain();
        } while (!stopCondition(grain));
    }

    private static IEnumerable<Cell> GetBricks(Cell from, Cell to)
    {
        var start = from > to ? to : from;
        var end = from > to ? from : to;

        return start.Row == end.Row
            ? Enumerable.Range(start.Col, end.Col - start.Col + 1).Select(col => start with { Col = col })
            : Enumerable.Range(start.Row, end.Row - start.Row + 1).Select(row => start with { Row = row });
    }

    private static IEnumerable<Cell> ParseWall(string line) =>
        line.Split(" -> ", StringSplitOptions.TrimEntries)
            .Select(it => new Cell(it.After(",").ToInt(), it.Before(",").ToInt()))
            .Slide(2)
            .SelectMany(pair => GetBricks(pair[0], pair[1]));

    private sealed class Sandbox(ICollection<Cell> walls)
    {
        private readonly int floor = walls.Max(cell => cell.Row) + 2;
        private readonly HashSet<Cell> walls = walls.ToHashSet();

        public HashSet<Cell> Grains { get; } = [];

        internal Cell Source { get; } = new(0, 500);

        public bool FallOff(Cell grain) => grain.Row == floor - 1;

        public Cell InsertGrain()
        {
            var grain = Source;
            do { } while (Move(ref grain, S) || Move(ref grain, SW) || Move(ref grain, SE));

            Grains.Add(grain);
            // Console.WriteLine($"Grain at {grain.Position}");
            return grain;
        }

        private bool Move(ref Cell grain, Direction dir)
        {
            var next = grain.Move(dir);
            var blocked = walls.Contains(next) || Grains.Contains(next) || next.Row == floor;
            var moved = !blocked;
            grain = moved ? next : grain;
            return moved;
        }
    }
}
