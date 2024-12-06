namespace AdventOfCode.Tests.Y2024.Day05;

using AdventOfCode.Y2024.Day05;

public class Y2024Day05Test : SolverTest<Y2024Day05>
{
    private static readonly string Input = """
                                           47|53
                                           97|13
                                           97|61
                                           97|47
                                           75|29
                                           61|13
                                           75|53
                                           29|13
                                           97|29
                                           53|29
                                           61|53
                                           97|53
                                           61|29
                                           47|13
                                           75|47
                                           97|75
                                           47|61
                                           75|61
                                           47|29
                                           75|13
                                           53|13

                                           75,47,61,53,29
                                           97,61,53,29,13
                                           75,29,13
                                           75,97,47,61,53
                                           61,13,29
                                           97,13,75,29,47
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 143)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 123)];
}
