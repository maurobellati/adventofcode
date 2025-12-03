namespace AdventOfCode.Y2025.Day03;

using Tool;
using Battery = (int Digit, int Index);

public class Y2025Day03 : Solver
{
    public object PartOne(List<string> input) => Solve(input, 2);

    public object PartTwo(List<string> input) => Solve(input, 12);

    private static long CalculateJoltage(List<Battery> battery, int resultSize)
    {
        List<int> digits = [];
        var leftIndex = 0;
        for (var i = 0; i < resultSize; i++)
        {
            var rightIndex = battery.Count - resultSize + i + 1;
            var searchRange = battery[leftIndex..rightIndex];
            var item = searchRange.MaxBy(it => it.Digit);
            digits.Add(item.Digit);
            leftIndex = item.Index + 1;
        }

        // build the number from digits: avoid String to Int conversion and parsing
        return digits.Aggregate(0L, (result, digit) => (result * 10) + digit);
    }

    private static List<Battery> ParseBatteryPack(string line) => line.Select((@char, i) => new Battery(@char - '0', i)).ToList();

    private static long Solve(List<string> input, int resultSize) =>
        input
            .Select(ParseBatteryPack)
            .Select(battery => CalculateJoltage(battery, resultSize))
            .Sum();
}
