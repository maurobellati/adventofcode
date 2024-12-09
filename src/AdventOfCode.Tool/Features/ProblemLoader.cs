namespace AdventOfCode.Tool.Features;

using ErrorOr;

public class ProblemLoader(
    ISolverFinder solverFinder,
    IConfigurationService configService) : IProblemLoader
{
    private readonly Config config = configService.Load();

    public async IAsyncEnumerable<ErrorOr<SolvableProblem>> Load(int? year = null, int? day = null)
    {
        foreach (var problemSolver in solverFinder.Find(year, day))
        {
            yield return await LoadData(problemSolver);
        }
    }

    private async Task<ErrorOr<SolvableProblem>> LoadData(ProblemSolver solver)
    {
        var inputFilePath = config.ResolveInputFilePath(solver.Key);

        if (!File.Exists(inputFilePath))
        {
            return Error.NotFound("Solver.Input.NotFound", $"{solver.Key}: input file {inputFilePath} does not exist");
        }

        var input = await File.ReadAllTextAsync(inputFilePath);

        var answersFilePath = config.ResolveAnswersFilePath(solver.Key);
        var answers = File.Exists(answersFilePath) ? (await File.ReadAllLinesAsync(answersFilePath)).ToList() : [];

        return new SolvableProblem(solver.Key, new(input, answers), solver.Solver);
    }
}
