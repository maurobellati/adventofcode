namespace AdventOfCode.Y2023.Day08;

using Tool;
using LR = (string Left, string Right);

public class Y2023Day08 : Solver
{
    public object PartOne(List<string> input)
    {
        var instructions = ParseInstructions(input);
        var network = ParseNetwork(input);

        var start = "AAA";
        return CountStep(start, node => node == "ZZZ", instructions, network);
    }

    public object PartTwo(List<string> input)
    {
        var instructions = ParseInstructions(input);
        var network = ParseNetwork(input);

        var startNodes = network.Keys.Where(it => it.EndsWith('A')).ToList();
        var steps = startNodes.Select(start => CountStep(start, node => node.EndsWith('Z'), instructions, network)).ToList();
        // Console.WriteLine($"steps: {string.Join(", ", steps)}");

        // return least common multiple of all steps
        return steps.Aggregate(MathExtensions.Lcm);
    }

    private static long CountStep(string start, Predicate<string> goal, string instructions, Dictionary<string, LR> network)
    {
        var step = 0;
        var node = start;
        while (!goal(node))
        {
            var instruction = instructions[step % instructions.Length];
            step++;
            node = instruction == 'L' ? network[node].Left : network[node].Right;
            // Console.WriteLine("Step {0}: {1} => {2}", step, instruction, node);
        }

        return step;
    }

    private static string ParseInstructions(List<string> input) =>
        // line 0: a string of "LR" characters, each representing a left or right turn
        input.First();

    private static Dictionary<string, LR> ParseNetwork(List<string> input) =>
        // line 2 to n: labeled nodes as: "AAA = (BBB, CCC)"
        input.Skip(2).ToDictionary(line => line[..3], line => (line[7..10], line[12..15]));
}
