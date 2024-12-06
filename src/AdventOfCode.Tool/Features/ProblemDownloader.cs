namespace AdventOfCode.Tool.Features;

using System.Text.RegularExpressions;
using ErrorOr;

public partial class ProblemDownloader(
    IHttpClientFactory httpClientFactory
) : IProblemDownloader
{
    public async Task<ErrorOr<ProblemDefinition>> DownloadAsync(ProblemKey key)
    {
        using var client = CreateHttpClient();

        var answers = await DownloadAnswers(key, client);
        var input = await DownloadProblemInput(key, client);

        return new ProblemDefinition(key, new(input, answers));
    }

    [GeneratedRegex(@"Your puzzle answer was\s+<code>(?<Answer>.*?)</code>", RegexOptions.IgnoreCase)]
    private static partial Regex AnswerRegex();

    private static async Task<List<string>> DownloadAnswers(ProblemKey key, HttpClient client)
    {
        var content = await DownloadProblemContent(key, client);
        return ExtractAnswers(content);
    }

    private static async Task<string> DownloadProblemContent(ProblemKey key, HttpClient client) =>
        await client.GetStringAsync($"{key.Year}/day/{key.Day}");

    private static async Task<string> DownloadProblemInput(ProblemKey key, HttpClient client) =>
        await client.GetStringAsync($"{key.Year}/day/{key.Day}/input");

    private static List<string> ExtractAnswers(string content) =>
        AnswerRegex().Matches(content)
            .Select(match => match.Groups["Answer"].Value)
            .ToList();

    private HttpClient CreateHttpClient() => httpClientFactory.CreateClient("AoC-Website");
}
