namespace AdventOfCode.Tests.Y2023.Day07;

using AdventOfCode.Y2023.Day07;

public class Y2023Day07Test : SolverTest<Y2023Day07>
{
    private static readonly string Input = """
                                           32T3K 765
                                           T55J5 684
                                           KK677 28
                                           KTJJT 220
                                           QQQJA 483
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 6440)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 5905)];
}
