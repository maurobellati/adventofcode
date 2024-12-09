namespace AdventOfCode.Tool.Features;

using System.Globalization;

internal static class ConfigurationExtensions
{
    internal static string ResolveAnswersFilePath(this Config config, ProblemKey key) =>
        Path.Combine(config.ResolvePath(key), "answers.txt");

    internal static string ResolveClassName(this Config config, ProblemKey key) =>
        config.ClassName.Replace(key);

    internal static string ResolveInputFilePath(this Config config, ProblemKey key) =>
        Path.Combine(config.ResolvePath(key), "input.txt");

    internal static string ResolveNamespaceName(this Config config, ProblemKey key) =>
        config.NamespaceName.Replace(key);

    internal static string ResolveSolverFilePath(this Config config, ProblemKey key) =>
        Path.Combine(config.ResolvePath(key), $"{config.ResolveClassName(key)}.cs");

    private static string Replace(this string source, ProblemKey key) =>
        source.ReplaceYearAndDay(key.Year.ToString("D4", CultureInfo.InvariantCulture), key.Day.ToString("D2", CultureInfo.InvariantCulture));

    private static string ReplaceYearAndDay(this string source, string year, string day) =>
        source.Replace(Config.YearPlaceholder, year, StringComparison.Ordinal)
            .Replace(Config.DayPlaceholder, day, StringComparison.Ordinal);

    private static string ResolvePath(this Config config, ProblemKey key) =>
        config.ClassPath.Replace(key);
}
