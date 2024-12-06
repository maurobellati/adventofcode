namespace AdventOfCode.Y2023.Day22;

using System.Collections;
using AdventOfCodeDotNet;

public class Y2023Day22 : Solver
{
    public object PartOne(List<string> input)
    {
        var endGrid = ParseGrid(input).FreeFall();
        return endGrid.Select(brick => CountDiffsWhenRemovingOneBrick(endGrid, brick)).Count(delta => delta == 0);
    }

    public object PartTwo(List<string> input)
    {
        var endGrid = ParseGrid(input).FreeFall();
        return endGrid.Sum(brick => CountDiffsWhenRemovingOneBrick(endGrid, brick));
    }

    private static int CountDiffsWhenRemovingOneBrick(Grid grid, Brick brick)
    {
        // remove brick, so we can try to move something down
        var currentGrid = new Grid(grid.Except([brick]));
        var nextGrid = currentGrid.Tick();
        return currentGrid.CountDiffs(nextGrid);
    }

    private static Brick ParseBrick(string line) => Brick.Create(ParsePoint(line.Before('~')), ParsePoint(line.After('~')));

    private static Grid ParseGrid(List<string> input) => new(input.Select(ParseBrick));

    private static Point ParsePoint(string input)
    {
        var ints = input.ExtractInts().ToList();
        return new(ints[0], ints[1], ints[2]);
    }

    internal sealed class Grid : IEnumerable<Brick>
    {
        private readonly Dictionary<Point, Brick> pointToBrick = [];

        private Grid()
        {
        }

        public Grid(IEnumerable<Brick> bricks)
        {
            foreach (var brick in bricks)
            {
                Add(brick);
            }
        }

        public IEnumerator<Brick> GetEnumerator() => pointToBrick.Values.Distinct().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object? obj)
        {
            if (obj is not Grid other)
            {
                return false;
            }

            var thisSet = pointToBrick.Values.ToHashSet();
            var otherSet = other.pointToBrick.Values.ToHashSet();
            return thisSet.SetEquals(otherSet);
        }

        public override int GetHashCode() => pointToBrick.GetHashCode();

        public int CountDiffs(Grid other)
        {
            var thisSet = pointToBrick.Values.ToHashSet();
            var otherSet = other.pointToBrick.Values.ToHashSet();
            return thisSet.Except(otherSet).Count();
        }

        public Grid FreeFall()
        {
            // move until we can't move anymore
            var current = this;
            var next = current.Tick();
            while (!next.Equals(current))
            {
                current = next;
                next = current.Tick();
            }

            return next;
        }

        public Grid Tick()
        {
            var result = new Grid();
            foreach (var brick in this.OrderBy(brick => brick.A.Z))
            {
                result.Add(result.TryMoveDown(brick));
            }

            return result;
        }

        private void Add(Brick brick) => brick.Points.ForEach(point => pointToBrick[point] = brick);

        private bool AnyOverlaps(Brick brick) => brick.Points.Exists(point => pointToBrick.ContainsKey(point));

        private Brick TryMoveDown(Brick brick)
        {
            var moved = brick.MoveDown(1);
            var anyCollision = moved.A.Z < 1 || moved.B.Z < 1 || AnyOverlaps(moved);
            return anyCollision ? brick : moved;
        }
    }

    internal readonly record struct Brick : IComparable<Brick>, IComparable
    {
        private Brick(Point a, Point b)
        {
            A = a;
            B = b;
            var dx = A.X == B.X ? 0 : 1;
            var dy = A.Y == B.Y ? 0 : 1;
            var dz = A.Z == B.Z ? 0 : 1;

            Points = [A];
            var p = A;
            do
            {
                p = new(p.X + dx, p.Y + dy, p.Z + dz);
                Points.Add(p);
            } while (p != B);
        }

        public Point A { get; }

        public Point B { get; }

        public List<Point> Points { get; }

        public int CompareTo(Brick other) => A.Z.NullableCompareTo(other.A.Z) ?? B.Z.CompareTo(other.B.Z);

        public int CompareTo(object? obj) => CompareTo((Brick)obj!);

        public static Brick Create(Point a, Point b)
        {
            // make sure A is always the lowest point
            var swapped = a.Z > b.Z;
            return new(swapped ? b : a, swapped ? a : b);
        }

        public override string ToString() => $"Brick({A}, {B})";

        public Brick MoveDown(int step) => Create(A.MoveDown(step), B.MoveDown(step));
    }

    internal readonly record struct Point(int X, int Y, int Z)
    {
        public override string ToString() => $"P({X},{Y},{Z})";

        public Point MoveDown(int step) => this with { Z = Z - step };
    }
}
