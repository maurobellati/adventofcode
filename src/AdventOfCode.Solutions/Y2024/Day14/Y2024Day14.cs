namespace AdventOfCode.Y2024.Day14;

using Tool;
using Robot = (Cell P, Cell V);

public class Y2024Day14 : Solver
{
    public object PartOne(List<string> input)
    {
        var start = ParseRoom(input);
        var end = Tick(start, 100);
        return end.Robots
            .Select(robot => robot.P)
            .Where(pos => pos.Row != start.Size.Row / 2 && pos.Col != start.Size.Col / 2) // exclude the cells on the middle lines
            .GroupBy(pos => (pos.Row < start.Size.Row / 2, pos.Col < start.Size.Col / 2)) // true for first half, false for second half. Ie: (true, true) is top left, (false, false) is bottom right
            .Select(g => g.Count())
            .Aggregate(1L, (acc, count) => acc * count);
    }

    public object PartTwo(List<string> input)
    {
        var room = ParseRoom(input);
        for (var second = 1; second <= 10_000; second++)
        {
            room = Tick(room, 1);
            var cons = room.Robots.Select(robot => robot.P)
                .OrderBy(it => it.Row).ThenBy(it => it.Col)
                .Aggregate(new ConsecutiveCounter(), (acc, cell) => acc.Count(cell));

            if (cons.LongestStreak >= 10)
            {
                // AnsiConsole.WriteLine($"Found {cons.LongestStreak} at {second}");
                // PrettyPrint(room);
                return second;
            }
        }

        return -1;
    }

    private static Room ParseRoom(List<string> input)
    {
        // 'size=' is not in the main problem, but we need it for tests.
        var sizeConfig = input.FirstOrDefault(line => line.StartsWith("size=", InvariantCulture))?.ExtractInts().ToList();
        var size = new Cell(sizeConfig?[1] ?? 103, sizeConfig?[0] ?? 101);

        var robots = input
            .Where(line => line.StartsWith("p=", InvariantCulture))
            .Select(line => line.ExtractInts().ToList())
            .Select(ints => new Robot(new(ints[1], ints[0]), new(ints[3], ints[2])))
            .ToList();

        return new(size, robots);
    }

    private static void PrettyPrint(Room room)
    {
        var positions = room.Robots.Select(r => r.P).ToHashSet();
        Enumerable.Range(0, room.Size.Row)
            .SelectMany(row => Enumerable.Range(0, room.Size.Col).Select(col => new Cell(row, col)))
            .Select(pos => positions.Contains(pos) ? '#' : '.')
            .Chunk(room.Size.Col)
            .Select(row => string.Join("", row))
            .ToList()
            .ForEach(Console.WriteLine);
    }

    private static Room Tick(Room room, int seconds)
    {
        var robots = room.Robots.Select(robot => robot with { P = (robot.P + (robot.V * seconds)).Mod(room.Size) });
        return room with { Robots = robots.ToList() };
    }

    private sealed class ConsecutiveCounter
    {
        private int currentStreak;

        private Cell lastCell = Cell.Origin;

        public int LongestStreak { get; private set; }

        public ConsecutiveCounter Count(Cell cell)
        {
            if (cell.Col == lastCell.Col + 1)
            {
                currentStreak++;
            }
            else
            {
                currentStreak = 0;
            }

            LongestStreak = Math.Max(LongestStreak, currentStreak);
            lastCell = cell;
            return this;
        }
    }

    private sealed record Room(Cell Size, List<Robot> Robots);
}
