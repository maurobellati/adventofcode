namespace AdventOfCode.Tests.Y2025.Day04;

using AdventOfCode.Y2025.Day04;

public class Y2025Day04Test : SolverTest<Y2025Day04>
{
    private static readonly string Input = """
                                           ..@@.@@@@.
                                           @@@.@.@.@@
                                           @@@@@.@.@@
                                           @.@@@@..@.
                                           @@.@@@@.@@
                                           .@@@@@@@.@
                                           .@.@.@.@@@
                                           @.@@@.@@@@
                                           .@@@@@@@@.
                                           @.@.@@@.@.
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 13)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 43)];
}
