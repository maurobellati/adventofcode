namespace AdventOfCode.Tests.Y2022.Day15;

using AdventOfCode.Y2022.Day15;

public class Y2022Day15Test : SolverTest<Y2022Day15>
{
    private static readonly string Input = """
                                           Sensor at x=8, y=7: closest beacon is at x=2, y=10
                                           Sensor at x=2, y=18: closest beacon is at x=-2, y=15
                                           Sensor at x=9, y=16: closest beacon is at x=10, y=16
                                           Sensor at x=13, y=2: closest beacon is at x=15, y=3
                                           Sensor at x=12, y=14: closest beacon is at x=10, y=16
                                           Sensor at x=10, y=20: closest beacon is at x=10, y=16
                                           Sensor at x=14, y=17: closest beacon is at x=10, y=16
                                           Sensor at x=2, y=0: closest beacon is at x=2, y=10
                                           Sensor at x=0, y=11: closest beacon is at x=2, y=10
                                           Sensor at x=20, y=14: closest beacon is at x=25, y=17
                                           Sensor at x=17, y=20: closest beacon is at x=21, y=22
                                           Sensor at x=16, y=7: closest beacon is at x=15, y=3
                                           Sensor at x=14, y=3: closest beacon is at x=15, y=3
                                           Sensor at x=20, y=1: closest beacon is at x=15, y=3

                                           config: y=10 search-up-to=20
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 26)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 56000011)];
}
