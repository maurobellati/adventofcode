namespace AdventOfCode.Tests.Y2022.Day12;

using AdventOfCode.Y2022.Day12;

public class Y2022Day12Test : SolverTest<Y2022Day12>
{
    private static readonly string Input = """
                                           Sabqponm
                                           abcryxxl
                                           accszExk
                                           acctuvwj
                                           abdefghi
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 31)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 29)];
}
