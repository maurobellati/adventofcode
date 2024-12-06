namespace AdventOfCode.Tests.Y2022.Day06;

using AdventOfCode.Y2022.Day06;

public class Y2022Day06Test : SolverTest<Y2022Day06>
{
    protected override IEnumerable<TestCase> PartOnes() =>
    [
        new("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7),
        new("bvwbjplbgvbhsrlpgdmjqwftvncz", 5),
        new("nppdvjthqldpwncqszvftbrmjlhg", 6),
        new("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10),
        new("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)
    ];

    protected override IEnumerable<TestCase> PartTwos() =>
    [
        new("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19),
        new("bvwbjplbgvbhsrlpgdmjqwftvncz", 23),
        new("nppdvjthqldpwncqszvftbrmjlhg", 23),
        new("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29),
        new("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)
    ];
}
