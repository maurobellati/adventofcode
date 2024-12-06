namespace AdventOfCode.Tool.Commands;

using System.ComponentModel;
using ErrorOr;
using Features;
using Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class UploadAnswerCommand(IProblemUploader problemUploader) : AsyncCommand<UploadAnswerCommand.Settings>
{
    private static void WriteErrors(List<Error> errors) =>
        AnsiConsole.MarkupLine($"[red]Error:[/] {errors.First().Description}");

    private static void WriteIntro(Settings settings)
    {
        var message = settings.Year is null && settings.Day is null ? "Uploading all problems" : $"Uploading {MarkupExtensions.YearDay(settings.Year, settings.Day)}";
        AnsiConsole.MarkupLine(message);
    }

    private static void WriteOk(string s) =>
        AnsiConsole.MarkupLine($"[green]Success:[/] {s}");

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        WriteIntro(settings);

        await AnsiConsole.Status()
            .StartAsync(
                "Uploading ...",
                async ctx =>
                {
                    await foreach (var result in problemUploader.UploadAsync(settings.Year, settings.Day))
                    {
                        result.Switch(WriteOk, WriteErrors);
                        ctx.Refresh();
                    }
                });

        return 0;
    }

    internal sealed class Settings : CommandSettings
    {
        [Description("The day to upload")]
        [CommandArgument(1, "[DAY]")]
        public int? Day { get; init; }

        [Description("The year to upload")]
        [CommandArgument(0, "[YEAR]")]
        public int? Year { get; init; }
    }
}
