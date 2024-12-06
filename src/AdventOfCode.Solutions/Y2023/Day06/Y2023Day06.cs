namespace AdventOfCode.Y2023.Day06;

using AdventOfCodeDotNet;

public class Y2023Day06 : Solver
{
    public object PartOne(List<string> input)
    {
        // Input file is in this format:
        // Time:      7  15   30
        // Distance:  9  40  200
        var times = ParseInput(input[0].Split(":")[1]);
        var currentRecords = ParseInput(input[1].Split(":")[1]);

        var recordBreakCount = times.Zip(currentRecords, BeatRecordCountWithRoots);

        // return multiplication of recordBreakCount
        return recordBreakCount.Aggregate(1L, (x, y) => x * y);
    }

    public object PartTwo(List<string> input)
    {
        var time = input[0].After(":").Strip(" ").ToLong();
        var currentRecord = input[1].After(":").Strip(" ").ToLong();

        return BeatRecordCountWithRoots(time, currentRecord);
    }

    private static int BeatRecordCount(int time, int record)
    {
        var timeToDistances = TimeToDistances(time).ToList();
        var beatRecordCount = timeToDistances.Count(distance => distance > record);
        // Console.WriteLine("With time {0} distances are [{1}]. Can beat the current record ({2}) {3} times", time, string.Join(", ", timeToDistances), record, beatRecordCount);
        return beatRecordCount;
    }

    private static long BeatRecordCountWithRoots(long time, long record)
    {
        // equation is: y = x * (time - x) - record
        // which is: y = - x^2 + time*x - record
        // a=-1, b=time, c=-record
        // delta = b^2 - 4ac = time^2 - 4 * (-1) * (-record) = time^2 - 4 * record
        // roots are: (-b - sqrt(delta)) / 2a and (-b + sqrt(delta)) / 2a
        var deltaSqrt = Math.Sqrt((time * time) - (4 * record));
        var root1 = (time - deltaSqrt) / 2;
        var root2 = (time + deltaSqrt) / 2;

        // get the number of integers strictly included between the roots
        var ceilingRoot1 = (int)Math.Floor(root1 + 1);
        var floorRoot2 = (int)Math.Ceiling(root2 - 1);
        var beatRecordCount = floorRoot2 - ceilingRoot1 + 1;
        // Console.WriteLine(
        //     "Equation: y = - x^2 + {0}x - {1} ==> roots: {2} and {3} ==> {5} - {4} + 1 = {6}",
        //     time,
        //     record,
        //     root1,
        //     root2,
        //     ceilingRoot1,
        //     floorRoot2,
        //     beatRecordCount);
        return beatRecordCount;
    }

    private static IEnumerable<long> ParseInput(string line) =>
        line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(long.Parse);

    private static IEnumerable<int> TimeToDistances(int time) =>
        Enumerable.Range(0, time).Select(
            holdTime =>
            {
                var speed = holdTime;
                var timeLeft = time - holdTime;
                return speed * timeLeft;
            });
}
