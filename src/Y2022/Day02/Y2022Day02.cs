namespace AdventOfCode.Y2022.Day02;

using System.Diagnostics;
using AdventOfCodeDotNet;
using Moves = (string Theirs, string Ours);
using MoveAndOutcome = (string Theirs, int Outcome);
using FullMove = (string Theirs, string Ours, int Outcome);

public class Y2022Day02 : Solver
{
    private const int Rock = 1;
    private const int Paper = 2;
    private const int Scissors = 3;

    private const int LosePoints = 0;
    private const int DrawPoints = 3;
    private const int WinPoints = 6;

    private static readonly Dictionary<string, int> Codes = new()
    {
        ["A"] = Rock,
        ["B"] = Paper,
        ["C"] = Scissors,
        ["X"] = Rock,
        ["Y"] = Paper,
        ["Z"] = Scissors
    };


    private static readonly Dictionary<Moves, int> MovesOutcomes = new()
    {
        [new("A", "X")] = DrawPoints,
        [new("A", "Y")] = WinPoints,
        [new("A", "Z")] = LosePoints,
        [new("B", "X")] = LosePoints,
        [new("B", "Y")] = DrawPoints,
        [new("B", "Z")] = WinPoints,
        [new("C", "X")] = WinPoints,
        [new("C", "Y")] = LosePoints,
        [new("C", "Z")] = DrawPoints
    };

    // private Hand RockPaper = (Rock, Paper);
    // private Hand PaperScissors = (Paper, Scissors);
    // private Hand ScissorsRock = (Scissors, Rock);
    // private static readonly List<Moves> Wins =
    // [
    //     (Rock, Paper),
    //     (Paper, Scissors),
    //     (Scissors, Rock)
    // ];

    public object PartOne(List<string> input)
    {
        List<int> solutions = [WithEverythingHardcoded.PartOne(input)];
        Debug.Assert(solutions.Distinct().Count() == 1);
        return solutions.First();
    }

    // input.Select(it => Scores[it]).Sum();
    public object PartTwo(List<string> input)
    {
        List<int> solutions = [WithEverythingHardcoded.PartTwo(input)];
        Debug.Assert(solutions.Distinct().Count() == 1);
        return solutions.First();
    }

    private static MoveAndOutcome ParseMoveAndOutcome(string line)
    {
        var outcome = line.After(' ') switch
        {
            "X" => LosePoints,
            "Y" => DrawPoints,
            "Z" => WinPoints,
            _ => throw new ArgumentException($"Invalid outcome for {line}")
        };
        return new(line.Before(' '), outcome);
    }

    private static Moves ParseMoves(string line) => new(line.Before(' '), line.After(' '));

    private FullMove InferOurMove(MoveAndOutcome input)
    {
        var ours = input.Theirs switch
        {
            "X" => "B",
            "Y" => "C",
            "Z" => "A",
            _ => throw new ArgumentException($"Invalid move for {input}")
        };
        return new(input.Theirs, ours, input.Outcome);
    }

    private FullMove InferOutcome(Moves moves) => new(moves.Theirs, moves.Ours, MovesOutcomes[moves]);

    // private int Score(Moves moves) => Codes[moves.Ours] + MovesOutcomes[moves];

    // private int Score2(MoveAndResult moveAndResult) => Codes[moves.Ours] + MovesOutcomes[moves];

    private int Score3((string Theirs, string Ours, int Outcome) input) => Codes[input.Ours] + input.Outcome;

    // A for Rock, B for Paper, and C for Scissors.

    // The score for a single round is the score for the shape you selected (1 for Rock, 2 for Paper, and 3 for Scissors) plus the score for the outcome of the round (0 if you lost, 3 if the round was a draw, and 6 if you won).

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
