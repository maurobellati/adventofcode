namespace AdventOfCode.Tool;

/// <summary>
///     Represents a problem as defined on the website
/// </summary>
public record ProblemDefinition(ProblemKey Key, ProblemData Data);

public enum ResultType
{
    Success,
    Failure,
    Pending
}
