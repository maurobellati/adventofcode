namespace AdventOfCode.Tests.Y2022.Day04;

using AdventOfCode.Y2022.Day04;

public class Y2022Day04Test : SolverTest<Y2022Day04>
{
    private static readonly string Input = """
                                           2-4,6-8
                                           2-3,4-5
                                           5-7,7-9
                                           2-8,3-7
                                           6-6,4-6
                                           2-6,4-8
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 2)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 4)];
}
