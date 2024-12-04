namespace AdventOfCode.Y2024.Day04;

using System.Text.RegularExpressions;
using AdventOfCodeDotNet;

public class Y2024Day04 : Solver
{
    private static readonly Regex XmasRegex = new("XMAS");
    private static readonly Regex SamxRegex = new("SAMX");

    public object PartOne(List<string> input)
    {
        var inputGrid = GridFactory.Create(input);
        var rotations = new[] { inputGrid.Items, inputGrid.Rotate90(Rotation.Clockwise).Items, inputGrid.Rotate45(Rotation.Clockwise), inputGrid.Rotate45(Rotation.CounterClockwise) };
        return rotations.Sum(CountXmases);
    }

    public object PartTwo(List<string> input) => 0;

    private static int CountXmases(List<List<char>> grid) =>
        grid.Select(chars => string.Join("", chars))
            .Sum(line => XmasRegex.Matches(line).Count + SamxRegex.Matches(line).Count);
}
