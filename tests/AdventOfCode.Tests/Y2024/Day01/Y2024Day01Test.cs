namespace AdventOfCode.Tests.Y2024.Day01;

using AdventOfCode.Y2024.Day01;

public class Y2024Day01Test : SolverTest<Y2024Day01>
{
    private static readonly string Input = """
                                           3   4
                                           4   3
                                           2   5
                                           1   3
                                           3   9
                                           3   3
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 11)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 31)];
}
