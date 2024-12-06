namespace AdventOfCode.Tool.Features;

using ErrorOr;

public interface IProblemDownloader
{
    // TODO: convert to ErrorOr
    public Task<ErrorOr<ProblemDefinition>> DownloadAsync(ProblemKey key);
}
