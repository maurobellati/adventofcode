namespace AdventOfCode.Tests.Y2022.Day17;

using AdventOfCode.Y2022.Day17;

public class Y2022Day17Test : SolverTest<Y2022Day17>
{
    private static readonly string Input = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 3068)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 1514285714288)];
}
