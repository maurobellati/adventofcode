namespace AdventOfCode.Tool.Features;

using ErrorOr;

public interface ISolverRunner
{
    IAsyncEnumerable<ErrorOr<SolutionResult>> Run(int? year = null, int? day = null);
}
