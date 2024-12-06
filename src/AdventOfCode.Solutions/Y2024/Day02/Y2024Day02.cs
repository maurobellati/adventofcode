namespace AdventOfCode.Y2024.Day02;

using Tool;
using Report = List<int>;

public class Y2024Day02 : Solver
{
    public object PartOne(List<string> input) => input.Select(ParseReport).Count(IsSafe);

    public object PartTwo(List<string> input) => input.Select(ParseReport).Count(IsSafeWithDampener);

    private static bool IsSafe(Report report)
    {
        /* A report only counts as safe if both of the following are true:
           - The levels are either all increasing or all decreasing.
           - Any two adjacent levels differ by at least one and at most three.
        */
        var diffs = report.Slide(2).Select(group => group[1] - group[0]).ToList();
        return diffs.All(diff => Math.Abs(diff) is >= 1 and <= 3) &&
               diffs.Select(Math.Sign).Distinct().Count() == 1;
    }

    private static bool IsSafeWithDampener(Report report) =>
        // A report is safe even if it contains at most one level that makes it unsafe.
        IsSafe(report) || ReportsWithoutOneElementEach(report).Any(IsSafe);

    private static Report ParseReport(string line) => line.ExtractInts().ToList();

    private static IEnumerable<Report> ReportsWithoutOneElementEach(Report report)
    {
        for (var i = 0; i < report.Count; i++)
        {
            var copy = new Report(report);
            copy.RemoveAt(i);
            yield return copy;
        }
    }
}
