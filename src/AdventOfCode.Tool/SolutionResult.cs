namespace AdventOfCode.Tool;

/// <summary>
///     Represents the result of solving a problem
/// </summary>
public record SolutionResult(SolvableProblem ProblemInstance, PartResult Part01, PartResult Part02)
{
    public List<PartResult> PartResults => [Part01, Part02];

    public PartResult GetPart(int part) => part switch
    {
        1 => Part01,
        2 => Part02,
        _ => throw new ArgumentOutOfRangeException(nameof(part))
    };
}
