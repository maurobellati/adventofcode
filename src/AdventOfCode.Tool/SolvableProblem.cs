namespace AdventOfCode.Tool;

/// <summary>
///     Represents a local problem instance, including a solver
/// </summary>
public record SolvableProblem(ProblemKey Key, ProblemData Data, Solver Solver);
