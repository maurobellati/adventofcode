namespace AdventOfCode.Tests.Y2025.Day06;

using AdventOfCode.Y2025.Day06;

public class Y2025Day06Test : SolverTest<Y2025Day06>
{
    private static readonly string Input = """
                                           123 328  51 64
                                            45 64  387 23
                                             6 98  215 314
                                           *   +   *   +
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 4277556)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 3263827)];
}
