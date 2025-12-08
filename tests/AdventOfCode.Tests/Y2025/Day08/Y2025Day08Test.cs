namespace AdventOfCode.Tests.Y2025.Day08;

using AdventOfCode.Y2025.Day08;

public class Y2025Day08Test : SolverTest<Y2025Day08>
{
    private static readonly string Input = """
                                           162,817,812
                                           57,618,57
                                           906,360,560
                                           592,479,940
                                           352,342,300
                                           466,668,158
                                           542,29,236
                                           431,825,988
                                           739,650,466
                                           52,470,668
                                           216,146,977
                                           819,987,18
                                           117,168,530
                                           805,96,715
                                           346,949,466
                                           970,615,88
                                           941,993,340
                                           862,61,35
                                           984,92,344
                                           425,690,689

                                           Connections: 10
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 40)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 25272)];
}
