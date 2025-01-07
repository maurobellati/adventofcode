namespace AdventOfCode.Y2024.Day24;

using Tool;

public class Y2024Day24 : Solver
{
    private const string And = "AND";
    private const string Xor = "XOR";
    private const string Or = "OR";

    private static readonly List<GateRule> FullAdderRules =
    [
        new(
            "last bit output can come from OR",
            (gate, _) => gate is { Op: Or, Output: "z45" }),
        new(
            "AND-gate from first input bits must be a carry (go to AND|XOR)",
            (gate, gates) => gate is { Input1: "x00", Input2: "y00", Op: And } && ChildrenOps(gate, gates).SequenceEqual([And, Xor])),
        new(
            "XOR-gate from input bits must go to AND|XOR",
            (gate, gates) => gate.Input1[0] == 'x' && gate.Input2[0] == 'y' && gate.Op == Xor && ChildrenOps(gate, gates).SequenceEqual([And, Xor])),
        new(
            "XOR-gate can go to output",
            (gate, _) => gate.Output[0] == 'z' && gate.Op == Xor),
        new(
            "AND-gate must be input for carry (go to OR)",
            (gate, gates) => gate.Op == And && ChildrenOps(gate, gates).SequenceEqual([Or])),
        new(
            "OR-gate must be a carry (go to AND|XOR)",
            (gate, gates) => gate.Op == Or && ChildrenOps(gate, gates).SequenceEqual([And, Xor]))
    ];

    public object PartOne(List<string> input)
    {
        var values = ParseValues(input);
        var gates = ParseGates(input);

        while (gates.Count > 0)
        {
            var gate = gates.First(it => values.ContainsKey(it.Input1) && values.ContainsKey(it.Input2));
            gates.Remove(gate);

            values[gate.Output] = gate.Op switch
            {
                And => values[gate.Input1] & values[gate.Input2],
                Or => values[gate.Input1] | values[gate.Input2],
                Xor => values[gate.Input1] ^ values[gate.Input2],
                _ => throw new InvalidOperationException()
            };
        }

        var booleans = values.Where(kv => kv.Key.StartsWith('z')).OrderByDescending(kv => kv.Key).Select(kv => kv.Value);
        return booleans.Aggregate(0L, (acc, @bool) => (acc << 1) | (uint)(@bool ? 1 : 0));
    }

    public object PartTwo(List<string> input)
    {
        var gates = ParseGates(input);
        return gates.Where(gate => !IsValid(gate, gates)).Select(it => it.Output).Order().Join(",");
    }

    private static IEnumerable<string> ChildrenOps(Gate gate, List<Gate> gates) =>
        gates.Where(it => it.Input1 == gate.Output || it.Input2 == gate.Output).Select(it => it.Op).Distinct().Order();

    private static bool IsValid(Gate gate, List<Gate> gates)
    {
        var rule = FullAdderRules.FirstOrDefault(rule => rule.Predicate(gate, gates));
        var isValid = rule is not null;
        // AnsiConsole.WriteLine(isValid ? $"{gate} is ✅ by {rule!.RuleName}" : $"{gate} is ❌");
        return isValid;
    }

    private static List<Gate> ParseGates(List<string> input) =>
        input.SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1)
            .Select(line => line.Split(' '))
            .Select(
                parts =>
                {
                    var inputs = new[] { parts[0], parts[2] }.Order().ToList();
                    return new Gate(inputs[0], parts[1], inputs[1], parts[4]);
                })
            .ToList();

    private static Dictionary<string, bool> ParseValues(List<string> input) =>
        input.TakeWhile(line => !string.IsNullOrEmpty(line)).ToDictionary(line => line.Before(":"), line => line.After(":")[1] == '1');

    private sealed record GateRule(string RuleName, Func<Gate, List<Gate>, bool> Predicate);

    private sealed record Gate(string Input1, string Op, string Input2, string Output)
    {
        public override string ToString() => $"{Input1} {Op,3} {Input2} -> {Output}";
    }
}
