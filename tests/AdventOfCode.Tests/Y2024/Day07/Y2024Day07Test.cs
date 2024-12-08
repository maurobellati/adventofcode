namespace AdventOfCode.Tests.Y2024.Day07;

using AdventOfCode.Y2024.Day07;

public class Y2024Day07Test : SolverTest<Y2024Day07>
{
    private static readonly string Input = """
                                           190: 10 19
                                           3267: 81 40 27
                                           83: 17 5
                                           156: 15 6
                                           7290: 6 8 6 15
                                           161011: 16 10 13
                                           192: 17 8 14
                                           21037: 9 7 18 13
                                           292: 11 6 16 20
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 3749)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 11387)];
}
