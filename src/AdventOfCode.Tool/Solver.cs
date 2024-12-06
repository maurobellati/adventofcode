namespace AdventOfCode.Tool;

public interface Solver
{
    public object Part(List<string> input, int part) => part switch
    {
        1 => PartOne(input),
        2 => PartTwo(input),
        _ => throw new ArgumentException("Invalid part number", nameof(part))
    };

    public object PartOne(List<string> input);

    public object PartTwo(List<string> input);
}
