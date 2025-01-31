namespace AdventOfCode.Tests.Y2024.Day25;

using AdventOfCode.Y2024.Day25;

public class Y2024Day25Test : SolverTest<Y2024Day25>
{
    private static readonly string Input = """
                                           #####
                                           .####
                                           .####
                                           .####
                                           .#.#.
                                           .#...
                                           .....

                                           #####
                                           ##.##
                                           .#.##
                                           ...##
                                           ...#.
                                           ...#.
                                           .....

                                           .....
                                           #....
                                           #....
                                           #...#
                                           #.#.#
                                           #.###
                                           #####

                                           .....
                                           .....
                                           #.#..
                                           ###..
                                           ###.#
                                           ###.#
                                           #####

                                           .....
                                           .....
                                           .....
                                           #....
                                           #.#..
                                           #.#.#
                                           #####
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 3)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 0)];
}
