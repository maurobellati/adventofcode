namespace AdventOfCode.Y2025.Day01;

using Tool;

public class Y2025Day01 : Solver
{
    private const int Size = 100;
    private const int StartPosition = 50;
    private const int Target = 0;

    private static readonly Dictionary<char, int> Signs = new()
    {
        ['L'] = -1,
        ['R'] = 1
    };

    public object PartOne(List<string> input) =>
        input
            .Select(Parse)
            .Aggregate(
                (Pos: StartPosition, Count: 0),
                (result, rotation) =>
                {
                    var nextPosition = (result.Pos + rotation).Mod(Size);
                    var nextCount = result.Count + (nextPosition == Target ? 1 : 0);
                    return (nextPosition, nextCount);
                })
            .Count;

    public object PartTwo(List<string> input) =>
        input
            .Select(Parse)
            .Aggregate(
                (Pos: StartPosition, Count: 0),
                (result, rotation) =>
                {
                    var rotationCycles = Math.Abs(rotation / Size);
                    var rotationReminder = rotation % Size;

                    var rawPosition = result.Pos + rotationReminder;
                    var crossedZero = result.Pos > 0 && rawPosition is <= 0 or >= Size ? 1 : 0;

                    var nextCount = result.Count + rotationCycles + crossedZero;
                    var nextPosition = rawPosition.Mod(Size);

                    Console.WriteLine($"{rotation,4}: {result.Pos,2} + {rotationReminder,3} = {rawPosition,3} ==> {rotationCycles,2}*cycles + {crossedZero}*crossings = {nextCount,4}");

                    return (nextPosition, nextCount);
                })
            .Count;

    private static int Parse(string line) => Signs[line[0]] * line[1..].ToInt();
}
