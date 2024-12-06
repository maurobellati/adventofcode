namespace AdventOfCode.Tool.Features;

using ErrorOr;

public class ProblemInitializer(
    IConfigurationService configService,
    IProblemDownloader problemDownloader) : IProblemInitializer
{
    private readonly Config config = configService.Load();

    public async IAsyncEnumerable<ErrorOr<ProblemKey>> Initialize(int year, int? day = null)
    {
        var keys = day switch
        {
            not null => [new(year, day.Value)],
            _ => Enumerable.Range(1, 25).Select(it => new ProblemKey(year, it))
        };

        foreach (var key in keys)
        {
            yield return await Initialize(key);
        }
    }

    private static async Task<ErrorOr<ProblemKey>> SaveProblemData(ProblemDefinition it, ProblemKey key, Config config)
    {
        var inputPath = config.ResolveInputFilePath(key);
        await File.WriteAllTextAsync(inputPath, it.Data.Input);

        var answersPath = config.ResolveAnswersFilePath(key);
        await File.WriteAllLinesAsync(answersPath, it.Data.Answers);

        return it.Key;
    }

    private async Task<ErrorOr<ProblemKey>> DownloadProblemData(ProblemKey key) =>
        await problemDownloader.DownloadAsync(key).ThenAsync(it => SaveProblemData(it, key, config));

    private async Task<ErrorOr<ProblemKey>> Initialize(ProblemKey key)
    {
        ScaffoldSolver(key);
        return await DownloadProblemData(key);
    }

    private void ScaffoldSolver(ProblemKey key)
    {
        var filePath = config.ResolveSolverFilePath(key);
        if (File.Exists(filePath))
        {
            return;
        }

        var namespaceName = config.ResolveNamespaceName(key);
        var className = config.ResolveClassName(key);
        var fileContent = $$"""
                            namespace {{namespaceName}};

                            using Tool;

                            public class {{className}} : {{nameof(Solver)}}
                            {
                                public object PartOne(List<string> input) => 0;

                                public object PartTwo(List<string> input) => 0;
                            }
                            """;

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, fileContent);
    }
}
