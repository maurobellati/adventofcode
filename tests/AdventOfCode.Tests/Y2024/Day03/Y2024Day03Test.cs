namespace AdventOfCode.Tests.Y2024.Day03;

using AdventOfCode.Y2024.Day03;

public class Y2024Day03Test : SolverTest<Y2024Day03>
{
    protected override IEnumerable<TestCase> PartOnes() => [new("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))", 161)];


     protected override IEnumerable<TestCase> PartTwos() => [new("xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))", 48)];
}
