namespace AdventOfCode.Tests.Y2024.Day09;

using AdventOfCode.Y2024.Day09;

public class Y2024Day09Test : SolverTest<Y2024Day09>
{
    private static readonly string Input = "2333133121414131402";

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 1928)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 2858)];
}
