namespace AdventOfCode.Y2023.Day20;

using System.Diagnostics;
using AdventOfCodeDotNet;
using Pulse = int;

public class Y2023Day20 : Solver
{
    private const Pulse Low = 0;
    private const Pulse High = 1;

    public object PartOne(List<string> input)
    {
        var machine = ParseMachine(input);

        List<PulseCommand> commands = [];

        const int Cycles = 1000;
        for (var i = 1; i <= Cycles; i++)
        {
            var cycleCommand = Run(machine);
            LogCommands(i, cycleCommand);
            commands.AddRange(cycleCommand);
        }

        return commands.Count(it => it.Pulse == High) * commands.Count(it => it.Pulse == Low);
    }

    public object PartTwo(List<string> input)
    {
        //what is the fewest number of button presses required to deliver a single low pulse to the module named rx?
        var machine = ParseMachine(input);

        var output = "rx";
        var rxSource = machine.GetSourcesOf(output).Single();
        Debug.Assert(machine.GetModuleByName(rxSource) is ConjunctionModule);

        var rxSourceSourceNames = machine.GetSourcesOf(rxSource).ToList();
        // Console.WriteLine($"[{rxSourceSourceNames.Join(", ")}] --> {rxSource} --> {output}");

        // PrintMermaidFlowchart(machine);

        // Let's try to find the cycle count for each source of rxSource

        // store the cycle count for each source
        Dictionary<string, long> cycleLength = [];
        // store how many times we have seen each source
        var seenCount = rxSourceSourceNames.ToDictionary(it => it, _ => 0);

        // iterate until we've seen each source pulse at least 3 times, to be sure the cycle is accurate
        var runCount = 0;
        while (seenCount.Any(kv => kv.Value < 3))
        {
            var cycleCommand = Run(machine);
            runCount++;
            // LogCommands(i, cycleCommand.Where(it => rxSourceSourceNames.Contains(it.Source.Name)));

            foreach (var sourceName in cycleCommand
                         .Where(it => rxSourceSourceNames.Contains(it.SourceName) && it.Pulse == High)
                         .Select(it => it.SourceName))
            {
                seenCount[sourceName]++;
                cycleLength.TryAdd(sourceName, runCount);
                var expectedRunCount = cycleLength[sourceName] * seenCount[sourceName];
                Debug.Assert(expectedRunCount == runCount);
                // Console.WriteLine(
                //     "Seen {0} {1} times after ({2} * {1}) {3} cycles",
                //     sourceName,
                //     seenCount[sourceName],
                //     cycleLength[sourceName],
                //     runCount);
            }
        }

        return cycleLength.Values.Aggregate(MathExtensions.Lcm);
    }

    private static void LogCommands(int _, IEnumerable<PulseCommand> cycleCommand)
    {
        // Console.WriteLine($"Cycle {index}");
        foreach (var command in cycleCommand)
        {
            // Console.WriteLine($"{command.SourceName} -{command.Pulse}-> {command.DestinationName}");
        }
    }

    private static Machine ParseMachine(List<string> input)
    {
        List<Module> modules = [];
        Dictionary<string, List<string>> forwardConnections = [];
        foreach (var parts in input.Select(it => it.Split(" -> ", StringSplitOptions.TrimEntries)))
        {
            var connections = parts[1].Split(", ", StringSplitOptions.TrimEntries).ToList();
            Module module = parts[0].Trim() switch
            {
                ['%', .. var name] => new FlipFlopModule(name),
                ['&', .. var name] => new ConjunctionModule(name),
                var name => new BroadcasterModule(name)
            };

            modules.Add(module);
            forwardConnections.Add(module.Name, connections);
        }

        return new(modules, forwardConnections);
    }

    private static List<PulseCommand> Run(Machine machine)
    {
        List<PulseCommand> executedCommands = [];

        var commands = new Queue<PulseCommand>();
        commands.Enqueue(new("button", "broadcaster", Low));

        while (commands.Count > 0)
        {
            var command = commands.Dequeue();
            executedCommands.Add(command);

            var destinationModule = machine.GetModuleByName(command.DestinationName);
            var pulse = destinationModule.ReceivePulse(command.SourceName, command.Pulse);
            if (pulse is null)
            {
                continue;
            }

            foreach (var destinationDestination in machine.Connections[command.DestinationName])
            {
                commands.Enqueue(new(command.DestinationName, destinationDestination, pulse.Value));
            }
        }

        return executedCommands;
    }

    private sealed class Machine
    {
        private readonly Dictionary<string, Module> modules;

        public Machine(IEnumerable<Module> modules, Dictionary<string, List<string>> connections)
        {
            Connections = connections;
            this.modules = modules.ToDictionary(it => it.Name, it => it);

            // wire reverse connections for ConjunctionModules
            foreach (var conjunctionModule in this.modules.Values.OfType<ConjunctionModule>())
            {
                var reverseConnections = connections.Where(kv => kv.Value.Contains(conjunctionModule.Name)).Select(kv => kv.Key).ToList();
                conjunctionModule.SetReverseConnections(reverseConnections);
            }

            // add missing modules
            foreach (var name in connections.SelectMany(it => it.Value).Except(connections.Keys))
            {
                // Console.WriteLine($"Adding missing module {name}");
                this.modules.Add(name, new(name));
            }
        }

        public Dictionary<string, List<string>> Connections { get; }

        public Module GetModuleByName(string name) => modules[name];

        public IEnumerable<string> GetSourcesOf(string module) => Connections.Where(kv => kv.Value.Contains(module)).Select(kv => kv.Key);
    }

    private sealed record PulseCommand(string SourceName, string DestinationName, Pulse Pulse)
    {
        public override string ToString() => $"{SourceName} -{Pulse}-> {DestinationName}";
    }

    private sealed record BroadcasterModule(string Name) : Module(Name)
    {
        public override Pulse? ReceivePulse(string moduleName, Pulse pulse) => pulse;
    }

    private sealed record FlipFlopModule(string Name) : Module(Name)
    {
        private bool isOn;

        public override Pulse? ReceivePulse(string moduleName, Pulse pulse)
        {
            if (pulse == High)
            {
                // When high pulse: it is ignored and nothing happens.
                return null;
            }

            // When low pulse: it flips between on and off. If it was off, it turns on and sends a high pulse.
            isOn = !isOn;
            //If it was on, it turns off and sends a low pulse.
            return isOn ? High : Low;
        }
    }

    private sealed record ConjunctionModule(string Name) : Module(Name)
    {
        private readonly Dictionary<string, Pulse> pulses = new();

        public override Pulse? ReceivePulse(string moduleName, Pulse pulse)
        {
            // When a pulse is received, the conjunction module first updates its memory for that input.
            pulses[moduleName] = pulse;
            //Then, if it remembers high pulses for all inputs, it sends a low pulse; otherwise, it sends a high pulse.
            return pulses.Values.All(p => p == High) ? Low : High;
        }

        public void SetReverseConnections(ICollection<string> connections)
        {
            foreach (var connection in connections)
            {
                // initially default to remembering a low pulse for each input.
                pulses.Add(connection, Low);
            }
        }
    }

    private record Module(string Name)
    {
        public virtual Pulse? ReceivePulse(string moduleName, Pulse pulse) => null;

        public override string ToString() => $"{GetType().Name} {Name}";
    }
}
