namespace AdventOfCode.Y2025.Day08;

using System.Numerics;
using Tool;
using Cluster = HashSet<System.Numerics.Vector3>;

public class Y2025Day08 : Solver
{
    public object PartOne(List<string> input)
    {
        var points = ParsePoints(input);
        var connectionsCount = ExtractConnectionCount(input) ?? 1_000;
        var closestPairs = EnumerateClosestPairs(points).Take(connectionsCount);

        List<Cluster> clusters = [];
        foreach (var pair in closestPairs)
        {
            UpdateClusterMembership(clusters, pair);
        }

        var top3ClusterSizes = clusters
            .OrderByDescending(cluster => cluster.Count)
            .Take(3)
            .Select(cluster => cluster.Count);

        return top3ClusterSizes.Aggregate(1, (a, b) => a * b);
    }

    private static IEnumerable<(Vector3, Vector3)> EnumerateClosestPairs(List<Vector3> points) =>
        points.GetUnorderedUniquePairs()
            .Select(pair => (Pair: pair, Distance: Vector3.DistanceSquared(pair.Item1, pair.Item2)))
            .OrderBy(it => it.Distance)
            .Select(it => it.Pair);

    private static List<Vector3> ParsePoints(List<string> input) =>
        input.TakeWhile(line => line.IsNotBlank())
            .Select(line => line.ExtractInts().ToList())
            .Select(ints => new Vector3(ints[0], ints[1], ints[2]))
            .ToList();

    private static void UpdateClusterMembership(List<Cluster> clusters, (Vector3, Vector3) pair)
    {
        var cluster1 = clusters.Find(it => it.Contains(pair.Item1));
        var cluster2 = clusters.Find(it => it.Contains(pair.Item2));

        switch (cluster1, cluster2)
        {
            case (null, null):
                // neither point is in a cluster yet => create a new cluster
                clusters.Add([pair.Item1, pair.Item2]);
                break;

            case ({ } c1, { } c2) when !ReferenceEquals(c1, c2):
                // both points are in different clusters => merge clusters
                clusters.Remove(c2);
                c1.UnionWith(c2);
                break;

            case (var c1, null):
                // only point 1 is in a cluster => add point 2 to that cluster
                c1.Add(pair.Item2);
                break;

            case (null, var c2):
                // only point 2 is in a cluster => add point 1 to that cluster
                c2.Add(pair.Item1);
                break;
        }
    }

    private int? ExtractConnectionCount(List<string> input) =>
        input[^1].StartsWith("Connections:", OrdinalIgnoreCase) ? input[^1].After(':').ToInt() : null;

    public object PartTwo(List<string> input)
    {
        var points = ParsePoints(input);
        var closestPairs = EnumerateClosestPairs(points).ToList();

        var pairIndex = -1;
        List<Cluster> clusters = [];
        do
        {
            var pair = closestPairs[++pairIndex];
            UpdateClusterMembership(clusters, pair);
        } while (clusters[0].Count < points.Count);

        var lastPair = closestPairs[pairIndex];

        return lastPair.Item1.X * lastPair.Item2.X;
    }
}
