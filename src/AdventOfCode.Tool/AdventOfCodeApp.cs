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
                config.SetApplicationName("AdventOfCode-Cli");

                config.AddCommand<ConfigureCommand>("config");

                config.AddCommand<InitProblemsCommand>("init")
                    .WithExample("init", "year", "2021", "1");

                config.AddCommand<RunSolversCommand>("run")
                    .WithExample("run", "2021", "1");

                config.AddCommand<UploadAnswerCommand>("upload")
                    .WithExample("upload", "2021", "1");
            });
    }

    public int Run(string[] args) => app.Run(args);
}
