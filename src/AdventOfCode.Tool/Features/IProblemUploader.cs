namespace AdventOfCode.Tool.Features;

using ErrorOr;

public interface IProblemUploader
{
    public IAsyncEnumerable<ErrorOr<string>> UploadAsync(int year, int day);
}
