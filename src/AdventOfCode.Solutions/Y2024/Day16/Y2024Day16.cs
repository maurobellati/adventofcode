namespace AdventOfCode.Y2024.Day16;

using Tool;
using Costs = Dictionary<PathStep, int>;
using DijkstraResult = (Dictionary<PathStep, int> Costs, Dictionary<PathStep, HashSet<PathStep>> Parents);

public class Y2024Day16 : Solver
{
    private const int MoveForwardCost = 1;
    private const int RotateCost = 1000;

    public object PartOne(List<string> input) => GetLowestScore(GridFactory.Create(input));

    public object PartTwo(List<string> input) => GetBestSpots(GridFactory.Create(input));

    private static int GetBestSpots(Grid<char> grid)
    {
        var goal = Goal(grid);
        var dijkstra = RunDijkstra(grid, Start(grid), goal);

        var bestGoalScore = GetLowerCostAt(dijkstra.Costs, goal);

        // Start from ALL the best steps at the goal (since they can be multiple) and go back to the start
        var bestSpots = dijkstra.Costs.Where(kv => kv.Key.Cell == goal && kv.Value == bestGoalScore).Select(kv => kv.Key).ToList();

        var queue = new Queue<PathStep>([..bestSpots]);
        while (queue.Count > 0)
        {
            var step = queue.Dequeue();
            bestSpots.Add(step);

            foreach (var parent in dijkstra.Parents[step]
                         .Where(parent => IsBestParent(dijkstra.Costs, parent, step))) // Some direct parents coming from a suboptimal path, may have higher costs than the optimal path.
            {
                queue.Enqueue(parent);
            }
        }

        return bestSpots.DistinctBy(it => it.Cell).Count();
    }

    private static int GetCostFromParentToChild(PathStep parent, PathStep child) => GetNextSteps(parent).Single(kv => kv.Step == child).Cost;

    private static int GetLowerCostAt(Costs costs, Cell goal) => costs.Where(kv => kv.Key.Cell == goal).Min(kv => kv.Value);

    private static int GetLowestScore(Grid<char> grid)
    {
        var dijkstra = RunDijkstra(grid, Start(grid), Goal(grid));
        return GetLowerCostAt(dijkstra.Costs, Goal(grid));
    }

    private static IEnumerable<(PathStep Step, int Cost)> GetNextSteps(PathStep step)
    {
        yield return (step.Move(), MoveForwardCost);
        yield return (step.Rotate(Rotation.Clockwise).Move(), RotateCost + MoveForwardCost);
        yield return (step.Rotate(Rotation.CounterClockwise).Move(), RotateCost + MoveForwardCost);
    }

    private static Cell Goal(Grid<char> grid) => grid.Entries.Single(e => e.Value == 'E').Cell;

    private static bool IsBestParent(Costs costs, PathStep parent, PathStep child) => costs[parent] + GetCostFromParentToChild(parent, child) == costs[child];

    private static DijkstraResult RunDijkstra(Grid<char> grid, PathStep start, Cell end)
    {
        Costs costs = new() { [start] = 0 };
        PriorityQueue<PathStep, int> queue = new([(start, 0)]); // Use a priority queue to explore the cells with the lowest cost first
        Dictionary<PathStep, HashSet<PathStep>> parents = new() { [start] = [] };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current.Cell == end)
            {
                continue; // Console.WriteLine($"Goal found at {current.Cell} with cost {costs[current]}");
            }

            foreach (var (nextStep, moveCost) in GetNextSteps(current))
            {
                if (grid.ValueAt(nextStep.Cell) == '#') // Skip if we hit a wall
                {
                    continue;
                }

                var newCost = costs[current] + moveCost;
                if (costs.TryGetValue(nextStep, out var previousCost) && newCost > previousCost) // Skip if we already visited this cell with a lower cost
                {
                    continue;
                }

                costs[nextStep] = newCost;
                parents.Add(nextStep, current); // Save the parent step to reconstruct the path later
                queue.Enqueue(nextStep, newCost);
            }
        }

        return (costs, parents);
    }

    private static PathStep Start(Grid<char> grid) => new(grid.Entries.Single(e => e.Value == 'S').Cell, Direction.E);
}
