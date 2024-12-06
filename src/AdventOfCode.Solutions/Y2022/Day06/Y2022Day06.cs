namespace AdventOfCode.Y2022.Day06;

using Tool;

public class Y2022Day06 : Solver
{
    public object PartOne(List<string> input) => Solve(input, 4);

    public object PartTwo(List<string> input) => Solve(input, 14);

    private static object Solve(List<string> input, int size) =>
        input[0]
            .Slide(size)
            .Select((window, index) => (Window: window, Index: index))
            .First(pair => pair.Window.Count == pair.Window.Distinct().Count())
            .Index + size;
}
