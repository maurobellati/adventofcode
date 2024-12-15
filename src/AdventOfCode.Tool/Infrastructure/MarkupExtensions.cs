namespace AdventOfCode.Tool.Infrastructure;

using System.Globalization;

public static class MarkupExtensions
{
    public static string ProblemKey(ProblemKey key) => YearDay(key.Year, key.Day);

    public static string YearDay(int? year, int? day) =>
        $"{year} {day?.ToString("D2", CultureInfo.InvariantCulture)}";
}
