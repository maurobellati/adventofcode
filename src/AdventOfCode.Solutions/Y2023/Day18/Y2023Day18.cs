// namespace AdventOfCode.Y2023.Day18;
//
// using System.Globalization;
// using AdventOfCodeDotNet;
//
// public class Y2023Day18 : Solver
// {
//     public object PartOne(List<string> input) => Part1.Solve(input);
//
//     public object PartTwo(List<string> input) => Part2.Solve(input);
//
//     private static class Part1
//     {
//         public static int Solve(List<string> input)
//         {
//             var instructions = input
//                 .Select(ParseInstruction)
//                 .ToList();
//
//             var loop = BuildLoop(instructions);
//             return FindTotalAreaByFlooding(loop);
//         }
//
//         internal static Instruction ParseInstruction(string line)
//         {
//             // line is in the format: "R 6 (#70c710)"
//             var parts = line.Split(' ');
//             return new(
//                 parts[0] switch
//                 {
//                     "R" => Direction.E,
//                     "L" => Direction.W,
//                     "U" => Direction.N,
//                     "D" => Direction.S,
//                     _ => throw new ArgumentException($"Invalid direction at line: {line}")
//                 },
//                 parts[1].ToInt());
//         }
//
//         private static List<Cell> BuildLoop(List<Instruction> instructions)
//         {
//             var start = new Cell(0, 0);
//             // for each instruction, add the cells to the list
//             List<Cell> cells = [start];
//             foreach (var instruction in instructions)
//             {
//                 var lastCell = cells.Last();
//                 for (var step = 1; step <= instruction.Value; step++)
//                 {
//                     cells.Add(lastCell.Move(instruction.Direction, step));
//                 }
//             }
//
//             if (cells.Last() != start)
//             {
//                 throw new ArgumentException("The loop does not end at the start");
//             }
//
//             return cells[..^1];
//         }
//
//         private static int FindTotalAreaByFlooding(List<Cell> loop)
//         {
//             // for each point diagonally near the origin, try to flood the space until reach the loop
//             var origin = loop.First();
//             foreach (var start in new[] { origin.Move(1, 1), origin.Move(-1, 1), origin.Move(1, -1), origin.Move(-1, -1) })
//             {
//                 var area = Flood(start, loop);
//                 if (area > 0)
//                 {
//                     return area + loop.Count;
//                 }
//             }
//
//             throw new ArgumentException("Unable to find a valid area");
//         }
//
//         private static int Flood(Cell start, List<Cell> loop)
//         {
//             HashSet<Cell> visited = [];
//             Queue<Cell> queue = new();
//
//             queue.Enqueue(start);
//             while (queue.Count > 0)
//             {
//                 if (visited.Count > loop.Count * loop.Count / 16)
//                 {
//                     // over flooding
//                     return 0;
//                 }
//
//                 var cell = queue.Dequeue();
//                 if (visited.Contains(cell))
//                 {
//                     continue;
//                 }
//
//                 if (loop.Contains(cell))
//                 {
//                     continue;
//                 }
//
//                 visited.Add(cell);
//
//                 foreach (var direction in Direction.GetAll())
//                 {
//                     queue.Enqueue(cell.Move(direction));
//                 }
//             }
//
//             return visited.Count;
//         }
//     }
//
//     public static class Part2
//     {
//         public static long Solve(List<string> input)
//         {
//             var instructions = input.Select(ParseInstruction).ToList();
//
//             var loop = BuildLoop(instructions);
//             var loopWithSubsegments = SplitWithSubSegments(loop);
//
//             // find all the rectangles inside the loop
//             var rectangles = BuildRectangles(loopWithSubsegments).Where(it => IsInside(loopWithSubsegments, it)).ToList();
//
//             // calculate the area of the rectangles without the borders to avoid double counting
//             var rectangleAreaWithoutBorders = rectangles.Sum(it => it.InnerArea);
//
//             // calculate the area of the segments without the endpoints to avoid double counting
//             var allSegments = rectangles.SelectMany(it => it.Segments()).Distinct().ToList();
//             var segmentsAreaWithoutEndpoints = allSegments.Select(it => it.Length - 2L).Sum();
//
//             // calculate the area of the endpoints (each endpoint is a 1x1 rectangle)
//             var endpointsArea = allSegments.SelectMany(it => it.Cells).Distinct().Count();
//
//             // return the total area
//             return rectangleAreaWithoutBorders + segmentsAreaWithoutEndpoints + endpointsArea;
//         }
//
//         private static List<Segment> BuildLoop(List<Instruction> instructions)
//         {
//             var start = new Cell(0, 0);
//             var result = new List<Segment>();
//             foreach (var instruction in instructions)
//             {
//                 var end = start.Move(instruction.Direction, instruction.Value);
//                 result.Add(new(start, end));
//                 start = end;
//             }
//
//             return result;
//         }
//
//         private static IEnumerable<Rectangle> BuildRectangles(List<Segment> loop)
//         {
//             var cells = loop.SelectMany(segment => segment.Cells).ToList();
//             var allCols = cells.Select(it => it.Col).Distinct().Order().ToList();
//             var allRows = cells.Select(it => it.Row).Distinct().Order().ToList();
//
//             for (var rowIndex = 0; rowIndex < allRows.Count - 1; rowIndex++)
//             {
//                 for (var colIndex = 0; colIndex < allCols.Count - 1; colIndex++)
//                 {
//                     var topLeft = new Cell(allRows[rowIndex], allCols[colIndex]);
//                     var bottomRight = new Cell(allRows[rowIndex + 1], allCols[colIndex + 1]);
//                     yield return new(topLeft, bottomRight);
//                 }
//             }
//         }
//
//         private static bool IsInside(List<Segment> loop, Rectangle rectangle)
//         {
//             var col = (rectangle.TopLeft.Col + rectangle.BottomRight.Col) / 2;
//             var center = new Cell((rectangle.TopLeft.Row + rectangle.BottomRight.Row) / 2, col);
//             return IsInside(loop, center);
//         }
//
//         private static bool IsInside(List<Segment> loop, Cell cell)
//         {
//             // cast a ray from the cell to the right and count the number of segments that:
//             var count = loop
//                 .Where(segment => segment.A.Col == segment.B.Col) // are vertical
//                 .Where(segment => segment.A.Col > cell.Col) // are at the right of the cell
//                 .Where(segment => cell.Row.Between(segment.A.Row, segment.B.Row)) // intersects the horizontal ray
//                 .ToList();
//             // if the number of segments is odd, the cell is inside the loop
//             return count.Count % 2 == 1;
//         }
//
//         private static Instruction ParseInstruction(string line)
//         {
//             // lines in the format: "R 6 (#70c710)"
//             // Each hexadecimal code is six hexadecimal digits long.
//             // The first five hexadecimal digits encode the distance in meters as a five-digit hexadecimal number.
//             // The last hexadecimal digit encodes the direction to dig: 0 means R, 1 means D, 2 means L, and 3 means U.
//             var code = line.Split(' ')[2];
//             var metersInHex = code[2..^2];
//             var directionCode = code[^2..^1];
//             return new(
//                 directionCode switch
//                 {
//                     "0" => Direction.E,
//                     "1" => Direction.S,
//                     "2" => Direction.W,
//                     "3" => Direction.N,
//                     _ => throw new ArgumentException($"Invalid direction: {directionCode}")
//                 },
//                 int.Parse(metersInHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
//         }
//
//         private static List<Segment> SplitWithSubSegments(List<Segment> loop)
//         {
//             // build a grid of all the cells and split each segment in subsegments
//             var cells = loop.SelectMany(segment => segment.Cells).ToList();
//             var allXs = cells.Select(it => it.Col).Distinct().Order().ToList();
//             var allYs = cells.Select(it => it.Row).Distinct().Order().ToList();
//             return loop.SelectMany(segment => SplitWithSubSegments(segment, allXs, allYs)).ToList();
//         }
//
//         private static IEnumerable<Segment> SplitWithSubSegments(Segment segment, List<int> allXs, List<int> allYs)
//         {
//             if (segment.IsVertical)
//             {
//                 // vertical segment
//                 var col = segment.A.Col;
//                 var rowMin = Math.Min(segment.A.Row, segment.B.Row);
//                 var rowMax = Math.Max(segment.A.Row, segment.B.Row);
//                 var rowMinIndex = allYs.IndexOf(rowMin);
//                 var rowMaxIndex = allYs.IndexOf(rowMax);
//                 for (var rowIndex = rowMinIndex; rowIndex < rowMaxIndex; rowIndex++)
//                 {
//                     var b = new Cell(allYs[rowIndex + 1], col);
//                     yield return new(new(allYs[rowIndex], col), b);
//                 }
//             }
//             else
//             {
//                 // horizontal segment
//                 var row = segment.A.Row;
//                 var colMin = Math.Min(segment.A.Col, segment.B.Col);
//                 var colMax = Math.Max(segment.A.Col, segment.B.Col);
//                 var colMinIndex = allXs.IndexOf(colMin);
//                 var colMaxIndex = allXs.IndexOf(colMax);
//                 for (var colIndex = colMinIndex; colIndex < colMaxIndex; colIndex++)
//                 {
//                     var b = new Cell(row, allXs[colIndex + 1]);
//                     yield return new(new(row, allXs[colIndex]), b);
//                 }
//             }
//         }
//     }
//
//     public record Segment(Cell A, Cell B)
//     {
//         public Cell A { get; } = A <= B ? A : B;
//
//         public Cell B { get; } = A <= B ? B : A;
//
//         public IEnumerable<Cell> Cells => new[] { A, B };
//
//         public bool IsHorizontal => A.Row == B.Row;
//
//         public bool IsVertical => A.Col == B.Col;
//
//         public int Length => IsVertical ? B.Row - A.Row + 1 : B.Col - A.Col + 1;
//     }
//
//     internal class Rectangle : ValueObject
//     {
//         public Rectangle(Cell topLeft, Cell bottomRight)
//         {
//             var swapped = topLeft > bottomRight;
//             TopLeft = swapped ? bottomRight : topLeft;
//             BottomRight = swapped ? topLeft : bottomRight;
//         }
//
//         public Cell BottomRight { get; }
//
//         public long InnerArea => (BottomRight.Col - TopLeft.Col - 1L) * (BottomRight.Row - TopLeft.Row - 1L);
//
//         public Cell TopLeft { get; }
//
//         public override string ToString() => $"({TopLeft} -> {BottomRight})";
//
//         public IEnumerable<Segment> Segments()
//         {
//             yield return new(TopLeft, new(TopLeft.Row, BottomRight.Col));
//             yield return new(new(TopLeft.Row, BottomRight.Col), BottomRight);
//             yield return new(BottomRight, new(BottomRight.Row, TopLeft.Col));
//             yield return new(new(BottomRight.Row, TopLeft.Col), TopLeft);
//         }
//
//         protected override IEnumerable<IComparable> GetEqualityComponents()
//         {
//             yield return TopLeft.Col;
//             yield return TopLeft.Row;
//             yield return BottomRight.Col;
//             yield return BottomRight.Row;
//         }
//     }
//
//     private sealed record Instruction(Direction Direction, int Value);
// }


