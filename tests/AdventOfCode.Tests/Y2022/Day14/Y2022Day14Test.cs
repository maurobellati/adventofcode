namespace AdventOfCode.Tests.Y2022.Day14;

using AdventOfCode.Y2022.Day14;

public class Y2022Day14Test : SolverTest<Y2022Day14>
{
    private static readonly string Input = """
                                           498,4 -> 498,6 -> 496,6
                                           503,4 -> 502,4 -> 502,9 -> 494,9
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 24)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 93)];
}
