namespace AdventOfCode.Y2024.Day04;

using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCodeDotNet;

public class Y2024Day04 : Solver
{
    public object PartOne(List<string> input)
    {
        List<int> solutions = [ByRotatingGrid.PartOne(input), ByScanningGrid.PartOne(input)];
        Debug.Assert(solutions.Distinct().Count() == 1);
        return solutions.First();
    }

    public object PartTwo(List<string> input)
    {
        List<int> solutions = [ByScanningGrid.PartTwo(input)];
        Debug.Assert(solutions.Distinct().Count() == 1);
        return solutions.First();
    }

    private static class ByRotatingGrid
    {
        private static readonly Regex XmasRegex = new("XMAS");
        private static readonly Regex SamxRegex = new("SAMX");

        public static int PartOne(List<string> input)
        {
            var inputGrid = GridFactory.Create(input);
            var rotations = new[] { inputGrid.Items, inputGrid.Rotate90(Rotation.Clockwise).Items, inputGrid.Rotate45(Rotation.Clockwise), inputGrid.Rotate45(Rotation.CounterClockwise) };
            return rotations.Sum(CountXmases);
        }

        private static int CountXmases(List<List<char>> grid) =>
            grid.Select(chars => string.Join("", chars))
                .Sum(line => XmasRegex.Matches(line).Count + SamxRegex.Matches(line).Count);
    }

    private static class ByScanningGrid
    {
        public static int PartOne(List<string> input)
        {
            // search XMAS in all 8 directions
            var grid = GridFactory.Create(input);
            return (from entry in grid.GetEntries()
                where entry.Value == 'X'
                from direction in Direction.All
                let n1 = entry.Cell.Move(direction)
                let n2 = entry.Cell.Move(direction, 2)
                let n3 = entry.Cell.Move(direction, 3)
                where grid.ValueAtOrDefault(n1) == 'M'
                where grid.ValueAtOrDefault(n2) == 'A'
                where grid.ValueAtOrDefault(n3) == 'S'
                select entry).Count();
        }

        public static int PartTwo(List<string> input)
        {
            // Find two MAS in the shape of an X. One way to achieve that is like this:
            //
            // M.S
            // .A.
            // M.S
            //
            // Irrelevant characters have again been replaced with . in the above diagram.
            // Within the X, each MAS can be written forwards or backwards.
            var grid = GridFactory.Create(input);
            return (from entry in grid.GetEntries()
                where entry.Value == 'A'
                let nw = grid.ValueAtOrDefault(entry.Cell.Move(Direction.NW))
                let ne = grid.ValueAtOrDefault(entry.Cell.Move(Direction.NE))
                let sw = grid.ValueAtOrDefault(entry.Cell.Move(Direction.SW))
                let se = grid.ValueAtOrDefault(entry.Cell.Move(Direction.SE))
                where (nw == 'M' && se == 'S') || (nw == 'S' && se == 'M')
                where (ne == 'M' && sw == 'S') || (ne == 'S' && sw == 'M')
                select entry).Count();
        }
    }
}
