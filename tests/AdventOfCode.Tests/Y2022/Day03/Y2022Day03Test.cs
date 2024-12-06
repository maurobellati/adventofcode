namespace AdventOfCode.Tests.Y2022.Day03;

using AdventOfCode.Y2022.Day03;

public class Y2022Day03Test : SolverTest<Y2022Day03>
{
    private static readonly string Input = """
                                           vJrwpWtwJgWrhcsFMMfFFhFp
                                           jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
                                           PmmdzqPrVvPwwTWBwg
                                           wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
                                           ttgJtRGJQctTZtZT
                                           CrZsJsPPZsGzwwsLwLmpwMDw
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 157)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 70)];
}
