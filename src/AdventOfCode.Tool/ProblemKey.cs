namespace AdventOfCode.Tool;

using System.Globalization;

/// <summary>
///     Identifies a specific problem by year and day
/// </summary>
public readonly record struct ProblemKey(int Year, int Day)
{
    public override string ToString() => $"{Year}/{Day.ToString("D2", CultureInfo.InvariantCulture)}";
}
