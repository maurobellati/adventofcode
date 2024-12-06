namespace AdventOfCode.Tests.Y2022.Day01;

using AdventOfCode.Y2022.Day01;

public class Y2022Day01Test : SolverTest<Y2022Day01>
{
    private static readonly string Input = """
                                           1000
                                           2000
                                           3000

                                           4000

                                           5000
                                           6000

                                           7000
                                           8000
                                           9000

                                           10000
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 24_000)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 45_000)];
}
