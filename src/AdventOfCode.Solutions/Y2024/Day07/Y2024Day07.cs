namespace AdventOfCode.Y2024.Day07;

using Tool;
using Equation = (long Result, List<long> Numbers);

public class Y2024Day07 : Solver
{
    private const string Sum = "+";
    private const string Mul = "*";
    private const string Concatenate = "||";

    public object PartOne(List<string> input) => GetTotalCalibration(input, [Sum, Mul]);

    public object PartTwo(List<string> input) => GetTotalCalibration(input, [Sum, Mul, Concatenate]);

    private static long Evaluate(IList<string> ops, List<long> numbers)
    {
        var result = numbers[0];
        for (var opIndex = 0; opIndex < ops.Count; opIndex++)
        {
            var op = ops[opIndex];
            var number = numbers[opIndex + 1];
            result = op switch
            {
                Sum => result + number,
                Mul => result * number,
                Concatenate => $"{result}{number}".ToLong(),
                _ => throw new InvalidOperationException($"Invalid operation: {op}")
            };
        }

        return result;
    }

    private static long GetTotalCalibration(List<string> input, List<string> allowedOperations) =>
        input.Select(ParseEquation)
            .Where(it => IsSolvable(it, allowedOperations))
            .Select(it => it.Result)
            .Sum();

    private static bool IsSolvable(Equation equation, List<string> allowedOperations) =>
        allowedOperations.CartesianPower(equation.Numbers.Count - 1)
            .Any(ops => Evaluate(ops, equation.Numbers) == equation.Result);

    private static Equation ParseEquation(string line) => new(line.Before(":").ToLong(), line.After(":").ExtractLongs().ToList());
}
