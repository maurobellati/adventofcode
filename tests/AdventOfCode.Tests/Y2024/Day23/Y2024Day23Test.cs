namespace AdventOfCode.Tests.Y2024.Day23;

using AdventOfCode.Y2024.Day23;

public class Y2024Day23Test : SolverTest<Y2024Day23>
{
    private static readonly string Input = """
                                           kh-tc
                                           qp-kh
                                           de-cg
                                           ka-co
                                           yn-aq
                                           qp-ub
                                           cg-tb
                                           vc-aq
                                           tb-ka
                                           wh-tc
                                           yn-cg
                                           kh-ub
                                           ta-co
                                           de-co
                                           tc-td
                                           tb-wq
                                           wh-td
                                           ta-ka
                                           td-qp
                                           aq-cg
                                           wq-ub
                                           ub-vc
                                           de-ta
                                           wq-aq
                                           wq-vc
                                           wh-yn
                                           ka-de
                                           kh-ta
                                           co-tc
                                           wh-qp
                                           tb-vc
                                           td-yn
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 7)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, "co,de,ka,ta")];
}
