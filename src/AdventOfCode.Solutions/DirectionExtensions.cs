namespace AdventOfCode;

public static class DirectionExtensions
{
    private static readonly Dictionary<Direction, Direction> Opposites = new()
    {
        { Direction.N, Direction.S },
        { Direction.S, Direction.N },
        { Direction.E, Direction.W },
        { Direction.W, Direction.E },
        { Direction.NE, Direction.SW },
        { Direction.SW, Direction.NE },
        { Direction.SE, Direction.NW },
        { Direction.NW, Direction.SE }
    };

    private static readonly Dictionary<Direction, Dictionary<Rotation, Direction>> Rotations = new()
    {
        [Direction.N] = new()
        {
            [Rotation.Clockwise] = Direction.E,
            [Rotation.CounterClockwise] = Direction.W
        },
        [Direction.S] = new()
        {
            [Rotation.Clockwise] = Direction.W,
            [Rotation.CounterClockwise] = Direction.E
        },
        [Direction.E] = new()
        {
            [Rotation.Clockwise] = Direction.S,
            [Rotation.CounterClockwise] = Direction.N
        },
        [Direction.W] = new()
        {
            [Rotation.Clockwise] = Direction.N,
            [Rotation.CounterClockwise] = Direction.S
        },
        [Direction.NE] = new()
        {
            [Rotation.Clockwise] = Direction.SE,
            [Rotation.CounterClockwise] = Direction.NW
        },
        [Direction.SW] = new()
        {
            [Rotation.Clockwise] = Direction.NW,
            [Rotation.CounterClockwise] = Direction.SE
        },
        [Direction.SE] = new()
        {
            [Rotation.Clockwise] = Direction.SW,
            [Rotation.CounterClockwise] = Direction.NE
        },
        [Direction.NW] = new()
        {
            [Rotation.Clockwise] = Direction.NE,
            [Rotation.CounterClockwise] = Direction.SW
        }
    };

    private static readonly Dictionary<Direction, Cell> ToSteps = new()
    {
        { Direction.N, new(-1, 0) },
        { Direction.S, new(1, 0) },
        { Direction.E, new(0, 1) },
        { Direction.W, new(0, -1) },
        { Direction.NE, new(-1, 1) },
        { Direction.NW, new(-1, -1) },
        { Direction.SE, new(1, 1) },
        { Direction.SW, new(1, -1) }
    };

    public static Direction Opposite(this Direction direction) =>
        Opposites.TryGetValue(direction, out var opposite) ? opposite : throw new ArgumentOutOfRangeException(nameof(direction));

    public static Direction Rotate(this Direction direction, Rotation rotation) =>
        Rotations[direction][rotation];

    public static Cell ToStep(this Direction direction, int size = 1) =>
        ToSteps.TryGetValue(direction, out var step) ? step * size : throw new ArgumentOutOfRangeException(nameof(direction));
}
