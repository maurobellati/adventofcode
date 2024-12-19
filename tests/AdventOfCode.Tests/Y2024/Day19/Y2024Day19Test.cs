namespace AdventOfCode.Tests.Y2024.Day19;

using AdventOfCode.Y2024.Day19;

public class Y2024Day19Test : SolverTest<Y2024Day19>
{
    private static readonly string Example = """
                                             r, wr, b, g, bwu, rb, gb, br

                                             brwrr
                                             bggr
                                             gbbr
                                             rrbgbr
                                             ubwu
                                             bwurrg
                                             brgr
                                             bbrgwb
                                             """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Example, 6)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Example, 16)];
}
