namespace AdventOfCode.Y2022.Day15;

using Tool;
using ManhattanCircle = (Cell Center, int Radius);
using Scan = (Cell Sensor, Cell Beacon);

public class Y2022Day15 : Solver
{
    public object PartOne(List<string> input)
    {
        var measurements = ParseScans(input).ToList();
        var beacons = measurements.Select(it => it.Beacon);
        var targetRow = GetConfig(input)?.ExtractInts().First() ?? 2000000;

        var scanned = measurements.SelectMany(measurement => ScannedCellsAtRow(measurement, targetRow));
        return scanned.Except(beacons).Count();
    }

    public object PartTwo(List<string> input)
    {
        /*
         * Approaches that do NOT work due to the size of the search area:
         * 1. Find the intersection of all the rings of all the sensors, then remove the intersection of all the rings of all the beacons
         * 2. Brute force: for each cell in the search area, check if it is a beacon
         * 3. Scan all lines in the search area, count the number of scanned cells, and find the one with the least scanned cells

         * Approaches that work:
         * The ONLY hidden beacon is the cell that is not scanned by any sensor, so it must be outside one of the sensors' rings.
         * But since it's only ONE beacon, it must be just ONE cell outside one of the sensors' rings.
         * It also must also live at the intersection of TWO sensors' rings, otherwise there would be more than one beacon.
         * So:
         * 1. For each sensor region, select the Ring of (radius + 1)
         * 2. Pair up the rings of (radius + 1) and intersect them
         * 3. For each cell in those intersection, find the only cell where its distance to all sensors is greater than the sensor's radius.
         */
        var scanCircles = ParseScans(input).Select(scan => new ManhattanCircle(scan.Sensor, Radius(scan))).ToList();
        var area = ParseSearchArea(input);

        var beacon = scanCircles
            .Select(it => new ManhattanCircle(it.Center, it.Radius + 1))
            .GetUnorderedUniquePairs().SelectMany(pair => Intersect(pair.Item1, pair.Item2)) // intersect all pairs
            .Where(area.Contains) // filter by search area
            .First(cell => scanCircles.All(m => m.Center.ManhattanDistance(cell) > m.Radius)); // find the only cell that is not scanned by any sensor
        return (beacon.Col * 4000000L) + beacon.Row;
    }

    private static long Det(long a, long b, long c, long d) => (a * d) - (b * c);

    private static string? GetConfig(List<string> input) => input.SingleOrDefault(line => line.StartsWith("config", InvariantCulture));

    private static IEnumerable<Cell> Intersect(ManhattanCircle x, ManhattanCircle y)
    {
        // Extract properties for easier reference
        var (row1, col1) = (x.Center.Row, x.Center.Col);
        var (row2, col2) = (y.Center.Row, y.Center.Col);
        var r1 = x.Radius;
        var r2 = y.Radius;

        // Equations for edges of Manhattan rigs
        var edgesX = new[]
        {
            (1, 1, row1 + col1 + r1), // x + y = row1 + col1 + r1
            (1, 1, row1 + col1 - r1), // x + y = row1 + col1 - r1
            (1, -1, row1 - col1 + r1), // x - y = row1 - col1 + r1
            (1, -1, row1 - col1 - r1) // x - y = row1 - col1 - r1
        };

        var edgesY = new[]
        {
            (1, 1, row2 + col2 + r2), // x + y = row2 + col2 + r2
            (1, 1, row2 + col2 - r2), // x + y = row2 + col2 - r2
            (1, -1, row2 - col2 + r2), // x - y = row2 - col2 + r2
            (1, -1, row2 - col2 - r2) // x - y = row2 - col2 - r2
        };

        //Solve pairwise linear equations
        foreach (var (a1, b1, c1) in edgesX)
        {
            foreach (var (a2, b2, c2) in edgesY)
            {
                // Check if the two lines intersect
                var determinant = Det(a1, b1, a2, b2);
                if (determinant == 0)
                {
                    continue; // Parallel lines do not intersect
                }

                // Solve for (x, y) intersection
                var xIntersection = Convert.ToInt32(Det(c1, b1, c2, b2) / determinant);
                var yIntersection = Convert.ToInt32(Det(a1, c1, a2, c2) / determinant);

                var intersection = new Cell(xIntersection, yIntersection);

                // Validate the point lies on both Manhattan rings
                if (x.Center.ManhattanDistance(intersection) == r1 && y.Center.ManhattanDistance(intersection) == r2)
                {
                    yield return intersection;
                }
            }
        }
    }

    private static Func<string, bool> IsConfig() => line => line.StartsWith("config", InvariantCulture);

    private static IEnumerable<Scan> ParseScans(List<string> input) =>
        input.TakeWhile(line => !string.IsNullOrEmpty(line))
            .Select(line => line.ExtractInts().ToList())
            .Select(ints => new Scan(new(ints[1], ints[0]), new(ints[3], ints[2]))); // !! x is Col, y is Row

    private static Box ParseSearchArea(List<string> input)
    {
        var searchArea = input.SingleOrDefault(IsConfig())?.ExtractInts().Last() ?? 4_000_000;
        return new(Cell.Origin, new(searchArea, searchArea));
    }

    private static int Radius(Scan scan) => scan.Sensor.ManhattanDistance(scan.Beacon);

    private static IEnumerable<Cell> ScannedCellsAtRow(Scan scan, int row)
    {
        /*
         * -2 ....#....
         * -1 ...###..
         * -1 ..#####..
         *  0 .###S###.
         *  1 ..#####..  <= if vertical distance (row, sensor) is <= radius, there is intersection
         *  2 ...###...
         *  3 ....#....
         *  4 ....r....  <= if vertical distance (row, sensor) is > radius, there is NO intersection
         *
         */

        var sensor = scan.Sensor;
        var distanceFromSensor = Math.Abs(sensor.Row - row);

        var d = Radius(scan) - distanceFromSensor;
        if (d < 0)
        {
            yield break;
        }

        var middle = sensor with { Row = row };
        yield return middle;

        for (var i = 1; i <= d; i++)
        {
            yield return middle with { Col = middle.Col + i };
            yield return middle with { Col = middle.Col - i };
        }
    }
}
