namespace AdventOfCode.Tool.Features;

public class Config
{
    internal static readonly string YearPlaceholder = "{Year}";
    internal static readonly string DayPlaceholder = "{Day}";

    public string ClassName { get; set; } = "Y{Year}Day{Day}";

    public string ClassPath { get; set; } = "Y{Year}/Day{Day}";

    public string NamespaceName { get; set; } = "AdventOfCode.Y{Year}.Day{Day}";

    public string? Session { get; set; }
}
