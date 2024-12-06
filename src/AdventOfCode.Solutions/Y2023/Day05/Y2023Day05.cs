namespace AdventOfCode.Y2023.Day05;

using Tool;

public class Y2023Day05 : Solver
{
    public object PartOne(List<string> input)
    {
        // first line has format: "seeds: 1 2 3 4 5"
        var seeds = input[0].ExtractLongs();
        var maps = ParseMaps(input);
        return seeds.Select(seed => SeedToLocation(maps, seed)).Min();
    }

    public object PartTwo(List<string> input)
    {
        // first line has format: "seeds: 1 2 3 4 5 6"
        // seeds must be considered in pairs or Range: (1,2) (3,4) (5,6)
        // chunk seeds into pairs
        var seeds = input[0].ExtractLongs();
        var maps = ParseMaps(input);
        IEnumerable<Range> ranges = seeds.Chunk(2).Select(x => Range.FromStartAndLength(x[0], x[1])).ToList();

        var finalRanges = maps.Aggregate(ranges, (stepRanges, map) => stepRanges.SelectMany(map.Split));
        return finalRanges.MinBy(range => range.From)?.From ?? 0;
    }

    private static List<Map> ParseMaps(List<string> lines)
    {
        List<Map> result = [];
        // Iterate starting from the 3rd line
        var i = 2;
        while (i < lines.Count)
        {
            // if line has format: "SOURCE-to-DESTINATION map:"
            if (lines[i].EndsWith("map:", StringComparison.InvariantCulture))
            {
                var label = lines[i];

                // iterate over the next lines until an empty line is found
                List<Rule> rules = [];
                i++;
                while (i < lines.Count && !string.IsNullOrEmpty(lines[i]))
                {
                    var ruleParts = lines[i].ExtractLongs().ToList();
                    rules.Add(new(ruleParts[1], ruleParts[0], ruleParts[2]));
                    i++;
                }

                result.Add(new(label, rules));
                continue;
            }

            i++;
        }

        return result;
    }

    private static long SeedToLocation(IEnumerable<Map> maps, long seed) => maps.Aggregate(seed, (value, map) => map.Lookup(value));

    public record Map(string Label, List<Rule> Rules)
    {
        public override string ToString() => $"Map({Label}, Rules=[{string.Join(", ", Rules)}])";

        public long Lookup(long input)
        {
            var rule = Rules.Find(rule => rule.IsMatch(input));
            return rule?.Lookup(input) ?? input;
        }

        public IEnumerable<Range> Split(Range input)
        {
            List<Range> allUnmapped = [input];
            List<Range> allMapped = [];

            foreach (var rule in Rules)
            {
                var range = allUnmapped.Find(range => rule.SourceRange.Overlaps(range));
                if (range is null)
                {
                    continue;
                }

                allUnmapped.Remove(range);

                var (mapped, unmapped) = rule.Split(range);

                allUnmapped.AddRange(unmapped);

                if (mapped is not null)
                {
                    allMapped.Add(mapped);
                }
            }

            return allMapped.Concat(allUnmapped);
        }
    }

    public class Rule
    {
        public Rule(long sourceStart, long destinationStart, long length)
        {
            Shift = destinationStart - sourceStart;
            Length = length;
            SourceRange = Range.FromStartAndLength(sourceStart, length);
            DestinationRange = Range.FromStartAndLength(destinationStart, length);
        }

        public Range DestinationRange { get; }

        public long Length { get; init; }

        public long Shift { get; }

        public Range SourceRange { get; }

        public bool IsMatch(long input) => SourceRange.Contains(input);

        public long Lookup(long input) => Shift + input;

        public (Range? Mapped, List<Range> Unmapped) Split(Range input)
        {
            if (!SourceRange.Overlaps(input))
            {
                return (null, []);
            }

            var sourceIntersection = input.Intersect(SourceRange);

            List<Range> unmapped = [];
            if (input.To > sourceIntersection.To)
            {
                unmapped.Add(input with { From = sourceIntersection.To + 1 });
            }

            if (input.From < sourceIntersection.From)
            {
                unmapped.Add(input with { To = sourceIntersection.From - 1 });
            }

            var destinationMapped = sourceIntersection.Shift(Shift);
            return (destinationMapped, unmapped);
        }
    }
}
