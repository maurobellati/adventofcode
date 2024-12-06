namespace AdventOfCode.Tool.Features;

public interface ISolverFinder
{
    public IEnumerable<ProblemSolver> Find(int? year = null, int? day = null);
}
