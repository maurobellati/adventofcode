namespace AdventOfCode.Y2024.Day17;

using Tool;

public class Y2024Day17 : Solver
{
    public object PartOne(List<string> input) => ParseComputer(input).Process(Instructions(input)).Join(",");

    public object PartTwo(List<string> input) => FindQuine(ParseComputer(input), Instructions(input));

    private static object FindQuine(Computer computer, List<int> instructions)
    {
        var correctDigits = 0;
        var solution = 0L;
        var attemptsCount = 0;
        // Console.WriteLine("Instructions: {0}", string.Join(",", instructions));
        var i = 0;
        while (correctDigits < instructions.Count)
        {
            var candidate = (solution * 8) + i++;
            var output = new Computer(candidate, computer.RegistryB, computer.RegistryC).Process(instructions);
            attemptsCount++;

            if (output.SequenceEqual(instructions[^(correctDigits + 1)..]))
            {
                // Console.WriteLine("Tried  {0,18} {0,52:B}: *** Found last {1,2} digits: {2} attempts={3}", candidate, correctDigits + 1, output.Join(","), attemptsCount);
                correctDigits++;
                solution = candidate;
                i = 0;
            }
        }

        return solution;
    }

    private static List<int> Instructions(List<string> input) => input[4].ExtractInts().ToList();

    private static Computer ParseComputer(List<string> input) =>
        new(
            input[0].ExtractInts().Single(),
            input[1].ExtractInts().Single(),
            input[2].ExtractInts().Single()
        );

    private sealed class Computer(long registryA, long registryB, long registryC)
    {
        private readonly List<int> output = [];

        private int programCounter;

        private long RegistryA { get; set; } = registryA;

        internal long RegistryB { get; private set; } = registryB;

        internal long RegistryC { get; private set; } = registryC;

        public List<int> Process(List<int> instructions)
        {
            while (programCounter < instructions.Count)
            {
                var opcode = instructions[programCounter];
                var operand = instructions[programCounter + 1];
                Process(opcode, operand);
            }

            return output;
        }

        private void Adv(int operand) => RegistryA = Xdv(operand);

        private void Bdv(int operand) => RegistryB = Xdv(operand);

        private void Bst(int operand) => RegistryB = ComboOperand(operand) & 7;

        private void Bxc(int _) => RegistryB ^= RegistryC;

        private void Bxl(int operand) => RegistryB ^= operand;

        private void Cdv(int operand) => RegistryC = Xdv(operand);

        private long ComboOperand(int operand) =>
            operand switch
            {
                <= 3 => operand,
                4 => RegistryA,
                5 => RegistryB,
                6 => RegistryC,
                _ => throw new ArgumentException($"Invalid operand {operand}", nameof(operand))
            };

        private bool Jnz(int operand)
        {
            var jump = RegistryA != 0;
            if (jump)
            {
                programCounter = operand;
            }

            return jump;
        }

        private void Out(int operand) => Output(ComboOperand(operand) & 7);

        private void Output(long value) => output.Add((int)value);

        private void Process(int opcode, int operand)
        {
            switch (opcode)
            {
                case 0:
                    Adv(operand);
                    break;
                case 1:
                    Bxl(operand);
                    break;
                case 2:
                    Bst(operand);
                    break;
                case 3:
                    {
                        if (Jnz(operand))
                        {
                            return;
                        }

                        break;
                    }
                case 4:
                    Bxc(operand);
                    break;
                case 5:
                    Out(operand);
                    break;
                case 6:
                    Bdv(operand);
                    break;
                case 7:
                    Cdv(operand);
                    break;
                default:
                    throw new ArgumentException($"Invalid opcode {opcode}", nameof(opcode));
            }

            programCounter += 2;
        }

        private long Xdv(int operand) => RegistryA / (long)Math.Pow(2, ComboOperand(operand));
    }
}
