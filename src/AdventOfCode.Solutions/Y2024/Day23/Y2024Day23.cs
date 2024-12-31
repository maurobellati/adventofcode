namespace AdventOfCode.Y2024.Day23;

using Tool;

public class Y2024Day23 : Solver
{
    public object PartOne(List<string> input)
    {
        var setsBySize = FindSets(input, 3);
        return setsBySize[3].Count(set => set.Members.Any(v => v.StartsWith('t')));
    }

    public object PartTwo(List<string> input)
    {
        var sets = FindSets(input);
        var maxSets = sets.Where(kv => kv.Value.Count > 0).MaxBy(kv => kv.Key).Value;
        return maxSets.Single().Members.Order().Join(",");
    }

    private static Dictionary<int, HashSet<LanSet>> FindSets(List<string> input, int? maxSize = null)
    {
        Dictionary<string, HashSet<string>> neighbours = [];
        Dictionary<int, HashSet<LanSet>> result = [];
        foreach (var pair in input.Select(it => (it.Before("-"), it.After("-"))))
        {
            neighbours.Add(pair.Item1, pair.Item2);
            neighbours.Add(pair.Item2, pair.Item1);

            result.Add(2, LanSet.Create([pair.Item1, pair.Item2]));
        }

        var all = neighbours.Keys.ToHashSet();

        var size = 2;
        while (result[size].Count > 0 && !(size >= maxSize))
        {
            var polygons = result[size];
            size++;

            result.Add(size, []);

            foreach (var polygon in polygons)
            {
                var intersections = polygon.Members.Select(v => neighbours[v]).Aggregate(
                    all.ToHashSet(),
                    (acc, set) =>
                    {
                        acc.IntersectWith(set);
                        return acc;
                    });

                foreach (var newVertex in intersections)
                {
                    var newSet = LanSet.Create(polygon.Members.Concat([newVertex]).ToList());
                    if (result[size].Add(newSet))
                    {
                        // Console.WriteLine($"added: {newSet}");
                    }
                }
            }

            // Console.WriteLine($"size: {size,2}, count: {result[size].Count}");
        }

        return result;
    }

    private sealed class LanSet(List<string> members)
    {
        public List<string> Members { get; } = members;

        public static LanSet Create(List<string> input) => new(input.Order().ToList());

        public override bool Equals(object? obj) => obj is LanSet other && Members.SequenceEqual(other.Members);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.AddRange(Members);
            return hash.ToHashCode();
        }

        public override string ToString() => $"[{Members.Join(",")}]";
    }
}
