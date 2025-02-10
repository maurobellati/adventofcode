namespace AdventOfCode.Y2024.Day11;

using System.Globalization;
using Tool;

public class Y2024Day11 : Solver
{
    public object PartOne(List<string> input) => CountTotalStonesAfterNBlinks(input, 25);

    public object PartTwo(List<string> input) => CountTotalStonesAfterNBlinks(input, 75);

    private static Dictionary<long, long> ComputeNewStones(Dictionary<long, long> stones)
    {
        Dictionary<long, long> result = [];

        foreach (var (stoneValue, count) in stones)
        {
            var valueAsString = stoneValue.ToString(CultureInfo.InvariantCulture);
            if (stoneValue == 0)
            {
                result.SetOrIncrement(1, count);
                continue;
            }

            if (valueAsString.Length % 2 == 0)
            {
                var left = valueAsString[..(valueAsString.Length / 2)].ToInt();
                result.SetOrIncrement(left, count);

                var right = valueAsString[(valueAsString.Length / 2)..].ToInt();
                result.SetOrIncrement(right, count);
                continue;
            }

            result.SetOrIncrement(stoneValue * 2024, count);
        }

        return result;
    }

    private static long CountTotalStonesAfterNBlinks(List<string> input, int blinks) =>
        Enumerable.Range(0, blinks)
            .Aggregate(InitialStones(input), (current, _) => ComputeNewStones(current))
            .Values
            .Sum();

    private static Dictionary<long, long> InitialStones(List<string> input) =>
        input[0].ExtractLongs().ToDictionary(it => it, _ => 1L);
}
