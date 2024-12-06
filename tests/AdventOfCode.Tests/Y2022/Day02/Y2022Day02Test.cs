namespace AdventOfCode.Tests.Y2022.Day02;

using AdventOfCode.Y2022.Day02;

public class Y2022Day02Test : SolverTest<Y2022Day02>
{
    private static readonly string Input = """
                                           A Y
                                           B X
                                           C Z
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 15)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 12)];
}
