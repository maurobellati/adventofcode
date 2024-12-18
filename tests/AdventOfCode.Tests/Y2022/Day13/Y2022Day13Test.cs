namespace AdventOfCode.Tests.Y2022.Day13;

using AdventOfCode.Y2022.Day13;

public class Y2022Day13Test : SolverTest<Y2022Day13>
{
    private static readonly string Input = """
                                           [1,1,3,1,1]
                                           [1,1,5,1,1]

                                           [[1],[2,3,4]]
                                           [[1],4]

                                           [9]
                                           [[8,7,6]]

                                           [[4,4],4,4]
                                           [[4,4],4,4,4]

                                           [7,7,7,7]
                                           [7,7,7]

                                           []
                                           [3]

                                           [[[]]]
                                           [[]]

                                           [1,[2,[3,[4,[5,6,7]]]],8,9]
                                           [1,[2,[3,[4,[5,6,0]]]],8,9]
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 13)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 140)];
}
