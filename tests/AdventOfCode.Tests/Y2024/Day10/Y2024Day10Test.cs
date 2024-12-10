namespace AdventOfCode.Tests.Y2024.Day10;

using AdventOfCode.Y2024.Day10;

public class Y2024Day10Test : SolverTest<Y2024Day10>
{
    private static readonly string Input = """
                                           89010123
                                           78121874
                                           87430965
                                           96549874
                                           45678903
                                           32019012
                                           01329801
                                           10456732
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 36)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 81)];
}
