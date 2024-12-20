namespace AdventOfCode.Tool.Commands;

using System.ComponentModel;
using System.Diagnostics;
using ErrorOr;
using Features;
using Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class RunSolversCommand(ISolverRunner solverRunner) : AsyncCommand<RunSolversCommand.Settings>
{
    private static string FormatResult(PartResult partResult)
    {
        var answer = partResult.Answer.EscapeMarkup();
        var formatResult = partResult.ResultType switch
        {
            ResultType.Success => $"  :check_mark_button: [green]{answer}[/]",
            ResultType.Pending => $"  :crossed_fingers: [yellow]{answer}[/]",
            ResultType.Failure => $"  :cross_mark: [red]{answer}[/] (expected: [green]{partResult.ExpectedAnswer}[/])",
            _ => throw new UnreachableException()
        };
        return formatResult + $" [grey]({partResult.Duration.TotalMilliseconds}ms)[/]";
    }

    private static void ReportErrors(List<Error> errors)
    {
        if (errors.Count > 0)
        {
            AnsiConsole.MarkupLine("[red]Errors:[/]");
        }

        foreach (var error in errors)
        {
            AnsiConsole.MarkupLine($"[red]- {error.Description}[/]");
        }
    }

    private static void WriteErrors(List<Error> errors) =>
        AnsiConsole.MarkupLine($"[red]  Error:[/] {errors.First().Description}");

    private static void WriteIntro(Settings settings)
    {
        var message = settings.Year is null && settings.Day is null ? "Running all problems" : $"Running {MarkupExtensions.YearDay(settings.Year, settings.Day)}";
        AnsiConsole.MarkupLine(message);
    }

    private static void WriteOk(SolutionResult result)
    {
        var successCount = result.PartResults.Count(partResult => partResult.ResultType == ResultType.Success);
        var stars = new string('*', successCount);
        var key = result.ProblemInstance.Key;

        AnsiConsole.MarkupLine($"[bold]{key.Year} Day {key.Day}[/] [yellow bold]{stars}[/]");
        AnsiConsole.MarkupLine(FormatResult(result.Part01));
        AnsiConsole.MarkupLine(FormatResult(result.Part02));
        AnsiConsole.WriteLine();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        WriteIntro(settings);

        List<Error> allErrors = [];

        await AnsiConsole.Status()
            .AutoRefresh(true)
            .StartAsync(
                "Running ...",
                async ctx =>
                {
                    await foreach (var result in solverRunner.Run(settings.Year, settings.Day, m => ctx.Status(m)))
                    {
                        result.Switch(WriteOk, WriteErrors);
                        ctx.Refresh();
                    }
                });

        ReportErrors(allErrors);

        return 0;
    }

    internal sealed class Settings : CommandSettings
    {
        [Description("The day to run")]
        [CommandArgument(1, "[DAY]")]
        public int? Day { get; init; }

        [Description("The year to run")]
        [CommandArgument(0, "[YEAR]")]
        public int? Year { get; init; }
    }
}
