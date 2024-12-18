namespace AdventOfCode;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

public static partial class StringExtensions
{
    public static string After(this string input, char value, StringComparison comparisonType = Ordinal)
    {
        var indexOf = input.LastIndexOf(value);
        return indexOf == -1 ? input : input[(indexOf + 1)..];
    }

    public static string After(this string input, string value, StringComparison comparisonType = Ordinal)
    {
        var indexOf = input.LastIndexOf(value, comparisonType);
        return indexOf == -1 ? input : input[(indexOf + value.Length)..];
    }

    public static string Before(this string input, char value, StringComparison comparisonType = Ordinal)
    {
        var indexOf = input.IndexOf(value, comparisonType);
        return indexOf == -1 ? input : input[..indexOf];
    }

    public static string Before(this string input, string value, StringComparison comparisonType = Ordinal)
    {
        var indexOf = input.IndexOf(value, comparisonType);
        return indexOf == -1 ? input : input[..indexOf];
    }

    public static bool ContainsInvariant(this string input, char value) => input.Contains(value, InvariantCulture);

    public static bool ContainsInvariant(this string input, string value) => input.Contains(value, InvariantCulture);

    public static IEnumerable<T> Extract<T>(this string input, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern, Func<Match, T> parser) =>
        Regex.Matches(input, pattern).Select(parser);

    public static IEnumerable<T> Extract<T>(this string input, Regex regex, Func<string, T> parser) =>
        regex.Matches(input).Select(m => parser(m.Value));

    public static IEnumerable<T> Extract<T>(this string input, Regex regex, Func<Match, T> parser) =>
        regex.Matches(input).Select(parser);

    public static IEnumerable<int> ExtractInts(this string input) => input.Extract(NumberRegex(), int.Parse);

    public static IEnumerable<long> ExtractLongs(this string input) => input.Extract(NumberRegex(), long.Parse);

    public static bool IsBlank(this string input) => string.IsNullOrWhiteSpace(input);

    public static bool IsEmpty(this string input) => string.IsNullOrEmpty(input);

    public static bool IsNotBlank(this string input) => !input.IsBlank();

    public static bool IsNotEmpty(this string input) => !input.IsEmpty();

    public static List<string> Lines(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return [];
        }

        // Split the input into lines
        var lines = input.Split(["\r\n", "\n", "\r"], StringSplitOptions.None);

        // Remove the trailing empty line if the input ends with a newline
        if (lines.Length > 0 && string.IsNullOrEmpty(lines[^1]))
        {
            return lines.SkipLast().ToList();
        }

        return lines.ToList();
    }

    public static string[] Lines(this string input, StringSplitOptions options) => input.Split(Environment.NewLine, options);

    public static IEnumerable<(int Row, int Col, char Value)> Scan(this IEnumerable<string> text, Func<char, bool> predicate)
    {
        var row = 0;
        foreach (var line in text)
        {
            var col = 0;
            foreach (var c in line)
            {
                if (predicate(c))
                {
                    yield return (row, col, c);
                }

                col++;
            }

            row++;
        }
    }

    public static string Strip(this string input, string oldValue, StringComparison? comparisonType = InvariantCulture) =>
        input.Replace(oldValue, string.Empty, comparisonType ?? InvariantCulture);

    // public static string Strip(this string input, string oldValue, bool ignoreCase, CultureInfo? culture) =>
    // input.Replace(oldValue, string.Empty, ignoreCase, culture);

    public static int ToInt(this string? input) => input is null ? default : int.Parse(input, NumberStyles.Integer, CultureInfo.InvariantCulture);

    public static long ToLong(this string? input) => input is null ? default : long.Parse(input, NumberStyles.Integer, CultureInfo.InvariantCulture);

    public static string? TrimToNull(this string input) =>
        string.IsNullOrWhiteSpace(input) ? null : input;

    public static string[] Words(this string input) => input.Words(StringSplitOptions.RemoveEmptyEntries);

    public static string[] Words(this string input, StringSplitOptions options) => input.Split(' ', options);

    [GeneratedRegex("-?\\d+", RegexOptions.Compiled)]
    private static partial Regex NumberRegex();
}
