namespace AdventOfCode.Y2022.Day17;

using Tool;

public class Y2022Day17 : Solver
{
    public object PartOne(List<string> input)
    {
        var jetPatterns = input.First();
        // Console.WriteLine($"Jet Patterns count: {jetPatterns.Length}");
        var cave = new Cave(jetPatterns);
        for (var i = 0; i < 2022; i++)
        {
            cave.DropRock();
        }

        return cave.TowerHeight;
    }

    public object PartTwo(List<string> input)
    {
        var jetPatterns = input.First();
        // Console.WriteLine($"Jet Patterns count: {jetPatterns.Length}");
        var cave = new Cave(jetPatterns);

        // Dictionary<(int RockIndex, int JetPatternIndex), int> stateToTowerHeight = [];

        List<int> towerHeights = [0];
        List<(int RockIndex, int JetPatternIndex)> states = [(0,0)];
        var targetRockCount = 1000000000000;
        for (var i = 0L; i < targetRockCount; i++)
        {
            cave.DropRock();


            var currentState = (cave.CurrentRockIndex, cave.JetPatternIndex);
            if (states.Contains(currentState))
            {
                break;
            }

            towerHeights.Add(cave.TowerHeight);
            states.Add(currentState);
        }

        var cycleStart = states.IndexOf((cave.CurrentRockIndex, cave.JetPatternIndex));
        var cycleLength = states.Count - cycleStart;
        var cycleStartHeight = towerHeights[cycleStart];

        var cycleHeight = cave.TowerHeight - cycleStartHeight;

        var remainingRockCount = targetRockCount - cycleStart;

        var cyclesCount = remainingRockCount / cycleLength;

        var leftoverRockCount = (int)(remainingRockCount % cycleLength);

        var skippedCyclesHeight = cyclesCount * cycleHeight;
        var remainingCyclesHeight = towerHeights[cycleStart + leftoverRockCount] - cycleStartHeight;

        var result = cycleStartHeight + skippedCyclesHeight + remainingCyclesHeight;
        return result;
    }

    private sealed record Rock(int Height, List<Cell> Cells);

    private sealed class Cave(string jetPatterns)
    {
        private static readonly int Width = 7;

        private readonly List<bool[]> grid = [];

        private readonly List<Rock> rocks =
        [
            ParseRock("####"),
            ParseRock(" # \n###\n # "),
            ParseRock("  #\n  #\n###"),
            ParseRock("#\n#\n#\n#"),
            ParseRock("##\n##")
        ];

        private Cell currentRockPosition = Cell.Origin;

        private Rock CurrentRock => rocks[CurrentRockIndex];

        public int CurrentRockIndex => RockCount % rocks.Count;

        public int JetPatternIndex { get; private set; }

        public int RockCount { get; private set; }

        public int TowerHeight { get; private set; }

        private static IEnumerable<Cell> GetRockCells(Rock rock, Cell position) =>
            rock.Cells.Select(it => new Cell(position.Row - it.Row, position.Col + it.Col));

        private static Rock ParseRock(string piece)
        {
            var lines = piece.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var cells = new List<Cell>();
            for (var row = 0; row < lines.Length; row++)
            {
                for (var col = 0; col < lines[row].Length; col++)
                {
                    if (lines[row][col] == '#')
                    {
                        cells.Add(new(row, col));
                    }
                }
            }

            return new(lines.Length, cells);
        }

        public void DropRock()
        {
            InitRock();

            if (JetPatternIndex == 0)
            {
                // Console.WriteLine($"### Starting rock {RockCount:D5}. Jet index: {JetPatternIndex}");
            }

            // Console.WriteLine("\n### A new rock begins falling:");
            PrettyPrint();
            while (true)
            {
                TryMoveHorizontally();

                if (TryMoveDown())
                {
                    // Console.WriteLine("Rock falls 1 unit down");
                }
                else
                {
                    // Console.WriteLine("Rock falls 1 unit, causing it to come to rest:");
                    FreezeRock();
                    break;
                }
            }

            PrettyPrint();

            var rockIndex = rocks.IndexOf(CurrentRock);
            if (Math.Abs(JetPatternIndex - jetPatterns.Length) < 5)
            {
                // Console.WriteLine($"### Rock {RockCount:D5} ({rockIndex}) done. Jet index: {JetPatternIndex}");
            }

            RockCount++;
        }

        private bool CanMoveTo(Cell rockPosition) =>
            GetRockCells(CurrentRock, rockPosition)
                .All(
                    cell => cell.Col >= 0 && cell.Col < Width && cell.Row >= 0 // inside the grid
                            && !grid[cell.Row][cell.Col] // not occupied
                );

        private void FreezeRock()
        {
            foreach (var cell in GetRockCells(CurrentRock, currentRockPosition))
            {
                grid[cell.Row][cell.Col] = true;
            }

            TowerHeight = Math.Max(TowerHeight, currentRockPosition.Row + 1);
            // Console.WriteLine($"Updated Tower Height: {TowerHeight}");
        }

        private Direction GetNextDir()
        {
            var dir = jetPatterns[JetPatternIndex] == '>' ? Direction.E : Direction.W;
            JetPatternIndex = (JetPatternIndex + 1) % jetPatterns.Length;
            return dir;
        }

        private void InitRock()
        {
            var rockTopRow = TowerHeight + 3 + CurrentRock.Height - 1;

            var linesCount = rockTopRow + 1 - grid.Count;
            for (var i = 0; i < linesCount; i++)
            {
                grid.Add(new bool[Width]);
            }

            currentRockPosition = new(rockTopRow, 2);
        }

        private void PrettyPrint()
        {
            // var rockCells = GetRockCells(currentRock, currentRockPosition).ToHashSet();
            // for (var row = grid.Count - 1; row >= 0; row--)
            // {
            //     for (var col = 0; col < Width; col++)
            //     {
            //         var symbol = rockCells.Contains(new(row, col)) ? '@' : grid[row][col] ? '#' : '.';
            //         {
            //             // Console.Write(symbol);
            //         }
            //     }
            //
            //     // Console.WriteLine();
            // }
        }

        private bool TryMoveDown() => TryMoveTo(currentRockPosition with { Row = currentRockPosition.Row - 1 });

        private bool TryMoveHorizontally() => TryMoveTo(currentRockPosition.Move(GetNextDir()));

        private bool TryMoveTo(Cell position)
        {
            if (!CanMoveTo(position))
            {
                return false;
            }

            currentRockPosition = position;
            return true;
        }
    }
}
