namespace AdventOfCode.Tests.Y2025.Day09;

using AdventOfCode.Y2025.Day09;

public class Y2025Day09Test : SolverTest<Y2025Day09>
{
    private static readonly string Input = """
                                           7,1
                                           11,1
                                           11,7
                                           9,7
                                           9,5
                                           2,5
                                           2,3
                                           7,3
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 50)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 0)];
}
