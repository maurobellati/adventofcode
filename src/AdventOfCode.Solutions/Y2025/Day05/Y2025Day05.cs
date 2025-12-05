namespace AdventOfCode.Y2025.Day05;

using Tool;
using Range = (long Start, long End);

public class Y2025Day05 : Solver
{
    public object PartOne(List<string> input)
    {
        var sections = input.Split(line => line.IsBlank()).ToList();
        var ranges = sections[0].Select(ParseRange).ToList();
        var ingredients = sections[1].Select(line => line.ToLong());

        return ingredients.Count(IsFresh());

        Func<long, bool> IsFresh() => it => ranges.Any(r => it.Between(r.Start, r.End));
    }

    public object PartTwo(List<string> input)
    {
        var ranges = input
            .TakeWhile(line => line.IsNotBlank())
            .Select(ParseRange)
            .OrderBy(range => range.Start)
            .ToList();

        List<Range> mergedRanges = [ranges.First()];
        foreach (var range in ranges.Skip(1))
        {
            var last = mergedRanges.Last();
            var rangeAreDetached = last.End < range.Start - 1;

            if (rangeAreDetached)
            {
                mergedRanges.Add(range);
            }
            else
            {
                mergedRanges[^1] = new(last.Start, Math.Max(last.End, range.End));
            }
        }

        var totalSizeCovered = mergedRanges.Sum(range => range.End - range.Start + 1);

        return totalSizeCovered;
    }

    private static Range ParseRange(string line)
    {
        var parts = line.Split('-');
        return new(parts[0].ToLong(), parts[1].ToLong());
    }
}
