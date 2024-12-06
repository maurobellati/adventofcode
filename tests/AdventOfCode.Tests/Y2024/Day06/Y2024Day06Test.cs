namespace AdventOfCode.Tests.Y2024.Day06;

using AdventOfCode.Y2024.Day06;

public class Y2024Day06Test : SolverTest<Y2024Day06>
{
    private static readonly string Input = """
                                           ....#.....
                                           .........#
                                           ..........
                                           ..#.......
                                           .......#..
                                           ..........
                                           .#..^.....
                                           ........#.
                                           #.........
                                           ......#...
                                           """;

    private static readonly string Real = """
                                           ..........................#.#........................................#..........................................#.............#...
                                           ....................#...............................#.......#...............................#..........................#...#......
                                           .....#.......#...................................................#..#....................##.........#...#...........#.....#..#.#..
                                           ............#...#....#..............##...#.#....#........................#...................................................#....
                                           ....#......##........................................................................#..#..##...................##.........#......
                                           .............##.........#..##..............#...............#....................#..................#......#...................#...
                                           ...........#.....................#.......#.................#....#.#..............#...........#.........................#..........
                                           .#............#..............................................................#........#................#........#.................
                                           ..........................#..#.......................................................#.........#..................................
                                           .........#....#................#...........#...................##............#..........#.........................................
                                           ....#...................................#....#.........#..........#.......................#..............................#........
                                           .............................#.....##.............................................................#..#...........................#
                                           ....#................#...#.#..........#.#....#..................#..........#......#.............................#.................
                                           ......................#............#......................................##................#..........#..#.............#.........
                                           ..#........#............#.....###....##...........................#..............................#.....#..........................
                                           .....#...........#..#.........#...................#.#.................#........#............#.....#........#......................
                                           ..........................................................................................................................#.......
                                           ..#...................#.....................................................................#....................#................
                                           ...............#............................#....#..........#...................#............#...............#..#.................
                                           ................#...........................#...........................................#.........................#...............
                                           ..........#.......#.........................##.................................................#...................##..#..........
                                           .#.........#...#..#............................................#........................#.......................................#.
                                           ........#...........###..................#...........#..............#.....................#................#......................
                                           .............#....#............................#.#..............................#..#................................#.............
                                           ...#.....#..........#...........#..................................................#........#...........#..#..........##.....#....
                                           ...............................................................................#...................#........................##....
                                           ......#.#...........................#.................#.................................................................#.........
                                           ..................................#............................................................................#.#................
                                           ........................#.......#.....#..........#..#.............##.................#..............#............##...............
                                           ......#........................#........................................#......#..................................................
                                           ...............##.......#..............................................................................................#..........
                                           ....#...................................................................................#.........#..........#..............#.....
                                           .....#.............#.........................#.........#........#................#.#.........................#..........#.........
                                           .............#......##...........#................#...#.#.....#......................................#..........#.................
                                           ...........................#............#................#................#...........................#.....#.....................
                                           .......#.................#..#..............#................................#....#........#.......#.............#....#....##......
                                           ........................................................#........................................................................#
                                           .................#.........................................#.......#.......#......#...#.#.................##......................
                                           ..........#............#..............................................#........................................#..........#..#....
                                           ..............#......................................................#......#................................#....................
                                           ...#........#.............#.................#......#.......................#..............................##........#.............
                                           .....................#............................................................#............#...............................#..
                                           ......#.....#..............................................................#...............#...#..................................
                                           .......................................#..#.......#...#..............#.................................................#..........
                                           ...................................................................................................#........................#.....
                                           ....#.....#.......................................................#.....#......#...................#............#.................
                                           ..............#....#.....................#........................................##....#...#.....................................
                                           .....#...............................................................#............#............................#..................
                                           ........................#..........................#..............................................................................
                                           #..............#...........................................................................#......................................
                                           ...................................#...#....................................................................#.....................
                                           ................#......#.........#.................#...........................#..................................................
                                           ......#...#................#...............#..................................................#...................................
                                           .......................#.............#..#....................#............#.......................................................
                                           .....................#.#.....................................#................................##.............#.#..#..............#
                                           .....................................#...#...........................#....................................................##...#..
                                           .................#..#............#.............................#..#................................................##.............
                                           ...#...........................................#.................................................#.............#.............##...
                                           ..................#....#....#.#..........................#.............#................#............................#............
                                           .........................#................#.#..#.#............^.........................................................#.........
                                           .....#...............#............................................................................................................
                                           .#.......................................................................................#...............#....#.............#.....
                                           ......#................................#............#............................................................#................
                                           .#.....................#..............#.............#.......................#.....#....#.#.#...................#....#.............
                                           ..................................................................#........#............................#..#.#...............#....
                                           .#.....................#......#........#.......#.................#.......................#..#.....................................
                                           ..............#..#..#.............................................#....................#......#...................................
                                           .................##.#............................##..#...............................................................#....#.......
                                           ...........#..#........................#.#..........#.................##.....#...#.........................#.#...#............#...
                                           .....................#..........#..#..............................................................................................
                                           .......#...............................#..#.........#.....................................#......#...................#............
                                           ..............................##.#........#.......#..#.....#......................................................................
                                           ..................#......................#.....................................###...#............................##..............
                                           ...................................#...................................#...........................#....##..#.....................
                                           .....................#.....#.#.........#.........................#.....#.......#..........#.......................#...............
                                           ...........#..................................................................................................#...................
                                           ......#............................#.............................#..........................................#.....##..............
                                           ...........................#...........................................................#.............................#..........#.
                                           ..........................................#..........................................#......#...............#.....................
                                           ..............##.........#........................................................................................................
                                           .....................#............#..#......#..#......................................................................#...........
                                           ........#.........#.....................#.................................#...#.#....#....#......................#.#..............
                                           ......#...........#............................................................#..............#......................#............
                                           .....#.....................#.....#.........#.............................#....#............................#......................
                                           ..#..............................................................................................................#...#............
                                           ..............#.......................#........#............................................#.......................#.............
                                           ........................................#....................................................................................##...
                                           .......#..#.......#.................#.#.................#...........#............#........#.#.........#...........#.#..........#..
                                           .....##.......................#.........................#.........................#...........#.........#.....#...........#.......
                                           ..........................#.....................#...............................#.............................#...................
                                           .....#..........#.............#...#..........................#.#...................#...........................#......#...........
                                           ......................................#.......#.........#......................................#..................................
                                           ......#...#....................#................#...#.......#.............#............#...........#.....#........................
                                           ........................................#............#......................................................#............#........
                                           ..#.......#....#..........................................................................#......................##..#.......#....
                                           ................#...................................................#..........................................#..................
                                           ...........#........................................................................................#.............................
                                           ..................#........................................#.....#...........................#..................#.................
                                           ........#.......#.....#...........#.......................................................#.........#.............................
                                           .............#.....#.#............................#...................................................................#...........
                                           ...............#...................................#...........................#.............#..#..................#..##..........
                                           .#...............................#....#.....................#...................................#...................#...#.........
                                           ..................#..#.#..................................................................................................#.......
                                           .........#.....#...#.........#........#...........................................#..................#.#.#......#.................
                                           .........#......#.......................#........#.............................#...#................#...........................#.
                                           ...#.........................................................................................##........................#..........
                                           .....................................#...........................................................#.#...#......#...............#...
                                           #.....................#.........#.................................................#.....................................#.........
                                           ............##.............................#...............................................#......................................
                                           .#.................#........................#.................................................................#.....#.............
                                           .....#.......................#......................#.................#....#....................................................#.
                                           .............................#..................................................................#.#...............................
                                           ........#.......#.....##..........................................#...................#.........#...........#...#...#...#.........
                                           ..........#.#...#.....................#............................................................##.........##..................
                                           ...................#..............##.....#...........#.#.......#................................................#...............#.
                                           ............................................................#....#.............#....##.......#...#...#................#...........
                                           ......#...............................#.........#...................#....#..............#.................#.......................
                                           ....................................#.....................#....#....#.................#...............#...........................
                                           .................................#.............#.#...................................#..............#...#.....#.............#.....
                                           ......................#..................#.............#.................#................................................#.......
                                           .....................##.......................#.............#...#........#..........#.....#............................##...#.....
                                           ..........................#.....................#...........#.....................................................#...............
                                           ................#.....................................................................................................#...........
                                           ........#......#.......................#....................................#................................#....................
                                           .....................................#............................................................................................
                                           ........#..........#.........#.#................##................#...#..........................#.........#......##.......#......
                                           .......#...................#................................#..#.......................#..............................#...........
                                           ............#.....................................#......#........................#..........#.............................#......
                                           ..#...........#...........#...................................#............#...#.....#......................................#.....
                                           ................................................................#...........#.......##.......#..........#............#............
                                           """;

    protected override IEnumerable<TestCase> PartOnes() => [new(Input, 41), new(Real, 5162)];

    protected override IEnumerable<TestCase> PartTwos() => [new(Input, 6)];
}
