namespace AdventOfCode.Y2022.Day03;

using Tool;

public class Y2022Day03 : Solver
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public object PartOne(List<string> input) => input.Select(SplitInHalf).Select(UniqueRepetition).Select(Score).Sum();

    public object PartTwo(List<string> input) => input.Chunk(3).Select(UniqueRepetition).Select(Score).Sum();

    private static int Score(char @char) => Alphabet.IndexOf(@char, InvariantCulture) + 1;

    private static string[] SplitInHalf(string line) => [line[..(line.Length / 2)], line[(line.Length / 2)..]];

    private static char UniqueRepetition(IEnumerable<string> strings) =>
        strings.Aggregate(
                Alphabet.ToHashSet(),
                (value, item) =>
                {
                    value.IntersectWith(item);
                    return value;
                })
            .Single();
}
