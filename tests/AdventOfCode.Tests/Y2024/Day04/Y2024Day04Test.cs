namespace AdventOfCode.Tests.Y2024.Day04;

using AdventOfCode.Y2024.Day04;

public class Y2024Day04Test : SolverTest<Y2024Day04>
{
    private static readonly string Input = """
                                           MMMSXXMASM
                                           MSAMXMSMSA
                                           AMXSXMAAMM
                                           MSAMASMSMX
                                           XMASAMXAMM
                                           XXAMMXXAMA
                                           SMSMSASXSS
                                           SAXAMASAAA
                                           MAMMMXMMMM
                                           MXMXAXMASX
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 18)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 9)];
}
