namespace AdventOfCode.Y2022.Day11;

using Tool;

public class Y2022Day11 : Solver
{
    public object PartOne(List<string> input) => Solve(input, 20, 3);

    public object PartTwo(List<string> input) => Solve(input, 10_000, 1);

    private static Monkey ParseMonkey(IEnumerable<string> input)
    {
        var lines = input.ToList();
        var id = lines[0].ExtractInts().First();
        var divisibleBy = lines[3].ExtractInts().First();

        var ifTrueMonkeyId = lines[4].ExtractInts().First();
        var ifFalseMonkeyId = lines[5].ExtractInts().First();
        return new(
            id,
            lines[1].ExtractLongs().ToList(),
            Operation,
            divisibleBy,
            ifTrueMonkeyId,
            ifFalseMonkeyId);

        long Operation(long old)
        {
            var opTokens = lines[2].After("new = ").Split(" ");
            var x = opTokens[0] == "old" ? old : opTokens[0].ToLong();
            var y = opTokens[2] == "old" ? old : opTokens[2].ToLong();
            return opTokens[1] switch
            {
                "*" => x * y,
                "+" => x + y,
                _ => throw new InvalidOperationException($"Invalid operation {opTokens[1]}")
            };
        }
    }

    private static long Solve(List<string> input, int rounds, int factor)
    {
        var monkeys = input.Split(string.IsNullOrEmpty).Select(ParseMonkey).ToList();
        var monkeyBuffers = monkeys.ToDictionary(it => it.Id, it => new Queue<long>(it.StartingItems));
        var inspectionStats = monkeys.ToDictionary(it => it.Id, _ => 0);

        // This is just for Part Two
        // To avoid overflow even using longs, we need to calculate the product of all divisors and store the modulo of the evaluation.
        var allDivisors = monkeys.Select(it => it.DivisibleBy).Aggregate(1, (a, b) => a * b);

        for (var roundIndex = 0; roundIndex < rounds; roundIndex++)
        {
            foreach (var monkey in monkeys)
            {
                var bufferIn = monkeyBuffers[monkey.Id];

                while (!bufferIn.IsEmpty())
                {
                    inspectionStats[monkey.Id]++;

                    var old = bufferIn.Dequeue();
                    var monkeyOperation = monkey.Operation(old);
                    var @new = monkeyOperation / factor % allDivisors;

                    var nextMonkeyId = @new % monkey.DivisibleBy == 0 ? monkey.IfTrueMonkeyId : monkey.IfFalseMonkeyId;
                    monkeyBuffers[nextMonkeyId].Enqueue(@new);
                }
            }
        }

        return inspectionStats.Values.Order().Reverse().Take(2).Aggregate(1L, (a, b) => a * b);
    }

    private sealed record Monkey(int Id, List<long> StartingItems, Func<long, long> Operation, int DivisibleBy, int IfTrueMonkeyId, int IfFalseMonkeyId);
}
