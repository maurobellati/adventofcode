namespace AdventOfCode.Y2022.Day09;

using Tool;

public class Y2022Day09 : Solver
{
    public object PartOne(List<string> input) => Solve(input, 2);

    public object PartTwo(List<string> input) => Solve(input, 10);

    private static PathStep FollowHead(PathStep head, PathStep tail)
    {
        var diff = head.Cell - tail.Cell;
        var distance = Math.Abs(diff.Row) + Math.Abs(diff.Col);
        return distance switch
        {
            <= 1 => tail,
            2 => diff switch
            {
                { Col: > 0, Row: 0 } => tail.Turn(Direction.E).Move(),
                { Col: < 0, Row: 0 } => tail.Turn(Direction.W).Move(),
                { Col: 0, Row: < 0 } => tail.Turn(Direction.N).Move(),
                { Col: 0, Row: > 0 } => tail.Turn(Direction.S).Move(),
                _ => tail
            },
            _ => diff switch
            {
                { Col: > 0, Row: > 0 } => tail.Turn(Direction.SE).Move(),
                { Col: > 0, Row: < 0 } => tail.Turn(Direction.NE).Move(),
                { Col: < 0, Row: < 0 } => tail.Turn(Direction.NW).Move(),
                { Col: < 0, Row: > 0 } => tail.Turn(Direction.SW).Move(),
                _ => throw new ArgumentOutOfRangeException($"Invalid diff {diff} and distance {distance}")
            }
        };
    }

    private static int Solve(List<string> input, int count)
    {
        var body = Enumerable.Range(0, count).Select(_ => new PathStep(Cell.Origin, Direction.E)).ToList();
        HashSet<PathStep> tailTrace = [];

        foreach (var line in input)
        {
            var direction = line[0] switch
            {
                'U' => Direction.N,
                'R' => Direction.E,
                'D' => Direction.S,
                'L' => Direction.W,
                _ => throw new InvalidOperationException($"Invalid direction {line[0]}")
            };

            var steps = line.After(' ').ToInt();

            for (var s = 0; s < steps; s++)
            {
                body[0] = body[0].Turn(direction).Move();
                for (var t = 0; t < body.Count - 1; t++)
                {
                    body[t + 1] = FollowHead(body[t], body[t + 1]);
                }

                tailTrace.Add(body[^1]);
            }
        }

        return tailTrace.Select(it => it.Cell).Distinct().Count();
    }
}
