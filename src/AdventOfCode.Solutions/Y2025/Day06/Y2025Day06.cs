namespace AdventOfCode.Y2025.Day06;

using Tool;

public class Y2025Day06 : Solver
{
    public object PartOne(List<string> input)
    {
        var grid = input.SkipLast().Select(line => line.ExtractLongs().ToList()).ToList();
        var operators = input.Last().ToCharArray().Where(it => it is not ' ').Select(ParseOp).ToList();

        var problemsCount = grid.First().Count;
        return Enumerable.Range(0, problemsCount)
            .Select(col =>
            {
                var addends = grid.Select(row => row[col]);
                return addends.Aggregate(operators[col]);
            })
            .Sum();
    }

    public object PartTwo(List<string> input)
    {
        var width = input.Max(line => line.Length);

        var grid = input
            .Select(line =>
                    line.PadRight(width, ' ') // so all lines have the same length
                        .ToList() // convert to a list of chars
            ).ToList();

        List<long> results = [];
        List<long> addends = [];
        for (var col = width - 1; col >= 0; col--)
        {
            var addend = grid.SkipLast()
                .Select(row => row[col]) // get char at col
                .Where(@char => @char is not ' ') // skip empty chars
                .Select(@char => (long)(@char - '0')) // parse char to int
                .Aggregate(0L, (result, digit) => (10 * result) + digit);

            addends.Add(addend);

            var operatorChar = grid.Last()[col];
            if (operatorChar is ' ')
            {
                continue;
            }

            var operation = ParseOp(operatorChar);
            var result = addends.Aggregate(operation);

            // Console.WriteLine($"Processing column {col}, operator '{operatorChar}', addends: {string.Join(", ", addends)} = {result}");
            results.Add(result);
            addends.Clear();

            // skip the next column
            col--;
        }

        return results.Sum();
    }

    private static Func<long, long, long> ParseOp(char @char) =>
        @char switch
        {
            '+' => (a, b) => a + b,
            '*' => (a, b) => a * b,
            _ => throw new InvalidOperationException($"Unknown operator: '{@char}'")
        };
}
