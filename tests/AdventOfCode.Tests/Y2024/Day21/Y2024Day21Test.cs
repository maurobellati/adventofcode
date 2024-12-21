namespace AdventOfCode.Tests.Y2024.Day21;

using AdventOfCode.Y2024.Day21;

public class Y2024Day21Test : SolverTest<Y2024Day21>
{
    private static readonly string Input = """
                                           029A
                                           980A
                                           179A
                                           456A
                                           379A
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 126384)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 154115708116294L)];
}
