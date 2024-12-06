namespace AdventOfCode.Y2022.Day05;

using Tool;
using Instruction = (int Count, int Source, int Destination);
using MoveStrategy = Action<IEnumerable<char>, Stack<char>>;

public class Y2022Day05 : Solver
{
    private static MoveStrategy AllAtOnce => (items, destination) => destination.PushRange(items.Reverse());

    private static MoveStrategy OneByOne => (items, destination) => destination.PushRange(items);

    public object PartOne(List<string> input) => Solve(input, OneByOne);

    public object PartTwo(List<string> input) => Solve(input, AllAtOnce);

    private static void ApplyInstructions(List<Stack<char>> stacks, List<Instruction> instructions, MoveStrategy moveStrategy)
    {
        foreach (var instruction in instructions)
        {
            var source = stacks[instruction.Source - 1];
            var destination = stacks[instruction.Destination - 1];

            var items = source.Pop(instruction.Count);
            moveStrategy(items, destination);
        }
    }

    private static List<Instruction> ParseInstructions(List<string> input) =>
        input
            .Split(string.IsNullOrEmpty).Skip(1).First()
            .Select(line => line.ExtractInts().ToList())
            .Select(ints => new Instruction(ints[0], ints[1], ints[2]))
            .ToList();

    private static Stack<char> ParseStack(List<string> body, int index) =>
        new(
            body.Where(line => line.Length > index && line[index] != ' ')
                .Select(line => line[index]));

    private static List<Stack<char>> ParseStacks(List<string> input)
    {
        var lines = input.Split(string.IsNullOrEmpty).First().Reverse().ToList();
        var header = lines[0];
        var body = lines[1..];

        return header
            .Select((@char, index) => (@char, index)).Where(it => it.@char != ' ') // indexes of non-empty
            .Select(index => ParseStack(body, index.index))
            .ToList();
    }

    private static string Solve(List<string> input, MoveStrategy moveStrategy)
    {
        var stacks = ParseStacks(input);
        var instructions = ParseInstructions(input);
        ApplyInstructions(stacks, instructions, moveStrategy);
        return new(stacks.Select(stack => stack.Peek()).ToArray());
    }
}
