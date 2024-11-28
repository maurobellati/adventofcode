namespace AdventOfCode.Y2023.Day01;

using AdventOfCodeDotNet;

public class Y2023Day01 : Solver
{
    public object PartOne(List<string> input) => input.Select(ExtractFirstAndLastDigits).Sum();

    public object PartTwo(List<string> input) => input.Select(ExtractFirstAndLastWordDigit).Sum();

    private static int ExtractFirstAndLastDigits(string line)
    {
        var firstIndex = line.IndexOfAny("0123456789".ToArray());
        var lastIndex = line.LastIndexOfAny("0123456789".ToArray());
        var first = line[firstIndex];
        var last = line[lastIndex];

        return $"{first}{last}".ToInt();
    }

    private static int ExtractFirstAndLastWordDigit(string line)
    {
        Dictionary<string, int> replacements = new()
        {
            { "0", 0 },
            { "1", 1 },
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 },
            { "zero", 0 },
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };

        var first = replacements
            .Select(kv => (kv.Value, Index: line.IndexOf(kv.Key, StringComparison.CurrentCultureIgnoreCase)))
            .Where(it => it.Index >= 0)
            .MinBy(it => it.Index)
            .Value;
        var last = replacements
            .Select(kv => (kv.Value, Index: line.LastIndexOf(kv.Key, StringComparison.CurrentCultureIgnoreCase)))
            .Where(it => it.Index >= 0)
            .MaxBy(it => it.Index)
            .Value;

        return (first * 10) + last;
    }
}
