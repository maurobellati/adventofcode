namespace AdventOfCode.Tests;

using Tool;

public abstract class SolverTest<T> where T : Solver
{
    private static Solver CreateInstance() => Activator.CreateInstance<T>();

    private static void PartTest(IEnumerable<TestCase> testCases, int part)
    {
        var solver = CreateInstance();
        using var mainScope = new AssertionScope();
        foreach (var testCase in testCases)
        {
            using var testScope = new AssertionScope(
                $"""
                 Part {part} with Input:
                 {testCase.Input}

                 """
            );

            solver.Part(testCase.Input.Lines(), part).Should().Be(testCase.Expected);
        }
    }

    protected virtual IEnumerable<TestCase> PartOnes() => [];

    protected virtual IEnumerable<TestCase> PartTwos() => [];

    [Fact]
    protected void PartOne() => PartTest(PartOnes(), 1);

    [Fact]
    protected void PartTwo() => PartTest(PartTwos(), 2);

    protected record TestCase(string Input, object Expected);
}
