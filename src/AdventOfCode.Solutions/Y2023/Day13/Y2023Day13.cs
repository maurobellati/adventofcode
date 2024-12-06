namespace AdventOfCode.Y2023.Day13;

using AdventOfCodeDotNet;
using LanguageExt;

public class Y2023Day13 : Solver
{
    public object PartOne(List<string> input) =>
        ParsePatterns(input)
            .Select(FindSummary)
            .Sum();

    public object PartTwo(List<string> input) => 0;

    private static Option<int> FindHorizontalSummary(Pattern pattern) =>
        FindHorizontalSymmetry(pattern).Map(it => it * 100);

    private static Option<int> FindHorizontalSymmetry(Pattern pattern)
    {
        // starting from line 0 to n-1, consider line n and n+1
        // if they are same, try n-1 and n+2 and so forth, until we find a different line or we reach the end

        for (var row = 0; row < pattern.RowCount - 1; row++)
        {
            if (pattern.Lines[row] == pattern.Lines[row + 1])
            {
                var i = 0;
                while (row - i >= 0 && row + 1 + i < pattern.RowCount)
                {
                    if (pattern.Lines[row - i] != pattern.Lines[row + 1 + i])
                    {
                        break;
                    }

                    i++;
                }

                if (row - i < 0 || row + 1 + i >= pattern.RowCount)
                {
                    // we reached the end, so we found a symmetric pattern
                    return row + 1;
                }
            }
        }

        return Option<int>.None;
    }

    private static int FindSummary(Pattern pattern) =>
        FindHorizontalSummary(pattern).ToNullable() ?? FindVerticalSummary(pattern).ToNullable() ?? throw new ArgumentException("No symmetry found");

    private static Option<int> FindVerticalSummary(Pattern pattern) => FindVerticalSymmetry(pattern);

    private static Option<int> FindVerticalSymmetry(Pattern pattern)
    {
        // starting from column 0 to n-1, consider column n and n+1
        // if they are same, try n-1 and n+2 and so forth, until we find a different column or we reach the end
        for (var col = 0; col < pattern.ColCount - 1; col++)
        {
            if (pattern.Lines.TrueForAll(it => it[col] == it[col + 1]))
            {
                var i = 0;
                while (col - i >= 0 && col + 1 + i < pattern.ColCount)
                {
                    if (pattern.Lines.Any(it => it[col - i] != it[col + 1 + i]))
                    {
                        break;
                    }

                    i++;
                }

                if (col - i < 0 || col + 1 + i >= pattern.ColCount)
                {
                    // we reached the end, so we found a symmetric pattern
                    return col + 1;
                }
            }
        }

        return Option<int>.None;
    }

    private static IEnumerable<Pattern> ParsePatterns(List<string> input) =>
        input.Split(string.IsNullOrWhiteSpace).Select(group => new Pattern(group));
}

internal sealed class Pattern(IEnumerable<string> lines)
{
    public int ColCount => Lines[0].Length;

    public List<string> Lines { get; } = lines.ToList();

    public int RowCount => Lines.Count;
}
