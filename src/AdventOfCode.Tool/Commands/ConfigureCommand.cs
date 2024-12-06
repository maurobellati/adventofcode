namespace AdventOfCode.Tool.Commands;

using Features;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class ConfigureCommand(IConfigurationService configService) : Command<ConfigureCommand.Settings>
{
    public override int Execute(CommandContext context, Settings settings)
    {
        var defaultConfig = configService.TryLoad().Match(ok => ok, _ => new());

        var namespaceTemplate = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the [green]default namespace template[/]:")
                .DefaultValue(defaultConfig.NamespaceName)
                .AllowEmpty());

        var classNameTemplate = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the [green]default class name template[/]:")
                .DefaultValue(defaultConfig.ClassName)
                .AllowEmpty());

        var classPathTemplate = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the [green]default class path template[/]:")
                .DefaultValue(defaultConfig.ClassPath)
                .AllowEmpty());

        var config = new Config
        {
            NamespaceName = namespaceTemplate,
            ClassName = classNameTemplate,
            ClassPath = classPathTemplate
            // DefaultYear = AnsiConsole.Prompt(
            // new TextPrompt<int>("Enter the [green]default year[/] (default: [blue]2023[/]):")
            // .DefaultValue(2023))
        };

        configService.Save(config);

        AnsiConsole.MarkupLine("[green]Configuration file created successfully.[/]");

        return 0;
    }

    internal sealed class Settings : CommandSettings;
}
