namespace AdventOfCode.Y2025.Day02;

using Tool;
using static System.Globalization.CultureInfo;

public class Y2025Day02 : Solver
{
    public object PartOne(List<string> input) => Solve(input, InvalidPart01);

    public object PartTwo(List<string> input) => Solve(input, InvalidPart02);

    private static bool AllEquals(List<List<char>> chunks) => chunks.All(chunk => chunk.SequenceEqual(chunks[0]));

    private static bool InvalidPart01(long id)
    {
        // check if a number is made by two identical halves (ie 1212, 3434, 5656, etc)
        var s = id.ToString(InvariantCulture);
        var len = s.Length;
        var half = len / 2;
        return len % 2 == 0 && s[..half] == s[half..];
    }

    private static bool InvalidPart02(long id)
    {
        var s = id.ToString(InvariantCulture);
        return Enumerable.Range(1, s.Length / 2)
            .Where(size => s.Length % size == 0)
            .Select(size => s.Chunk(size).ToList())
            .Where(chunk => chunk.Count > 1)
            .Where(AllEquals)
            .Any();
    }

    private static long Solve(List<string> input, Func<long, bool> invalidPart01) =>
        input.First()
            .Split(',')
            .Select(it => it.Split('-'))
            .Select(pair => (pair[0].ToLong(), pair[1].ToLong()))
            .SelectMany(range => MoreEnumerable.Range(range.Item1, range.Item2).Where(invalidPart01))
            .Sum();
}
