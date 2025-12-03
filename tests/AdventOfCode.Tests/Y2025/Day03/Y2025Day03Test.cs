namespace AdventOfCode.Tests.Y2025.Day03;

using AdventOfCode.Y2025.Day03;

public class Y2025Day03Test : SolverTest<Y2025Day03>
{
    private static readonly string Input = """
                                           987654321111111
                                           811111111111119
                                           234234234234278
                                           818181911112111
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 357)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 3121910778619)];
}
