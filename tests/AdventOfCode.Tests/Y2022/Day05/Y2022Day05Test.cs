namespace AdventOfCode.Tests.Y2022.Day05;

using AdventOfCode.Y2022.Day05;

public class Y2022Day05Test : SolverTest<Y2022Day05>
{
    private static readonly string Input = """
                                               [D]
                                           [N] [C]
                                           [Z] [M] [P]
                                            1   2   3

                                           move 1 from 2 to 1
                                           move 3 from 1 to 3
                                           move 2 from 2 to 1
                                           move 1 from 1 to 2
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, "CMZ")];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, "MCD")];
}
