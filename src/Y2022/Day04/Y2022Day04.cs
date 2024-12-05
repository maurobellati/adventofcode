namespace AdventOfCode.Y2022.Day04;

using AdventOfCodeDotNet;

public class Y2022Day04 : Solver
{
    public object PartOne(List<string> input) => ParseAssignments(input).Count(pair => pair.A.Includes(pair.B) || pair.B.Includes(pair.A));

    public object PartTwo(List<string> input) => ParseAssignments(input).Count(pair => pair.A.Overlaps(pair.B));

    private static IEnumerable<Pair<Range>> ParseAssignments(List<string> input) => input.Select(line => new Pair<Range>(ParseRange(line.Before(",")), ParseRange(line.After(","))));

    private static Range ParseRange(string line) => new(line.Before("-").ToInt(), line.After("-").ToInt());
}
