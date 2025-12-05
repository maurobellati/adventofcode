namespace AdventOfCode.Tests.Y2025.Day05;

using AdventOfCode.Y2025.Day05;

public class Y2025Day05Test : SolverTest<Y2025Day05>
{
    private static readonly string Input = """
                                           3-5
                                           10-14
                                           16-20
                                           12-18

                                           1
                                           5
                                           8
                                           11
                                           17
                                           32
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 3)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 14)];
}
