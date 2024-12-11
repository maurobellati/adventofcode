namespace AdventOfCode.Tests.Y2024.Day11;

using AdventOfCode.Y2024.Day11;

public class Y2024Day11Test : SolverTest<Y2024Day11>
{
    private static readonly string Input = "125 17";

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 55312)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 65601038650482L)];
}
