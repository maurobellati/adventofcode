namespace AdventOfCode.Tool.Features;

using ErrorOr;

public interface IProblemLoader
{
    IAsyncEnumerable<ErrorOr<SolvableProblem>> Load(int? year = null, int? day = null);
}
