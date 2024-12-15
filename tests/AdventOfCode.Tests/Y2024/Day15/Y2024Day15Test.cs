namespace AdventOfCode.Tests.Y2024.Day15;

using AdventOfCode.Y2024.Day15;

public class Y2024Day15Test : SolverTest<Y2024Day15>
{
    private static readonly string Example1 = """
                                              ########
                                              #..O.O.#
                                              ##@.O..#
                                              #...O..#
                                              #.#.O..#
                                              #...O..#
                                              #......#
                                              ########

                                              <^^>>>vv<v>>v<<
                                              """;

    private static readonly string Example2 = """
                                              ##########
                                              #..O..O.O#
                                              #......O.#
                                              #.OO..O.O#
                                              #..O@..O.#
                                              #O#..O...#
                                              #O..O..O.#
                                              #.OO.O.OO#
                                              #....O...#
                                              ##########

                                              <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
                                              vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
                                              ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
                                              <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
                                              ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
                                              ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
                                              >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
                                              <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
                                              ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
                                              v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
                                              """;

    protected override IEnumerable<TestCase> PartOnes() =>
    [
        new(Example1, 2028),
        new(Example2, 10092)
    ];

    protected override IEnumerable<TestCase> PartTwos() => [new(Example2, 9021)];
}
