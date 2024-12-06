namespace AdventOfCode.Y2024.Day05;

using AdventOfCodeDotNet;
using Rule = (int Before, int After);
using Update = List<int>;

public class Y2024Day05 : Solver
{
    public object PartOne(List<string> input)
    {
        var (ordering, updates) = ParsePrintJob(input);
        return updates
            .Where(update => IsSorted(update, ordering))
            .Select(MidItem)
            .Sum();
    }

    public object PartTwo(List<string> input)
    {
        var (ordering, updates) = ParsePrintJob(input);
        return updates
            .Where(update => !IsSorted(update, ordering))
            .Select(update => Sort(update, ordering))
            .Select(MidItem)
            .Sum();
    }

    private static bool IsSorted(Update update, IComparer<int> ordering) => Sort(update, ordering).SequenceEqual(update);

    private static int MidItem(Update update) => update[update.Count / 2];

    private static (IComparer<int> Comparer, List<Update> Updates) ParsePrintJob(List<string> input)
    {
        var rulesAndUpdates = input.Split(string.IsNullOrEmpty).ToList();

        var rules = rulesAndUpdates[0].Select(line => new Rule(line.Before('|').ToInt(), line.After("|").ToInt())).ToList();
        var updates = rulesAndUpdates[1].Select(line => line.ExtractInts().ToList()).ToList();

        var ordering = new RuleBasedComparer(rules);
        return (ordering, updates);
    }

    private static Update Sort(Update update, IComparer<int> ordering) => [..update.OrderBy(x => x, ordering)];

    private class RuleBasedComparer(List<Rule> rules) : IComparer<int>
    {
        public int Compare(int x, int y) => rules.Contains((x, y)) ? -1 : rules.Contains((y, x)) ? 1 : 0;
    }
}
