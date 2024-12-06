namespace AdventOfCode.Tool.Features;

using System.Text.Json;
using ErrorOr;

public class ConfigurationService : IConfigurationService
{
    private const string ConfigFileName = ".aocconfig";
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

    public Config Load()
    {
        if (!File.Exists(ConfigFileName))
        {
            throw new FileNotFoundException($"Configuration file '{ConfigFileName}' not found.");
        }

        var configJson = File.ReadAllText(ConfigFileName);
        return JsonSerializer.Deserialize<Config>(configJson)
               ?? throw new InvalidOperationException("Failed to parse the configuration file.");
    }

    public void Save(Config config)
    {
        var configJson = JsonSerializer.Serialize(config, JsonSerializerOptions);
        File.WriteAllText(ConfigFileName, configJson);
    }

    public ErrorOr<Config> TryLoad()
    {
        if (!File.Exists(ConfigFileName))
        {
            return Error.NotFound("Config.NotFound", $"Configuration file '{ConfigFileName}' not found.");
        }

        var configJson = File.ReadAllText(ConfigFileName);
        return JsonSerializer.Deserialize<Config>(configJson)?.ToErrorOr() ??
               Error.Failure("Config.Invalid", "Failed to parse the configuration file.");
    }
}
