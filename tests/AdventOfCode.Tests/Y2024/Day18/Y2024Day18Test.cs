namespace AdventOfCode.Tests.Y2024.Day18;

using AdventOfCode.Y2024.Day18;

public class Y2024Day18Test : SolverTest<Y2024Day18>
{
    private static readonly string Example = """
                                             config: memorySize=6 corruptionsCount=12
                                             5,4
                                             4,2
                                             4,5
                                             3,0
                                             2,1
                                             6,3
                                             2,4
                                             1,5
                                             0,6
                                             3,3
                                             2,6
                                             5,1
                                             1,2
                                             5,5
                                             2,5
                                             6,5
                                             1,4
                                             0,4
                                             6,4
                                             1,1
                                             6,1
                                             1,0
                                             0,5
                                             1,6
                                             2,0
                                             """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Example, 22)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Example, "6,1")];
}
