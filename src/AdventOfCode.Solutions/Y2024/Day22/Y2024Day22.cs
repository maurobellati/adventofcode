namespace AdventOfCode.Y2024.Day22;

using Tool;

public class Y2024Day22 : Solver
{
    public object PartOne(List<string> input) =>
        input.Select(line => line.ToInt())
            .Select(number => Next(number, 2000).Last())
            .Sum();

    public object PartTwo(List<string> input) =>
        input.Select(line => line.ToInt())
            .SelectMany(PriceChanges)
            .GroupBy(
                it => it.Changes,
                it => it.Price,
                (_, prices) => prices.Sum())
            .Max();

    public static IEnumerable<long> Next(long number, int nth)
    {
        for (var i = 0; i < nth; i++)
        {
            number = Next(number);
            yield return number;
        }
    }

    private static long Next(long number)
    {
        // To mix a value into the secret number, calculate the bitwise XOR of the given value and the secret number.
        // To prune the secret number, calculate the value of the secret number modulo 16777216.

        // We use (PruneNumber -1) because it's a power of 2, so we can use a bitwise AND to prune the number instead of using the modulo operator.
        const int PruneNumberMinusOne = 16777216 - 1;

        // 1. Calculate the result of multiplying the secret number by 64. Then, mix this result into the secret number. Finally, prune the secret number.
        var result = number ^ (number * 64);
        result &= PruneNumberMinusOne;

        // 2. Calculate the result of dividing the secret number by 32. Round the result down to the nearest integer. Then, mix this result into the secret number. Finally, prune the secret number.
        result ^= result / 32;
        result &= PruneNumberMinusOne;

        // 3. Calculate the result of multiplying the secret number by 2048. Then, mix this result into the secret number. Finally, prune the secret number.
        result ^= result * 2048;
        result &= PruneNumberMinusOne;

        return result;
    }

    private static IEnumerable<(long Price, string Changes)> PriceChanges(int number)
    {
        var digits = Next(number, 2000).Select(num => num % 10).ToList();
        var digitsAndChanges = digits.Zip(digits.Skip(1), (a, b) => (Digit: b, Diff: b - a));
        return digitsAndChanges
            .Slide(4)
            .Select(
                slide => (
                    Price: slide.Last().Digit,
                    Changes: slide.Select(it => it.Diff).Join(",")
                )
            )
            .DistinctBy(it => it.Changes); // pick just the first occurrence of each distinct change
    }
}
