namespace AdventOfCode.Tests.Y2025.Day01;

using AdventOfCode.Y2025.Day01;

public class Y2025Day01Test : SolverTest<Y2025Day01>
{
    private static readonly string Input = """
                                           L68
                                           L30
                                           R48
                                           L5
                                           R60
                                           L55
                                           L1
                                           L99
                                           R14
                                           L82
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 3)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 6)];
}
