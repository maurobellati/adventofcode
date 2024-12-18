namespace AdventOfCode.Y2023.Day07;

using System.Globalization;
using Tool;

public class Y2023Day07 : Solver
{
    public object PartOne(List<string> input) => Solve(input, ParsePart1);

    public object PartTwo(List<string> input) => Solve(input, ParsePart2);

    private static string GetCardValues(string cards, string cardOrder) =>
        // 32T3K becomes 01.00.08.01.11
        cards.Select(card => cardOrder.IndexOf(card, InvariantCulture))
            .Select(i => i.ToString("D2", CultureInfo.InvariantCulture)).Join(".");

    private static string GetHandType(string cards) => cards.Select(c => cards.Count(card => c == card)).OrderDescending().Join();

    private static SimpleHand ParsePart1(string cards) => new(GetHandType(cards), GetCardValues(cards, "23456789TJQKA"), cards);

    private static SimpleHand ParsePart2(string cards)
    {
        var cardOrder = "J23456789TQKA"; // J cards are now the weakest individual cards, weaker even than 2
        var bestHandTypePossible = cardOrder
            .Select(@char => cards.Replace('J', @char))
            .Select(GetHandType)
            .Max()!;
        return new(bestHandTypePossible, GetCardValues(cards, cardOrder), cards);
    }

    private static long Solve(List<string> input, Func<string, SimpleHand> parser) =>
        input.Select(line => (Bid: line.After(' ').ToLong(), Hand: parser(line.Before(' '))))
            .OrderBy(it => it.Hand.Type).ThenBy(it => it.Hand.CardValues)
            .Select((hand, rank) => hand.Bid * (rank + 1))
            .Sum();

    private sealed record SimpleHand(string Type, string CardValues, string Cards);
}
