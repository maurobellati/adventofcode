namespace AdventOfCode.Y2022.Day20;

using Tool;
using Node = (int Index, long Value);

public class Y2022Day20 : Solver
{
    public object PartOne(List<string> input) => Solve(input);

    public object PartTwo(List<string> input) => Solve(input, 811589153, 10);

    private static long Solve(List<string> input, int scale = 1, int mixCount = 1)
    {
        var nodes = input
            .Select(line => line.ToLong() * scale)
            .Select((value, index) => new Node(index, value)) // Values are not unique, so we need an index to identify them
            .ToList();
        var dest = new List<Node>(nodes);

        while (mixCount-- > 0)
        {
            foreach (var node in nodes)
            {
                if (node.Value == 0)
                {
                    continue;
                }

                var idx = dest.IndexOf(node);
                dest.RemoveAt(idx);

                var newIdx = (idx + node.Value).Mod(dest.Count);
                dest.Insert(newIdx, node);
            }
        }

        var zeroIdx = dest.FindIndex(node => node.Value == 0);

        return dest[(zeroIdx + 1000) % dest.Count].Value +
               dest[(zeroIdx + 2000) % dest.Count].Value +
               dest[(zeroIdx + 3000) % dest.Count].Value;
    }
}
