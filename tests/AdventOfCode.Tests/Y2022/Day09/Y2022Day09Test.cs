namespace AdventOfCode.Tests.Y2022.Day09;

using AdventOfCode.Y2022.Day09;

public class Y2022Day09Test : SolverTest<Y2022Day09>
{
    protected override IEnumerable<TestCase> PartOnes() =>
    [
        new(
            """
            R 4
            U 4
            L 3
            D 1
            R 4
            D 1
            L 5
            R 2
            """,
            13)
    ];

    protected override IEnumerable<TestCase> PartTwos() =>
    [
        new(
            """
            R 4
            U 4
            L 3
            D 1
            R 4
            D 1
            L 5
            R 2
            """,
            1),
        new(
            """
            R 5
            U 8
            L 8
            D 3
            R 17
            D 10
            L 25
            U 20
            """,
            36)
    ];
}
