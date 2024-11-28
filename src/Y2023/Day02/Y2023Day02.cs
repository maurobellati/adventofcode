namespace AdventOfCode.Y2023.Day02;

using System.Text.RegularExpressions;
using AdventOfCodeDotNet;

public class Y2023Day02 : Solver
{
    public object PartOne(List<string> input) =>
        //12 red, 13 green, and 14 blu
        input.Select(ParseGame)
            .Where(game => game.Matches.TrueForAll(rgb => rgb is { Red: <= 12, Green: <= 13, Blue: <= 14 }))
            .Select(game => game.Id)
            .Sum();

    public object PartTwo(List<string> input) =>
        input.Select(ParseGame)
            .Select(game => MaxMatch(game.Matches))
            .Select(match => match.Red * match.Green * match.Blue)
            .Sum();

    private static int GetComponent(string input, string component)
    {
        var value = Regex.Match(input, $@"(\d+) {component}").Groups[1].Value;
        return value.TrimToNull().ToInt();
    }

    private static Match MaxMatch(IReadOnlyCollection<Match> rgbs) =>
        // for each component, get the max value
        new(
            rgbs.Max(rgb => rgb.Red),
            rgbs.Max(rgb => rgb.Green),
            rgbs.Max(rgb => rgb.Blue));

    private static Game ParseGame(string line)
    {
        // extract Id from: "Game {ID}: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"
        var id = line.Before(":").ExtractInts().Single();
        // get substring after :
        var matches = line.After(":").Split(";").Select(ParseMatch).ToList();
        return new(id, matches);
    }

    private static Match ParseMatch(string input) =>
        // input format: 1 red, 2 green, 6 blue
        // red, green, blue: are optional and can be in any order, but just once per input
        new(GetComponent(input, "red"), GetComponent(input, "green"), GetComponent(input, "blue"));

    public record Game(int Id, List<Match> Matches);

    public record Match(int Red, int Green, int Blue);
}
