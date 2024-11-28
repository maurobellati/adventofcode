namespace AdventOfCode.Y2023.Day11;

using AdventOfCodeDotNet;

public class Y2023Day11 : Solver
{
    public object PartOne(List<string> input) => Solve(input, 2);

    public object PartTwo(List<string> input) => Solve(input, 1_000_000);

    private static (Galaxy From, Galaxy To, long Distance) CalculateDistance(Galaxy galaxy1, Galaxy galaxy2, List<int> gapsUpToX, List<int> gapsUpToY, int expansion)
    {
        var xGap = Math.Abs(gapsUpToX[galaxy1.X] - gapsUpToX[galaxy2.X]);
        var yGap = Math.Abs(gapsUpToY[galaxy1.Y] - gapsUpToY[galaxy2.Y]);
        var manhattanDistance = Math.Abs(galaxy1.X - galaxy2.X) + Math.Abs(galaxy1.Y - galaxy2.Y);
        var totalGaps = (xGap + yGap) * (expansion - 1);
        var distance = manhattanDistance + totalGaps;
        // Console.WriteLine($"{galaxy2} - {galaxy1} = {manhattanDistance} + {totalGaps} = {totalDistance}");
        return (galaxy1, galaxy2, distance);
    }

    private static List<int> FindGapsUpTo(List<Galaxy> galaxies, Func<Galaxy, int> selector)
    {
        var max = galaxies.Max(selector);
        var gaps = Enumerable.Range(0, max).Except(galaxies.Select(selector)).ToHashSet();
        return Enumerable.Range(0, max + 1).Select(x => gaps.Count(gap => gap < x)).ToList();
    }

    private static IEnumerable<Galaxy> ParseGalaxies(List<string> input) =>
        // the input is a rectangular matrix of '#' and '.'
        // a galaxy is represented by a '#'
        input.Scan(@char => @char == '#').Select(cell => new Galaxy(cell.Row, cell.Col));

    private static long Solve(List<string> input, int expansion)
    {
        // parse file to a set of Galaxies
        var galaxies = ParseGalaxies(input).OrderBy(g => g.Y).ThenBy(g => g.X).ToList();

        // set x and y gaps equals so the all the x and y that are not covered by any galaxy
        var gapsUpToX = FindGapsUpTo(galaxies, it => it.X);
        var gapsUpToY = FindGapsUpTo(galaxies, it => it.Y);

        // compute the manhattan distance between each galaxy, considering a pair of galaxies just once, and add also the number of gaps between them, in both directions (x and y)
        // the manhattan distance is the sum of the absolute values of the differences of the coordinates

        List<(Galaxy From, Galaxy To, long Distance)> distances = [];
        for (var i = 0; i < galaxies.Count - 1; i++)
        {
            for (var j = i + 1; j < galaxies.Count; j++)
            {
                distances.Add(CalculateDistance(galaxies[i], galaxies[j], gapsUpToX, gapsUpToY, expansion));
            }
        }

        return distances.Sum(it => it.Distance);
    }

    private readonly record struct Galaxy(int X, int Y);
}
