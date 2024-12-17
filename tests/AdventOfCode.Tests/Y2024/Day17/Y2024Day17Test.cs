namespace AdventOfCode.Tests.Y2024.Day17;

using AdventOfCode.Y2024.Day17;

public class Y2024Day17Test : SolverTest<Y2024Day17>
{
    protected override IEnumerable<TestCase> PartOnes() =>
    [
        new(
            """
            Register A: 729
            Register B: 0
            Register C: 0

            Program: 0,1,5,4,3,0
            """,
            "4,6,3,5,6,3,5,2,1,0")
    ];

    protected override IEnumerable<TestCase> PartTwos() =>
    [
        new(
            """
            Register A: 2024
            Register B: 0
            Register C: 0

            Program: 0,3,5,4,3,0
            """,
            117440)
    ];
}
