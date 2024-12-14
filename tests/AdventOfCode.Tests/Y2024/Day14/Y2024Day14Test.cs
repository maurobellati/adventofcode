namespace AdventOfCode.Tests.Y2024.Day14;

using AdventOfCode.Y2024.Day14;

public class Y2024Day14Test : SolverTest<Y2024Day14>
{
    private static readonly string Input = """
                                           size=11,7
                                           p=0,4 v=3,-3
                                           p=6,3 v=-1,-3
                                           p=10,3 v=-1,2
                                           p=2,0 v=2,-1
                                           p=0,0 v=1,3
                                           p=3,0 v=-2,-2
                                           p=7,6 v=-1,-3
                                           p=3,0 v=-1,-2
                                           p=9,3 v=2,3
                                           p=7,3 v=-1,2
                                           p=2,4 v=2,-3
                                           p=9,5 v=-3,-3
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 12)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, -1)];
}
