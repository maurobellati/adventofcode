namespace AdventOfCode.Y2024.Day03;

using System.Text.RegularExpressions;
using AdventOfCodeDotNet;

public class Y2024Day03 : Solver
{
    private const string MultiplyPattern = @"mul\((?<x>\d{1,3}),(?<y>\d{1,3})\)";
    private const string DoPattern = "do()";
    private const string DonTPattern = "don't()";

    public object PartOne(List<string> input)
    {
        var regex = new Regex(MultiplyPattern);
        return input.SelectMany(line => regex.Matches(line)).Select(Multiply).Sum();
    }

    public object PartTwo(List<string> input)
    {
        var regex = new Regex($"{Regex.Escape(DoPattern)}|{Regex.Escape(DonTPattern)}|{MultiplyPattern}");

        var isMultiplicationEnabled = true;
        var sumOfMultiplications = 0;

        var matches = input.SelectMany(line => regex.Matches(line));
        foreach (var match in matches)
        {
            switch (match.Value)
            {
                case DoPattern:
                    isMultiplicationEnabled = true;
                    break;
                case DonTPattern:
                    isMultiplicationEnabled = false;
                    break;
                default:
                    {
                        if (isMultiplicationEnabled)
                        {
                            sumOfMultiplications += Multiply(match);
                        }

                        break;
                    }
            }
        }

        return sumOfMultiplications;
    }

    private static int Multiply(Match match) => match.Groups["x"].Value.ToInt() * match.Groups["y"].Value.ToInt();
}
