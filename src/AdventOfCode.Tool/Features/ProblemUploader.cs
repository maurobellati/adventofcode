namespace AdventOfCode.Tool.Features;

using System.Globalization;
using ErrorOr;

public class ProblemUploader(
    IConfigurationService configurationService,
    IHttpClientFactory httpClientFactory,
    ISolverRunner solverRunner)
    : IProblemUploader
{
    private readonly Config config = configurationService.Load();

    public async IAsyncEnumerable<ErrorOr<string>> UploadAsync(int year, int day)
    {
        await foreach (var problem in solverRunner.Run(year, day))
        {
            yield return await problem.ThenAsync(Process);
        }
    }

    private static FormUrlEncodedContent AnswerHttpContent(SolutionResult result) =>
        new(
            (List<KeyValuePair<string, string>>)
            [
                new("level", GetPartIndexToUpload(result).ToString(CultureInfo.InvariantCulture)),
                new("answer", GetPartToUpload(result).Answer!)
            ]);

    private static string AnswerUrl(SolutionResult result) => $"{result.ProblemInstance.Key.Year}/day/{result.ProblemInstance.Key.Day}/answer";

    private static int GetPartIndexToUpload(SolutionResult result) =>
        result.ProblemInstance.Data.Answers.Count + 1;

    private static PartResult GetPartToUpload(SolutionResult result) => result.GetPart(GetPartIndexToUpload(result));

    private static bool IsCorrect(string responseBody) =>
        responseBody.Contains("That's the right answer!", StringComparison.InvariantCulture);

    private static bool IsWrongLevel(string responseBody) =>
        responseBody.Contains("Did you already complete it?", StringComparison.InvariantCulture);

    private static bool TheAnswerIsNull(SolutionResult result) =>
        GetPartToUpload(result).Answer == null;

    private static bool ThePartToUploadIsNotPending(SolutionResult result) =>
        GetPartToUpload(result).ResultType != ResultType.Pending;

    private static bool TheProblemIsAlreadySolved(SolutionResult result) =>
        result.ProblemInstance.Data.Answers.Count == 2;

    private HttpClient CreateHttpClient() => httpClientFactory.CreateClient("AoC-Website");

    private async Task<ErrorOr<string>> Process(SolutionResult result) =>
        await result.ToErrorOr()
            .FailIf(TheProblemIsAlreadySolved, Error.Validation("Problem.AlreadySolved", "It seems the problem has already been answered."))
            .FailIf(ThePartToUploadIsNotPending, Error.Validation("Problem.MustBePending", "The answer to upload must be pending."))
            .FailIf(TheAnswerIsNull, Error.Validation("Problem.NoAnswer", "The answer to upload must not be null."))
            .ThenAsync(UploadAnswer);

    private async Task SaveAnswer(SolutionResult result)
    {
        var answerFile = config.ResolveAnswersFilePath(result.ProblemInstance.Key);
        Console.WriteLine("Saving answer to " + answerFile);
        await File.AppendAllLinesAsync(answerFile, new[] { GetPartToUpload(result).Answer! });
    }

    private async Task<ErrorOr<string>> UploadAnswer(SolutionResult result)
    {
        using var client = CreateHttpClient();
        var responseMessage = await client.PostAsync(AnswerUrl(result), AnswerHttpContent(result));
        var responseBody = await responseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

        if (IsCorrect(responseBody))
        {
            await SaveAnswer(result);
            return "Correct answer!";
        }

        if (IsWrongLevel(responseBody))
        {
            return Error.Failure("Problem.WrongLevel", "You don't seem to be solving the right level.  Did you already complete it?");
        }

        return Error.Failure("Problem.WrongAnswer", "The answer is incorrect.");
    }
}
