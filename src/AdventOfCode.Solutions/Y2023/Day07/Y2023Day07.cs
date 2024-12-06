namespace AdventOfCode.Y2023.Day07;

using System.Globalization;
using System.Text;
using Tool;

public class Y2023Day07 : Solver
{
    public enum Card
    {
        Joker,
        _2,
        _3,
        _4,
        _5,
        _6,
        _7,
        _8,
        _9,
        _10,
        Jack,
        Queen,
        King,
        Ace
    }

    public enum HandType
    {
        HighCard,
        OnePair,
        TwoPairs,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    public object PartOne(List<string> input) => Solve(input, GetCardSymbols(Card.Jack));

    public object PartTwo(List<string> input) => Solve(input, GetCardSymbols(Card.Joker));

    private static Dictionary<char, Card> GetCardSymbols(Card symbolForJ) =>
        new()
        {
            ['2'] = Card._2,
            ['3'] = Card._3,
            ['4'] = Card._4,
            ['5'] = Card._5,
            ['6'] = Card._6,
            ['7'] = Card._7,
            ['8'] = Card._8,
            ['9'] = Card._9,
            ['T'] = Card._10,
            ['J'] = symbolForJ,
            ['Q'] = Card.Queen,
            ['K'] = Card.King,
            ['A'] = Card.Ace
        };

    private static HandWithBid ParseHandWithBid(string line, Dictionary<char, Card> symbolToCard)
    {
        var lineParts = line.Split(' ');
        var handAsString = lineParts[0];
        var bid = lineParts[1].ToLong();

        var cards = handAsString.Select(symbol => symbolToCard[symbol]).ToList();
        return new(Hand.Create(cards), bid);
    }

    private static long Solve(List<string> input, Dictionary<char, Card> symbolToCard) =>
        input.Select(line => ParseHandWithBid(line, symbolToCard))
            .OrderBy(it => it.Hand)
            .Select(WeightedBid)
            .Sum();

    private static long WeightedBid(HandWithBid handWithBid, int index)
    {
        var rank = index + 1;
        // Console.WriteLine($"{handWithBid}, Rank: {rank}, WeightedBid: {handWithBid.Bid * rank}");
        return handWithBid.Bid * rank;
    }

    private sealed record HandWithBid(Hand Hand, long Bid);

    public record Hand(List<Card> Cards, HandType Type) : IComparable<Hand>
    {
        public int CompareTo(Hand? other)
        {
            if (other is null)
            {
                return 1;
            }

            return Type.NullableCompareTo(other.Type) ??
                   Cards.Select((card, i) => card.NullableCompareTo(other.Cards[i])).FirstOrDefault(it => it != null) ??
                   0;
        }

        public static Hand Create(ICollection<Card> cards) => new(cards.ToList(), InferHandTypeFromCards(cards));

        private static HandType InferHandTypeFromCards(ICollection<Card> cards)
        {
            // count the jokers
            var jokerCount = cards.Count(it => it == Card.Joker);

            // group by card without the jokers
            var groups = cards.Where(it => it != Card.Joker).GroupBy(it => it).Select(it => it.Count()).OrderDescending().ToList();

            // if there are no regular cards, it means there are 5 jokers
            if (groups.Count == 0)
            {
                return HandType.FiveOfAKind;
            }

            // add jokers to the top group
            groups[0] += jokerCount;

            return groups switch
            {
                [5] => HandType.FiveOfAKind,
                [4, 1] => HandType.FourOfAKind,
                [3, 2] => HandType.FullHouse,
                [3, 1, 1] => HandType.ThreeOfAKind,
                [2, 2, 1] => HandType.TwoPairs,
                [2, 1, 1, 1] => HandType.OnePair,
                [1, 1, 1, 1, 1] => HandType.HighCard,

                _ => throw new ArgumentException("Invalid cards", nameof(cards))
            };
        }

        protected virtual bool PrintMembers(StringBuilder stringBuilder)
        {
            stringBuilder.Append(CultureInfo.InvariantCulture, $"{Type}, [{Cards.Join(", ")}]");
            return true;
        }
    }
}
