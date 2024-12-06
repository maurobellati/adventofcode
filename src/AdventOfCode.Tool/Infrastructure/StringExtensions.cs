namespace AdventOfCode.Tool.Infrastructure;

public static class StringExtensions
{
    public static string[] Lines(this string input)
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
            return lines[..^1];
        }

        return lines;
    }
}
