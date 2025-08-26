namespace AdventOfCode.Tests.Y2022.Day20;

using AdventOfCode.Y2022.Day20;

public class Y2022Day20Test : SolverTest<Y2022Day20>
{
    private static readonly string Input =
        """
        1
        2
        -3
        3
        -2
        0
        4
        """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 3)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 1623178306)];
}
