namespace AdventOfCode.Tool.Features;

public class SolverFinder(IConfigurationService configService) : ISolverFinder
{
    private readonly List<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies()
        .Reverse()
        .SelectMany(assembly => assembly.GetTypes())
        .ToList();

    private readonly Config config = configService.Load();

    public IEnumerable<ProblemSolver> Find(int? year = null, int? day = null) =>
        ProblemKeys(year, day).Select(TryGetProblemSolver).OfType<ProblemSolver>();

    private static IEnumerable<ProblemKey> ProblemKeys(int? year, int? day) =>
        from y in Enumerable.Range(2015, DateTime.Now.Year - 2015 + 1)
        from d in Enumerable.Range(1, 25)
        where (year is null || y == year) && (day is null || d == day)
        select new ProblemKey(y, d);

    private ProblemSolver? TryGetProblemSolver(ProblemKey key)
    {
        var type = TryGetSolverType(key);
        if (type is null || Activator.CreateInstance(type) is not Solver solver)
        {
            return null;
        }

        return new(key, solver);
    }

    private Type? TryGetSolverType(ProblemKey key) =>
        allTypes.FirstOrDefault(it => it.Namespace == config.ResolveNamespaceName(key) && it.Name == config.ResolveClassName(key));
}
