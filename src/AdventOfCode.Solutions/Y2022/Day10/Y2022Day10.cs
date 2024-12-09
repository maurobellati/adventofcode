namespace AdventOfCode.Y2022.Day10;

using Tool;

public class Y2022Day10 : Solver
{
    public object PartOne(List<string> input)
    {
        var program = ParseProgram(input);
        return Enumerable.Range(0, 6).Select(i => 20 + (40 * i))
            .Select(cycle => SignalStrength(program, cycle))
            .Sum();
    }

    public object PartTwo(List<string> input)
    {
        var program = ParseProgram(input);
        return Enumerable.Range(1, program.Count)
            .Select(cycle => (Pixel: (cycle - 1) % 40, Sprite: GetSpritePosition(program, cycle)))
            .Select(pos => pos.Sprite - 1 <= pos.Pixel && pos.Pixel <= pos.Sprite + 1)
            .Select(shouldPrint => shouldPrint ? "#" : ".")
            .Chunk(40)
            .Select(row => row.Join())
            .Join(Environment.NewLine);
    }

    private static int GetSpritePosition(List<int> program, int cycle)
    {
        var cycleInstructions = program[..(cycle - 1)];
        return 1 + cycleInstructions.Sum();
    }

    private static List<int> ParseProgram(List<string> input) => input.SelectMany<string, int>(line => line == "noop" ? [0] : [0, line.After(" ").ToInt()]).ToList();

    private static int SignalStrength(List<int> program, int cycle) => cycle * GetSpritePosition(program, cycle);
}
