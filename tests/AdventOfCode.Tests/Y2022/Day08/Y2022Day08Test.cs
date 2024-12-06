namespace AdventOfCode.Tests.Y2022.Day08;

using AdventOfCode.Y2022.Day08;

public class Y2022Day08Test : SolverTest<Y2022Day08>
{
    private static readonly string Input = """
                                           30373
                                           25512
                                           65332
                                           33549
                                           35390
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 21)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 8)];
}
