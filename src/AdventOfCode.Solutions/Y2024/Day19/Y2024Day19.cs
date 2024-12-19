namespace AdventOfCode.Y2024.Day19;

using Tool;
using static StringSplitOptions;

public class Y2024Day19 : Solver
{
    public object PartOne(List<string> input)
    {
        var patterns = ExtractPatterns(input);
        return ExtractDesigns(input).Count(design => CanBeProduced(design, patterns));
    }

    public object PartTwo(List<string> input)
    {
        var patterns = ExtractPatterns(input);
        return ExtractDesigns(input).Sum(design => CountVariations(design, patterns, new()));
    }

    private static bool CanBeProduced(string design, List<string> patterns) => CountVariations(design, patterns, new()) > 0;

    private static long CountVariations(string target, List<string> patterns, Dictionary<string, long> cache)
        => cache.TryGetValue(target, out var cached)
            ? cached
            : cache[target] = target.IsEmpty()
                ? 1
                : patterns.Where(pattern => target.StartsWith(pattern, InvariantCulture))
                    .Sum(pattern => CountVariations(target[pattern.Length..], patterns, cache));

    private static IEnumerable<string> ExtractDesigns(List<string> input) => input.Skip(2);

    private static List<string> ExtractPatterns(List<string> input) => input.First().Split(",", TrimEntries | RemoveEmptyEntries).ToList();
}
