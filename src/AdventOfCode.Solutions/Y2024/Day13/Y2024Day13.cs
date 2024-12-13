namespace AdventOfCode.Y2024.Day13;

using Tool;
using Position = (long Col, long Row);

public class Y2024Day13 : Solver
{
    public object PartOne(List<string> input) => Solve(input);

    public object PartTwo(List<string> input) => Solve(input, 10000000000000L);

    private static long Det(Position a, Position b) => (a.Row * b.Col) - (a.Col * b.Row);

    private static long GetCost(Machine machine)
    {
        // row equation: |ButtonA.Row * x1 + ButtonB.Row * x2| = |Prize.Row|
        // col equation: |ButtonA.Col * x1 + ButtonB.Col * x2| = |Prize.Col|

        var (a, b, p) = (machine.ButtonA, machine.ButtonB, machine.Prize);

        /*
         * |a1 b1| |x1| = |c1|
         * |a2 b2| |x1| = |c2|
         *
         * AX = C
         *
         * X = A^(-1) * C
         *
         * A^(-1) = 1/DET * |b2 -b1|
         *                  |-a2 a1|
         *
         * DET = a1*b2 − a2*b1
         *
         * x1 = (c1*b2 - c2*b1) / DET
         * x2 = (a1*c2 - a2*c1) / DET
         */

        var det = Det(a, b);
        if (det == 0)
        {
            return 0;
        }

        var x1 = Det(p, b) / det;
        var x2 = Det(a, p) / det;

        // if the solutions are NOT integers, then there is no solution and return null
        if ((x1 * a.Row) + (x2 * b.Row) != p.Row || (x1 * a.Col) + (x2 * b.Col) != p.Col)
        {
            return 0;
        }

        return (x1 * 3) + x2;
    }

    private static Machine ParseMachine(IEnumerable<string> lines, long factor)
    {
        var cells = lines.Take(3).Select(line => line.ExtractInts().ToList()).Select(pair => new Position(pair[0], pair[1])).ToList();
        return new(cells[0], cells[1], new(cells[2].Col + factor, cells[2].Row + factor));
    }

    private static long Solve(List<string> input, long? addFactor = 0) =>
        input.Split(string.IsNullOrEmpty)
            .Select(lines => ParseMachine(lines, addFactor ?? 0))
            .Select(GetCost)
            .Sum();

    private sealed record Machine(Position ButtonA, Position ButtonB, Position Prize);
}
