namespace AdventOfCode.Y2022.Day19;

using Tool;

public class Y2022Day19 : Solver
{
    public object PartOne(List<string> input) =>
        input.Select(ParseBlueprint)
            .Select(blueprint => QualityLevel(blueprint, 24))
            .Sum();

    public object PartTwo(List<string> input) =>
        input.Take(3)
            .Select(ParseBlueprint)
            .Select(blueprint => MaxNumberOfGeodes(blueprint, 32))
            .Aggregate(1L, (a, b) => a * b);

    private static Blueprint ParseBlueprint(string line)
    {
        var ints = line.ExtractInts().ToList();
        return new(
            ints[0],
            Quantities.SingleOre * ints[1],
            Quantities.SingleOre * ints[2],
            (Quantities.SingleOre * ints[3]) + (Quantities.SingleClay * ints[4]),
            (Quantities.SingleOre * ints[5]) + (Quantities.SingleObsidian * ints[6])
        );
    }

    private IEnumerable<Quantities> GetPurchaseOptions(Quantities quantities, Blueprint blueprint)
    {
        yield return Quantities.Zero; // No purchase

        if (quantities.CanAfford(blueprint.OreRobotCost))
        {
            yield return Quantities.SingleOre;
        }

        if (quantities.CanAfford(blueprint.ClayRobotCost))
        {
            yield return Quantities.SingleClay;
        }

        if (quantities.CanAfford(blueprint.ObsidianRobotCost))
        {
            yield return Quantities.SingleObsidian;
        }

        if (quantities.CanAfford(blueprint.GeodeRobotCost))
        {
            yield return Quantities.SingleGeode;
        }
    }

    private int MaxNumberOfGeodes(Blueprint input, int targetMinutes)
    {
        // Start with 1 ore robot
        var initialState = new FactoryState(Quantities.Zero, Quantities.SingleOre);
        List<FactoryState> states = [initialState];

        var minute = 0;
        while (minute < targetMinutes)
        {
            minute++;

            // Keep only the top N states based on some heuristic to limit the state space
            states = states
                .SelectMany(state => NextStates(state, input))
                .OrderByDescending(it => it.Resources.Geode)
                .ThenByDescending(it => it.Robots.Geode)
                .ThenByDescending(it => it.Robots.Obsidian)
                .ThenByDescending(it => it.Robots.Clay)
                .ThenByDescending(it => it.Robots.Ore)
                .Take(5_000)
                .ToList();
        }

        return states.Max(s => s.Resources.Geode);
    }

    private IEnumerable<FactoryState> NextStates(FactoryState state, Blueprint blueprint)
    {
        var purchases = GetPurchaseOptions(state.Resources, blueprint);

        foreach (var purchase in purchases)
        {
            var costs = (blueprint.OreRobotCost * purchase.Ore) +
                        (blueprint.ClayRobotCost * purchase.Clay) +
                        (blueprint.ObsidianRobotCost * purchase.Obsidian) +
                        (blueprint.GeodeRobotCost * purchase.Geode);

            var newResources = state.Resources - costs + state.Robots;
            var newRobots = state.Robots + purchase;

            yield return new(newResources, newRobots);
        }
    }

    private int QualityLevel(Blueprint input, int targetMinutes) => input.Id * MaxNumberOfGeodes(input, targetMinutes);

    private readonly record struct Blueprint(int Id, Quantities OreRobotCost, Quantities ClayRobotCost, Quantities ObsidianRobotCost, Quantities GeodeRobotCost);

    private readonly record struct Quantities(int Ore, int Clay, int Obsidian, int Geode)
    {
        public static readonly Quantities Zero = new(0, 0, 0, 0);
        public static readonly Quantities SingleOre = new(1, 0, 0, 0);
        public static readonly Quantities SingleClay = new(0, 1, 0, 0);
        public static readonly Quantities SingleObsidian = new(0, 0, 1, 0);
        public static readonly Quantities SingleGeode = new(0, 0, 0, 1);

        public static Quantities operator +(Quantities a, Quantities b) =>
            new(a.Ore + b.Ore, a.Clay + b.Clay, a.Obsidian + b.Obsidian, a.Geode + b.Geode);

        public static Quantities operator *(Quantities a, int factor) =>
            new(a.Ore * factor, a.Clay * factor, a.Obsidian * factor, a.Geode * factor);

        public static Quantities operator -(Quantities a, Quantities b) =>
            new(a.Ore - b.Ore, a.Clay - b.Clay, a.Obsidian - b.Obsidian, a.Geode - b.Geode);

        public bool CanAfford(Quantities other) => Ore >= other.Ore && Clay >= other.Clay && Obsidian >= other.Obsidian && Geode >= other.Geode;
    }

    private readonly record struct FactoryState(Quantities Resources, Quantities Robots);
}
