namespace AdventOfCode.Tests.Y2024.Day02;

using AdventOfCode.Y2024.Day02;

public class Y2024Day02Test : SolverTest<Y2024Day02>
{
    private static readonly string Input = """
                                           7 6 4 2 1
                                           1 2 7 8 9
                                           9 7 6 2 1
                                           1 3 2 4 5
                                           8 6 4 4 1
                                           1 3 6 7 9
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 2)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 4)];
}
