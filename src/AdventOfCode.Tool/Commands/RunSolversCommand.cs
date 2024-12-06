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
    private static void AddRow(Table table, SolutionResult result) =>
        table.AddRow(
            new Markup($"{MarkupExtensions.YearDay(result.ProblemInstance.Key.Year, result.ProblemInstance.Key.Day)} [yellow]{Stars(result)}[/]"),
            MarkupResult(result.Part01),
            MarkupResult(result.Part02));

    private static Table InitTable()
    {
        var table = new Table { Width = 120 };
        table.AddColumn("Problem", it => it.Width(20));
        table.AddColumn("Part 01", it => it.Width(50));
        table.AddColumn("Part 02", it => it.Width(50));

        table.Border = TableBorder.Rounded;
        return table;
    }

    private static Markup MarkupResult(PartResult partResult)
    {
        var answer = partResult.Answer.EscapeMarkup();
        return new(
            partResult.ResultType switch
            {
                ResultType.Success => $"[green]{answer}[/]",
                ResultType.Failure => $"[red]{answer}[/] (expected: [green]{partResult.ExpectedAnswer}[/])",
                ResultType.Pending => $"[yellow]{answer}[/]",
                _ => throw new UnreachableException()
            });
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

    private static string Stars(SolutionResult result) => string.Join(string.Empty, result.PartResults.Where(partResult => partResult.ResultType == ResultType.Success).Select(_ => ":glowing_star:"));

    private static void WriteIntro(Settings settings)
    {
        var message = settings.Year is null && settings.Day is null ? "Running all problems" : $"Running {MarkupExtensions.YearDay(settings.Year, settings.Day)}";
        AnsiConsole.MarkupLine(message);
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        WriteIntro(settings);

        var table = InitTable();
        List<Error> allErrors = [];
        await AnsiConsole.Live(table)
            .StartAsync(
                async ctx =>
                {
                    await foreach (var result in solverRunner.Run(settings.Year, settings.Day, settings.Prefix))
                    {
                        result.Switch(
                            ok => AddRow(table, ok),
                            errors => allErrors.Add(errors.First())
                        );
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

        [Description("The prefix to use")]
        [CommandOption("-x|--prefix")]
        public string? Prefix { get; set; }

        // TODO: Add option to run just Part 1 or Part 2

        [Description("The year to run")]
        [CommandArgument(0, "[YEAR]")]
        public int? Year { get; init; }
    }
}
