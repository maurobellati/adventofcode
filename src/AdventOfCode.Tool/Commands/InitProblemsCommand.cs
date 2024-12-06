namespace AdventOfCode.Tool.Commands;

using System.ComponentModel;
using ErrorOr;
using Features;
using Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class InitProblemsCommand(IProblemInitializer problemInitializer) : AsyncCommand<InitProblemsCommand.Settings>
{
    private static void WriteErrors(List<Error> errors) =>
        AnsiConsole.MarkupLine($"[red]Error:[/] {errors.First().Description}");

    private static void WriteOk(ProblemKey key) =>
        AnsiConsole.MarkupLine($"[green]Success:[/] {MarkupExtensions.ProblemKey(key)}");

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine($"Setup {MarkupExtensions.YearDay(settings.Year, settings.Day)}");

        await AnsiConsole.Status()
            .StartAsync(
                "Initializing ...",
                async ctx =>
                {
                    await foreach (var result in problemInitializer.Initialize(settings.Year, settings.Day))
                    {
                        result.Switch(WriteOk, WriteErrors);
                        ctx.Refresh();
                    }
                });

        AnsiConsole.MarkupLine("[bold green]Done![/]");
        return 0;
    }

    internal sealed class Settings : CommandSettings
    {
        [Description("The day to initialize")]
        [CommandArgument(1, "[DAY]")]
        public int? Day { get; init; }

        [Description("The year to initialize")]
        [CommandArgument(0, "<YEAR>")]
        public required int Year { get; init; }
    }
}
