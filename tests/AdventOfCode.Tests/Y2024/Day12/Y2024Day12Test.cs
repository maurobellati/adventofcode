namespace AdventOfCode.Tests.Y2024.Day12;

using AdventOfCode.Y2024.Day12;

public class Y2024Day12Test : SolverTest<Y2024Day12>
{
    protected override IEnumerable<TestCase> PartOnes() =>
    [
        new(
            """
            RRRRIICCFF
            RRRRIICCCF
            VVRRRCCFFF
            VVRCCCJFFF
            VVVVCJJCFE
            VVIVCCJJEE
            VVIIICJJEE
            MIIIIIJJEE
            MIIISIJEEE
            MMMISSJEEE
            """,
            1930),
        new(
            """
            OOOOO
            OXOXO
            OOOOO
            OXOXO
            OOOOO
            """,
            772)
    ];

    protected override IEnumerable<TestCase> PartTwos() =>
    [
        new(
            """
            AAAA
            BBCD
            BBCC
            EEEC
            """,
            80),
        new(
            """
            OOOOO
            OXOXO
            OOOOO
            OXOXO
            OOOOO
            """,
            436),
        new(
            """
            EEEEE
            EXXXX
            EEEEE
            EXXXX
            EEEEE
            """,
            236),
        new(
            """
            RRRRIICCFF
            RRRRIICCCF
            VVRRRCCFFF
            VVRCCCJFFF
            VVVVCJJCFE
            VVIVCCJJEE
            VVIIICJJEE
            MIIIIIJJEE
            MIIISIJEEE
            MMMISSJEEE
            """,
            1206)
    ];
}
