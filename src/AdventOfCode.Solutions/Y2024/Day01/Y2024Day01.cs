namespace AdventOfCode.Y2024.Day01;

using AdventOfCodeDotNet;

public class Y2024Day01 : Solver
{
    public object PartOne(List<string> input) => LeftList(input).Order().Zip(RightList(input).Order(), (left, right) => Math.Abs(left - right)).Sum();

    public object PartTwo(List<string> input)
    {
        var rightList = RightList(input).ToList();
        return LeftList(input).Select(left => rightList.Count(right => right == left) * left).Sum();
    }

    private static IEnumerable<int> LeftList(List<string> input) => input.Select(line => line.After("   ").ToInt());

    private static IEnumerable<int> RightList(List<string> input) => input.Select(line => line.Before("   ").ToInt());
}
