namespace AdventOfCode.Tool;

using Commands;
using Features;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

public class AdventOfCodeApp
{
    private readonly CommandApp app;

    public AdventOfCodeApp()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<IProblemDownloader, ProblemDownloader>();
        services.AddSingleton<IProblemInitializer, ProblemInitializer>();
        services.AddSingleton<IProblemLoader, ProblemLoader>();
        services.AddSingleton<IProblemUploader, ProblemUploader>();
        services.AddSingleton<ISolverFinder, SolverFinder>();
        services.AddSingleton<ISolverRunner, SolverRunner>();

        services.AddHttpClient(
            "AoC-Website",
            (serviceProvider, client) =>
            {
                var config = serviceProvider.GetRequiredService<IConfigurationService>().Load();
                var session = config.Session ?? throw new InvalidOperationException("Session cookie not set");

                client.BaseAddress = new("https://adventofcode.com");
                client.DefaultRequestHeaders.Add("Cookie", $"session={session}");
            });

        var registrar = new TypeRegistrar(services);
        app = new(registrar);

        app.Configure(
            config =>
            {
                // config.SetApplicationName("AdventOfCode-Cli");

                config.AddCommand<ConfigureCommand>("config");

                config.AddCommand<InitProblemsCommand>("init")
                    .WithDescription("Initialize problems for a given year and day")
                    .WithExample("init", "2024")
                    .WithExample("init", "2024", "1");

                config.AddCommand<RunSolversCommand>("run")
                    .WithDescription("Run solvers for a given year and day")
                    .WithExample("run", "2024")
                    .WithExample("run", "2024", "1");

                config.AddCommand<UploadAnswerCommand>("upload")
                    .WithDescription("Upload the most pending answer for a given year and day")
                    .WithExample("upload", "2021", "1");
            });
    }

    public int Run(string[] args) => app.Run(args);
}
