namespace AdventOfCode.Y2024.Day15;

using System.Globalization;
using Spectre.Console;
using Tool;

public class Y2024Day15 : Solver
{
    public object PartOne(List<string> input) => Solve(ParseWorld(input), ParseMoves(input));

    public object PartTwo(List<string> input) => Solve(Expand(ParseWorld(input)), ParseMoves(input));

    private static World Expand(World world)
    {
        return new(
            world.Walls.Select(X2).SelectMany(ExpandRight).ToHashSet(),
            world.Boxes.Select(box => new Box(box.Cells.Select(X2).SelectMany(ExpandRight).ToList())).ToList(),
            X2(world.Robot));

        Cell X2(Cell cell) => cell with { Col = cell.Col * 2 };
        IEnumerable<Cell> ExpandRight(Cell cell) => new[] { cell, cell with { Col = cell.Col + 1 } };
    }

    private static List<Direction> ParseMoves(List<string> input) =>
        input.SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1)
            .SelectMany(
                line => line.Select(
                    @char => @char switch
                    {
                        '^' => Direction.N,
                        'v' => Direction.S,
                        '<' => Direction.W,
                        '>' => Direction.E,
                        _ => throw new InvalidOperationException()
                    }))
            .ToList();

    private static World ParseWorld(List<string> input)
    {
        var grid = GridFactory.Create(input.TakeWhile(line => !string.IsNullOrWhiteSpace(line)));
        var robot = grid.Entries.Single(entry => entry.Value == '@').Cell;
        var walls = grid.Entries.Where(e => e.Value == '#').Select(e => e.Cell).ToHashSet();
        var boxes = grid.Entries.Where(e => e.Value == 'O').Select(e => new Box([e.Cell])).ToList();
        return new(walls, boxes, robot);
    }

    private static void PrettyPrint(World world)
    {
        for (var row = 0; row <= world.Rows; row++)
        {
            for (var col = 0; col <= world.Cols; col++)
            {
                var cell = new Cell(row, col);
                var @char = cell switch
                {
                    _ when world.Walls.Contains(cell) => '#',
                    _ when world.Robot == cell => '@',
                    _ when world.Boxes.Any(box => box.Contains(cell)) => 'O',
                    _ => '.'
                };
                AnsiConsole.Write(CultureInfo.InvariantCulture, @char);
            }

            AnsiConsole.WriteLine();
        }
    }

    private long Gps(World world) =>
        world.Boxes
            .Select(box => box.Cells.MinBy(c => c.Row)!)
            .Select(cell => (cell.Row * 100) + cell.Col)
            .Sum();

    private object Solve(World world, List<Direction> directions)
    {
        // Console.WriteLine("Start:");
        // PrettyPrint(world);

        foreach (var dir in directions)
        {
            // Clone and move the robot
            var newWorld = new World(world);
            newWorld.MoveRobot(dir);

            // Check if the new world is valid
            if (!newWorld.HasCollisions())
            {
                world = newWorld;
            }
        }

        // Console.WriteLine("End:");
        // PrettyPrint(world);

        return Gps(world);
    }

    public class World(HashSet<Cell> walls, List<Box> boxes, Cell robot)
    {
        public World(World world) : this(world.Walls.ToHashSet(), world.Boxes.ToList(), new(world.Robot.Row, world.Robot.Col))
        {
        }

        public List<Box> Boxes { get; } = boxes;

        public int Cols { get; } = walls.Max(it => it.Col);

        public Cell Robot { get; private set; } = robot;

        public int Rows { get; } = walls.Max(it => it.Row);

        public HashSet<Cell> Walls { get; } = walls;

        public bool HasCollisions() => Walls.Contains(Robot) || Boxes.Any(box => box.Cells.Any(Walls.Contains));

        public void MoveRobot(Direction dir)
        {
            // Move the robot
            var nextRobot = Robot.Move(dir);

            // Move all boxes that the robot is pushing
            var boxToPushes = Boxes.Where(box => box.Contains(nextRobot)).ToList();
            foreach (var boxToPush in boxToPushes)
            {
                MoveBox(boxToPush, dir);
            }

            Robot = nextRobot;
        }

        private void MoveBox(Box box, Direction dir)
        {
            // Move the box
            Box nextBox = new(box.Cells.Select(b => b.Move(dir)).ToList());

            // Remove the old box
            Boxes.Remove(box);

            // Move all boxes that the box is pushing
            var boxToPushes = Boxes.Where(it => it.Overlaps(nextBox)).ToList();
            foreach (var boxToPush in boxToPushes)
            {
                MoveBox(boxToPush, dir);
            }

            // Add the new box
            Boxes.Add(nextBox);
        }
    }

    public record Box(List<Cell> Cells)
    {
        public bool Contains(Cell cell) => Cells.Contains(cell);

        public bool Overlaps(Box box) => Cells.Any(box.Contains);
    }
}
