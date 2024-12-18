namespace AdventOfCode.Y2022.Day13;

using System.Text.RegularExpressions;
using Tool;

public partial class Y2022Day13 : Solver
{
    public object PartOne(List<string> input) =>
        input.Split(string.IsNullOrEmpty)
            .Select(lines => lines.ToList())
            .Select((pair, i) => (IsSorted: ComparePackets(pair[0], pair[1]) < 0, Index: i + 1))
            .Where(it => it.IsSorted)
            .Sum(it => it.Index);

    public object PartTwo(List<string> input)
    {
        var dividers = new[] { "[[2]]", "[[6]]" };
        var packets = input.Where(line => !string.IsNullOrEmpty(line)).Append(dividers).ToList();
        packets.Sort(ComparePackets);
        return dividers.Select(it => packets.IndexOf(it) + 1).Aggregate(1, (acc, index) => acc * index);
    }

    private static IEnumerable<string> ExtractElements(string input)
    {
        var i = 1; // Skip the first '['
        while (i < input.Length - 1) // Skip the last ']'
        {
            var item = input[i] == '[' ? ExtractListItem(input[i..]) : ExtractNumberItem(input[i..]);
            i += item.Length + 1;
            yield return item;
        }
    }

    private static string ExtractListItem(string input)
    {
        // For each '[' we find, we need to find a matching ']' that closes it, ignoring any '[' in between
        var endIndex = 0;
        var openCount = 0;

        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == '[')
            {
                openCount++;
            }
            else if (input[i] == ']')
            {
                openCount--;
                if (openCount == 0)
                {
                    endIndex = i;
                    break;
                }
            }
        }

        return input[..(endIndex + 1)];
    }

    private static string ExtractNumberItem(string input) => IntRegex().Match(input).Value;

    [GeneratedRegex(@"^\d+")]
    private static partial Regex IntRegex();

    private static bool IsList(string input) => input.StartsWith('[') && input.EndsWith(']');

    private int CompareList(List<string> left, List<string> right)
    {
        for (var index = 0; index < Math.Max(left.Count, right.Count); index++)
        {
            if (index >= right.Count) // Right side ran out of items, so inputs are not in the right order
            {
                return 1;
            }

            if (index >= left.Count) // Left side ran out of items, so inputs are in the right order
            {
                return -1;
            }

            var compare = ComparePackets(left[index], right[index]);
            if (compare != 0)
            {
                return compare;
            }
        }

        return 0;
    }

    private int ComparePackets(string left, string right) =>
        (IsList(left), IsList(right)) switch
        {
            (true, true) => CompareList(ExtractElements(left).ToList(), ExtractElements(right).ToList()),
            (true, false) => ComparePackets(left, $"[{right}]"),
            (false, true) => ComparePackets($"[{left}]", right),
            _ => left.ToInt().CompareTo(right.ToInt())
        };
}
