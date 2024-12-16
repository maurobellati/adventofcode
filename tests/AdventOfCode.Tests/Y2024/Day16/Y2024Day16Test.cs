namespace AdventOfCode.Tests.Y2024.Day16;

using AdventOfCode.Y2024.Day16;

public class Y2024Day16Test : SolverTest<Y2024Day16>
{
    private static readonly string Example1 = """
                                              ###############
                                              #.......#....E#
                                              #.#.###.#.###.#
                                              #.....#.#...#.#
                                              #.###.#####.#.#
                                              #.#.#.......#.#
                                              #.#.#####.###.#
                                              #...........#.#
                                              ###.#.#####.#.#
                                              #...#.....#.#.#
                                              #.#.#.###.#.#.#
                                              #.....#...#.#.#
                                              #.###.#.#.#.#.#
                                              #S..#.....#...#
                                              ###############
                                              """;

    private static readonly string Example2 = """
                                              #################
                                              #...#...#...#..E#
                                              #.#.#.#.#.#.#.#.#
                                              #.#.#.#...#...#.#
                                              #.#.#.#.###.#.#.#
                                              #...#.#.#.....#.#
                                              #.#.#.#.#.#####.#
                                              #.#...#.#.#.....#
                                              #.#.#####.#.###.#
                                              #.#.#.......#...#
                                              #.#.###.#####.###
                                              #.#.#...#.....#.#
                                              #.#.#.#####.###.#
                                              #.#.#.........#.#
                                              #.#.#.#########.#
                                              #S#.............#
                                              #################
                                              """;

    protected override IEnumerable<TestCase> PartOnes() =>
    [
        new(Example1, 7036),
        new(Example2, 11048)
    ];

    protected override IEnumerable<TestCase> PartTwos() =>
    [
        new(Example1, 45),
        new(Example2, 64)
    ];
}
