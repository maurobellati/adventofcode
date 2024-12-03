namespace AdventOfCode.Y2022.Day01;

using AdventOfCodeDotNet;

public class Y2022Day01 : Solver
{
    public object PartOne(List<string> input) => input.Split(string.IsNullOrEmpty).Select(TotalCalories).Max();

    public object PartTwo(List<string> input) => input.Split(string.IsNullOrEmpty).Select(TotalCalories).OrderDescending().Take(3).Sum();

    private long TotalCalories(IEnumerable<string> foodLines) => foodLines.Select(int.Parse).Sum();
}
