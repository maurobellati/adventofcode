namespace AdventOfCode.Tests.Y2022.Day18;

using AdventOfCode.Y2022.Day18;

public class Y2022Day18Test : SolverTest<Y2022Day18>
{
    private static readonly string Input = """
                                           2,2,2
                                           1,2,2
                                           3,2,2
                                           2,1,2
                                           2,3,2
                                           2,2,1
                                           2,2,3
                                           2,2,4
                                           2,2,6
                                           1,2,5
                                           3,2,5
                                           2,1,5
                                           2,3,5
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [
        new("1,1,1\n2,1,1", 10),
        new(Input, 64)
    ];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 0)];
}
