namespace AdventOfCode.Y2024.Day25;

using Tool;
using PinHeights = int[];

public class Y2024Day25 : Solver
{
    public object PartOne(List<string> input)
    {
        var keys = ParseKeys(input);
        var locks = ParseLocks(input);

        // return how many lock/keys are compatible
        // var count = locks.Count(@lock => keys.Any(key => IsCompatible(key, @lock)));
        var count2 = (from @lock in locks
            from key in keys
            where IsCompatible(key, @lock)
            select 1).Count();

        return count2;
    }

    public object PartTwo(List<string> input) => 0;

    public bool IsCompatible(PinHeights key, PinHeights @lock) => key.Zip(@lock, (x, y) => x + y).All(h => h <= 5);

    private List<PinHeights> ParseKeys(List<string> input) =>
        input.Split(string.IsNullOrEmpty)
            .Where(lines => lines.First().Equals(".....", OrdinalIgnoreCase))
            .Select(ToPinHeights)
            .ToList();

    private List<PinHeights> ParseLocks(List<string> input) =>
        input.Split(string.IsNullOrEmpty)
            .Where(lines => lines.First().Equals("#####", OrdinalIgnoreCase))
            .Select(ToPinHeights)
            .ToList();

    private PinHeights ToPinHeights(IEnumerable<string> lines) =>
        /* input lines are like:
           XXXXX
           #....
           #....
           #...#
           #.#.#
           #.###
        */
        // return a list of 5 ints counting how many # are in vertical
        // for the above example, the result should be [5,0,2,1,3]
        lines.Skip(1).Take(5)
            .Aggregate(
                new int[5],
                (pins, line) =>
                {
                    for (var i = 0; i < 5; i++)
                    {
                        pins[i] += line[i] == '#' ? 1 : 0;
                    }

                    return pins;
                });
}
