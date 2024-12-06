namespace AdventOfCode.Y2022.Day02;

using System.Diagnostics;
using Tool;
using Match = (Y2022Day02.WithDecoding.Move Elf, Y2022Day02.WithDecoding.Move Human);

public class Y2022Day02 : Solver
{
    public object PartOne(List<string> input)
    {
        List<int> solutions = [WithEverythingHardcoded.PartOne(input), WithDecoding.PartOne(input)];
        Debug.Assert(solutions.Distinct().Count() == 1);
        return solutions.First();
    }

    public object PartTwo(List<string> input)
    {
        List<int> solutions = [WithEverythingHardcoded.PartTwo(input), WithDecoding.PartTwo(input)];
        Debug.Assert(solutions.Distinct().Count() == 1);
        return solutions.First();
    }

    internal static class WithDecoding
    {
        private static readonly Dictionary<Move, Move> Winners = new()
        {
            [Move.Rock] = Move.Paper,
            [Move.Paper] = Move.Scissors,
            [Move.Scissors] = Move.Rock
        };

        private static readonly Dictionary<Move, Move> Losers = new()
        {
            [Move.Rock] = Move.Scissors,
            [Move.Paper] = Move.Rock,
            [Move.Scissors] = Move.Paper
        };

        public static int PartOne(List<string> input)
        {
            return input.Select(line => new Match(ElfMove(line[0]), HumanMove(line[2]))).Select(CalculateScore).Sum();

            Move HumanMove(char @char) =>
                @char switch
                {
                    'X' => Move.Rock,
                    'Y' => Move.Paper,
                    'Z' => Move.Scissors,
                    _ => throw new InvalidOperationException()
                };
        }

        public static int PartTwo(List<string> input)
        {
            return input.Select(line => new Match(ElfMove(line[0]), HumanMove(ElfMove(line[0]), line[2]))).Select(CalculateScore).Sum();

            Move HumanMove(Move elf, char @char) =>
                @char switch
                {
                    'X' => Losers[elf],
                    'Y' => elf,
                    'Z' => Winners[elf],
                    _ => throw new InvalidOperationException()
                };
        }

        private static int CalculateScore(Match match)
        {
            // the score for the shape you selected (1 for Rock, 2 for Paper, and 3 for Scissors)
            var selectedScore = (int)match.Human;
            // the score for the outcome of the round (0 if you lost, 3 if the round was a draw, and 6 if you won).
            var outcomeScore = (int)GetOutcome(match);
            return selectedScore + outcomeScore;
        }

        private static Move ElfMove(char input) => input switch
        {
            'A' => Move.Rock,
            'B' => Move.Paper,
            'C' => Move.Scissors,
            _ => throw new InvalidOperationException()
        };

        private static Outcome GetOutcome(Match match) =>
            match.Human == match.Elf ? Outcome.Draw :
            match.Human == Winners[match.Elf] ? Outcome.Win :
            match.Human == Losers[match.Elf] ? Outcome.Lose :
            throw new InvalidOperationException();

        internal enum Move
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        private enum Outcome
        {
            Lose = 0,
            Draw = 3,
            Win = 6
        }
    }

    private static class WithEverythingHardcoded
    {
        private const int Rock = 1;
        private const int Paper = 2;
        private const int Scissors = 3;

        private const int LosePoints = 0;
        private const int DrawPoints = 3;
        private const int WinPoints = 6;

        public static int PartOne(List<string> input)
        {
            // A => Rock, B => Paper, C => Scissors
            // X => Rock, Y => Paper, Z => Scissors
            Dictionary<string, int> results = new()
            {
                ["A X"] = Rock + DrawPoints,
                ["A Y"] = Paper + WinPoints,
                ["A Z"] = Scissors + LosePoints,
                ["B X"] = Rock + LosePoints,
                ["B Y"] = Paper + DrawPoints,
                ["B Z"] = Scissors + WinPoints,
                ["C X"] = Rock + WinPoints,
                ["C Y"] = Paper + LosePoints,
                ["C Z"] = Scissors + DrawPoints
            };

            return input.Select(line => results[line]).Sum();
        }

        public static int PartTwo(List<string> input)
        {
            // A => Rock, B => Paper,C => Scissors
            // X => Lose, Y => Draw, Z => Win
            Dictionary<string, int> results = new()
            {
                ["A X"] = LosePoints + Scissors,
                ["A Y"] = DrawPoints + Rock,
                ["A Z"] = WinPoints + Paper,
                ["B X"] = LosePoints + Rock,
                ["B Y"] = DrawPoints + Paper,
                ["B Z"] = WinPoints + Scissors,
                ["C X"] = LosePoints + Paper,
                ["C Y"] = DrawPoints + Scissors,
                ["C Z"] = WinPoints + Rock
            };

            return input.Select(line => results[line]).Sum();
        }
    }
}
