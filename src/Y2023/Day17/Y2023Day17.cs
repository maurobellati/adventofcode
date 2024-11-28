namespace AdventOfCode.Y2023.Day17;

using AdventOfCodeDotNet;
using MinMax = (int Min, int Max);

public class Y2023Day17 : Solver
{
    public object PartOne(List<string> input) => Solve(input, (0, 3));

    public object PartTwo(List<string> input) => Solve(input, (4, 10));

    private static (Node Node, int Cost, Dictionary<Node, Node> Parents) Dijkstra(Grid<int> grid, Cell start, Cell goal, MinMax sameDirectionMinMaxSteps)
    {
        PriorityQueue<(Node Node, int Cost), int> queue = new();
        HashSet<Node> processed = [];
        Dictionary<Node, int> costsAtNode = [];
        Dictionary<Node, Node> parents = [];

        var startNode = new Node(start, null);
        queue.Enqueue((startNode, 0), 0);

        while (queue.Count > 0)
        {
            var (currentNode, currentCost) = queue.Dequeue();
            // Console.WriteLine("\nProcessing ({0}, {1}, Cost={2})", currentNode.Cell, currentNode.History, currentCost);

            if (currentNode.Cell == goal && currentNode.History?.Count >= sameDirectionMinMaxSteps.Min)
            {
                // Console.WriteLine("Found goal! Cost={0}. Processed {1} nodes", currentCost, processed.Count);
                return (Node: currentNode, Cost: currentCost, parents);
            }

            foreach (var direction in GetAllowedDirections(currentNode, sameDirectionMinMaxSteps))
            {
                var nextCell = currentNode.Cell.Move(direction);
                if (!grid.Contains(nextCell))
                {
                    continue;
                }

                var history = currentNode.History?.Direction == direction ? currentNode.History with { Count = currentNode.History.Count + 1 } : new(direction, 1);
                var nextNode = new Node(nextCell, history);
                if (processed.Contains(nextNode))
                {
                    // skip already processed nodes
                    // Console.WriteLine("  Skipping ({0}, {1})", nextNode.Cell, nextNode.History);
                    continue;
                }

                var nextCost = currentCost + grid.ValueAt(nextCell);
                if (costsAtNode.TryGetValue(nextNode, out var bestCost) && bestCost <= nextCost)
                {
                    // skip nodes with same cell and history but higher cost
                    // Console.WriteLine("  Skipping ({0}, {1}, Cost={2}) because high cost", nextNode.Cell, nextNode.History, nextCost);
                    continue;
                }

                // Console.WriteLine("  Adding ({0}, {1}, Cost={2})", nextNode.Cell, nextNode.History, nextCost);
                queue.Enqueue((nextNode, nextCost), nextCost);
                parents[nextNode] = currentNode;
                costsAtNode[nextNode] = nextCost;
            }

            // mark current node as processed, after all neighbors have been visited
            processed.Add(currentNode);

            if (processed.Count % 100 == 0)
            {
                // Console.WriteLine("Processed {0} nodes", processedNodes.Count);
            }
        }

        throw new InvalidOperationException("No path found");
    }

    private static IEnumerable<Direction> GetAllowedDirections(Node node, MinMax sameDirectionMinMaxSteps) =>
        node.History is null
            ? Direction.GetAll()
            : Direction.GetAll()
                .Where(d => d != node.History.Direction.Opposite()) // disallow going back
                .Where(d => d == node.History.Direction || node.History.Count >= sameDirectionMinMaxSteps.Min) // disallow short runs in same direction
                .Where(d => d != node.History.Direction || node.History.Count < sameDirectionMinMaxSteps.Max); // disallow long runs in same direction

    private static Grid<int> ParseGrid(List<string> input) =>
        GridFactory.Create(input.Select(line => line.Select(@char => (int)char.GetNumericValue(@char))));

    private static int Solve(List<string> input, MinMax sameDirectionMinMaxSteps)
    {
        var grid = ParseGrid(input);
        var start = Cell.Origin;
        var goal = new Cell(grid.Rows - 1, grid.Cols - 1);

        var result = Dijkstra(grid, start, goal, sameDirectionMinMaxSteps);

        return result.Cost;
    }

    private sealed record Node(Cell Cell, DirectionHistory? History)
    {
        public override string ToString() => $"Node({Cell}, {History})";
    }

    private sealed record DirectionHistory(Direction Direction, int Count)
    {
        public override string ToString() => $"History({Direction}:{Count})";
    }
}
