namespace AdventOfCode.Tool.Features;

using ErrorOr;

public interface IConfigurationService
{
    Config Load();

    void Save(Config config);

    ErrorOr<Config> TryLoad();
}
