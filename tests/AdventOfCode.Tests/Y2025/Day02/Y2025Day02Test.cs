namespace AdventOfCode.Tests.Y2025.Day02;

using AdventOfCode.Y2025.Day02;

public class Y2025Day02Test : SolverTest<Y2025Day02>
{
    private static readonly string Input =
        "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 1227775554)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 4174379265)];
}

