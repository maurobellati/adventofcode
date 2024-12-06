namespace AdventOfCode.Tool.Features;

using ErrorOr;

public interface IProblemInitializer
{
    IAsyncEnumerable<ErrorOr<ProblemKey>> Initialize(int year, int? day = null);
}
