namespace AdventOfCode.Tests.Y2022.Day19;

using AdventOfCode.Y2022.Day19;

public class Y2022Day19Test : SolverTest<Y2022Day19>
{
    private static readonly string Input =
        """
        Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and  7 obsidian.
        Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and  8 clay. Each geode robot costs 3 ore and 12 obsidian.
        """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 33)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 56 * 62)];
}
