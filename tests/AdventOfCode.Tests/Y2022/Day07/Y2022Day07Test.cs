namespace AdventOfCode.Tests.Y2022.Day07;

using AdventOfCode.Y2022.Day07;

public class Y2022Day07Test : SolverTest<Y2022Day07>
{
    private static readonly string Input = """
                                           $ cd /
                                           $ ls
                                           dir a
                                           14848514 b.txt
                                           8504156 c.dat
                                           dir d
                                           $ cd a
                                           $ ls
                                           dir e
                                           29116 f
                                           2557 g
                                           62596 h.lst
                                           $ cd e
                                           $ ls
                                           584 i
                                           $ cd ..
                                           $ cd ..
                                           $ cd d
                                           $ ls
                                           4060174 j
                                           8033020 d.log
                                           5626152 d.ext
                                           7214296 k
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 95437)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 24933642)];
}
