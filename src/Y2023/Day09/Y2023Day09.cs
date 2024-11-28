namespace AdventOfCode.Y2023.Day09;

using AdventOfCodeDotNet;

public class Y2023Day09 : Solver
{
    public object PartOne(List<string> input) =>
        input
            .Select(ParseInputAsLongs)
            .Select(PredictNextNumber)
            .Select(list => list.Last())
            .Sum();

    public object PartTwo(List<string> input) =>
        input
            .Select(ParseInputAsLongs)
            .Select(PredictPreviousNumber)
            .Select(list => list.First())
            .Sum();

    private static List<long> ComputeDiffs(IReadOnlyCollection<long> input) =>
        input.Zip(input.Skip(1), (a, b) => b - a).ToList();

    private static List<long> ParseInputAsLongs(string line) => line.ExtractLongs().ToList();

    private static List<long> PredictNextNumber(List<long> input)
    {
        var next = input.Exists(it => it != 0) ? input.Last() + PredictNextNumber(ComputeDiffs(input)).Last() : 0L;
        input.Add(next);
        // Console.WriteLine($"{string.Join(", ", input)} ===> {string.Join(", ", input)}");
        return input;
    }

    private static List<long> PredictPreviousNumber(List<long> input)
    {
        var next = input.Exists(it => it != 0) ? input.First() - PredictPreviousNumber(ComputeDiffs(input)).First() : 0L;
        input.Insert(0, next);
        // Console.WriteLine($"{string.Join(", ", input)} ===> {string.Join(", ", input)}");
        return input;
    }
}
