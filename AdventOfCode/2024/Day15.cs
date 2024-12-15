namespace AdventOfCode._2024;

using Coordinate = Coordinate<int>;

public class Day15 : Day
{
    public Day15() : base()
    {
    }

    [Answer("10092", Example, Data = "##########{nl}#..O..O.O#{nl}#......O.#{nl}#.OO..O.O#{nl}#..O@..O.#{nl}#O#..O...#{nl}#O..O..O.#{nl}#.OO.O.OO#{nl}#....O...#{nl}##########{nl}{nl}<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^{nl}vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v{nl}><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<{nl}<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^{nl}^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><{nl}^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^{nl}>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^{nl}<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>{nl}^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>{nl}v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^")]
    [Answer("1487337", Regular)]
    public override string SolveA()
    {
        var (grid, moves) = Parse();
        var start = Start(grid);

        _ = moves.Aggregate(start, (current, move) => Walk(grid, current, move));

        return Calculate(grid).ToString();
    }

    [Answer("9021", Example, Data = "##########{nl}#..O..O.O#{nl}#......O.#{nl}#.OO..O.O#{nl}#..O@..O.#{nl}#O#..O...#{nl}#O..O..O.#{nl}#.OO.O.OO#{nl}#....O...#{nl}##########{nl}{nl}<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^{nl}vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v{nl}><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<{nl}<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^{nl}^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><{nl}^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^{nl}>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^{nl}<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>{nl}^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>{nl}v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^")]
    [Answer("1521952", Regular)]
    public override string SolveB()
    {
        var (grid, moves) = Parse();

        //

        var start = new Coordinate();
        var obstacles = new List<Obstacle>();
        var arrays = new char[grid.MaxY][]; // Temp testing..

        for (var y = 0; y < grid.MaxY; y++)
        {
            arrays[y] = new char[grid.MaxX * 2];

            for (var x = 0; x < grid.MaxX; x++)
            {
                if (grid[x, y] is '@') start = new Coordinate(x * 2, y);
                if (grid[x, y] is 'O') obstacles.Add(new Obstacle(new Coordinate(x * 2, y), new Coordinate(x * 2 + 1, y), Type.Box));
                if (grid[x, y] is '#') obstacles.Add(new Obstacle(new Coordinate(x * 2, y), new Coordinate(x * 2 + 1, y), Type.Wall));
            }
        }

        // Temp grid.. for displaying..
        var wideGrid = new Grid<char>(arrays);
        wideGrid.Fill('.');

        // Debugging
        Console.WriteLine();
        Console.WriteLine($"-- {start} --");

        foreach (var obstacle in obstacles)
        {
            if (obstacle.Type == Type.Wall)
            {
                wideGrid[obstacle.First] = '#';
                wideGrid[obstacle.Second] = '#';
            }
            else
            {
                wideGrid[obstacle.First] = '[';
                wideGrid[obstacle.Second] = ']';
            }
        }

        wideGrid[start] = '@';
        // wideGrid.Draw();

        Console.WriteLine();
        Console.WriteLine(".. START ..");

        Direction? previous = null;
        foreach (var (move, i) in moves.Select((m, i) => (m, i)))
        {
            if (i == 465)
            {
                wideGrid.Fill('.');
                foreach (var o in obstacles)
                {
                    if (o.Type == Type.Wall)
                    {
                        wideGrid[o.First] = '#';
                        wideGrid[o.Second] = '#';
                    }
                    else
                    {
                        wideGrid[o.First] = '[';
                        wideGrid[o.Second] = ']';
                    }
                }

                wideGrid[start] = '@';
                wideGrid.Draw();
            }

            start = Walk(obstacles, start, move);

            Console.WriteLine();
            Console.WriteLine($"-- {start} -- {previous} -> {move} -- ");
            previous = move;


            // grid.Draw();

            // File.AppendAllText("./test.txt", $"-- {start} -- {previous} -> {move} -- ");

            wideGrid.Fill('.');
            foreach (var o in obstacles)
            {
                if (o.Type == Type.Wall)
                {
                    wideGrid[o.First] = '#';
                    wideGrid[o.Second] = '#';
                }
                else
                {
                    wideGrid[o.First] = '[';
                    wideGrid[o.Second] = ']';
                }
            }

            wideGrid[start] = '@';

            if (i == 465)
                wideGrid.Draw();

            var sum2 = obstacles
                .Where(o => o.Type == Type.Box)
                .Sum(o => (o.First.Y * 100L) + o.First.X);

            var sum3 = Calculate(wideGrid);

            if (sum2 != sum3)
            {
                Console.WriteLine($"ERORR!!! SUM: {sum2} -> {Calculate(wideGrid)}, {move}: i:{i}");
            }

            Console.WriteLine($"SUM: {sum2} -> {Calculate(wideGrid)}");

            //
        }

        wideGrid.Draw();

        Console.WriteLine($"-- STOP -- => {start}");

        var sum = obstacles
            .Where(o => o.Type == Type.Box)
            .Sum(o => (o.First.Y * 100L) + o.First.X);

        return $"{Calculate(wideGrid)} or {sum}";
    }

    private static Coordinate Start(Grid<char> grid)
    {
        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                if (grid[x, y] != '@')
                    continue;

                grid[x, y] = '.';
                return new Coordinate(x, y);
            }
        }

        throw new Exception("Invalid grid, '@' not found");
    }

    private static long Calculate(Grid<char> grid)
    {
        var total = 0L;

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                // if (grid[x, y] != 'O')
                if (grid[x, y] != '[')
                    continue;

                var gps = (100L * y) + x;
                total += gps;
            }
        }

        return total;
    }

    private static Coordinate Walk(List<Obstacle> obstacles, Coordinate coordinate, Direction direction)
    {
        var dCoordinate = coordinate.Directions[direction];
        var oCoordinates = new List<Coordinate> { dCoordinate };

        var remembers = new List<Obstacle>();
        var failed = false;

        do
        {
            var obstaclesFound = obstacles
                .Where(o => oCoordinates.Any(oc => oc == o.First || oc == o.Second))
                .ToList();

            if (obstaclesFound.Count == 0)
            {
                break;
            }

            if (obstaclesFound.Any(o => o.Type == Type.Wall))
            {
                remembers.Clear();
                failed = true;
                break;
            }

            oCoordinates.Clear();

            foreach (var obstacle in obstaclesFound)
            {
                Console.WriteLine($"Object found!! T:{obstacle.Type}, F:{obstacle.First}, S:{obstacle.Second}");

                if (obstacle.Type == Type.Wall)
                {
                    break;
                }

                int bestX;
                if (direction is Direction.West)
                {
                    bestX = Math.Min(obstacle.First.X, obstacle.Second.X) - 1;
                    oCoordinates.Add(new Coordinate(bestX, coordinate.Y));
                }

                if (direction is Direction.East)
                {
                    bestX = Math.Max(obstacle.First.X, obstacle.Second.X) + 1;
                    oCoordinates.Add(new Coordinate(bestX, coordinate.Y));
                }

                int bestY;
                if (direction is Direction.North)
                {
                    bestY = Math.Min(obstacle.First.Y, obstacle.Second.Y) - 1;
                    oCoordinates.Add(new Coordinate(obstacle.First.X, bestY));
                    oCoordinates.Add(new Coordinate(obstacle.Second.X, bestY));
                }

                if (direction is Direction.South)
                {
                    bestY = Math.Max(obstacle.First.Y, obstacle.Second.Y) + 1;
                    oCoordinates.Add(new Coordinate(obstacle.First.X, bestY));
                    oCoordinates.Add(new Coordinate(obstacle.Second.X, bestY));
                }

                Console.WriteLine($"d: {dCoordinate}, o: {string.Join(',', oCoordinates)}");
                remembers.Add(obstacle);
            }
        } while (true);

        foreach (var remember in remembers)
        {
            Console.WriteLine($"Remember: {remember.Type}, F:{remember.First}, S:{remember.Second}");

            if (direction is Direction.North)
            {
                remember.First = remember.First.Up;
                remember.Second = remember.Second.Up;
            }
            else if (direction is Direction.South)
            {
                remember.First = remember.First.Down;
                remember.Second = remember.Second.Down;
            }
            else if (direction is Direction.East)
            {
                remember.First = remember.First.Right;
                remember.Second = remember.Second.Right;
            }
            else if (direction is Direction.West)
            {
                remember.First = remember.First.Left;
                remember.Second = remember.Second.Left;
            }
        }

        var rValue = failed ? coordinate : dCoordinate;
        Console.WriteLine($"Just walk..or [{failed}] from {coordinate} to {rValue} ({direction})");
        return rValue;
    }

    private void Move(Grid<char> grid, Coordinate coordinate, Direction direction)
    {
        if (direction is Direction.West)
        {
        }
    }

    private Coordinate Walk(Grid<char> grid, Coordinate coordinate, Direction direction)
    {
        var neighbours = grid.Directions(coordinate);

        if (neighbours.TryGetValue(direction, out var neighbour))
        {
            // Console.WriteLine($"Move {direction} from {coordinate} to {neighbour}");

            // geldig!

            var c = grid[neighbour];
            if (c is '.')
            {
                // Console.WriteLine($"Just walk to [{neighbour}]");
                return new Coordinate(neighbour);
            }

            if (c is 'O')
            {
                var remember = new HashSet<Coordinate> { neighbour };

                // Console.WriteLine($"Next step is an 'O' at [{neighbour}]");
                // loop, check if direction has somewhere space for an 'O'
                var end = false;
                while (!end)
                {
                    neighbours = grid.Directions(neighbour);
                    if (neighbours.TryGetValue(direction, out neighbour))
                    {
                        var nc = grid[neighbour];
                        if (nc is '.')
                        {
                            // Console.WriteLine($"Found [{nc}] at {neighbour}!");
                            remember.Add(neighbour);
                            end = true;
                        }
                        else if (nc is 'O')
                        {
                            // Console.WriteLine($"Found [{nc}] at {neighbour}, continue!");
                            remember.Add(neighbour);
                        }
                        else // # is end
                        {
                            // Console.WriteLine($"Found [{nc}] at {neighbour}, failed to move IMPOSSIBLE TO REACH??!");
                            remember.Clear();
                            end = true;
                        }
                    }
                    else
                    {
                        // Console.WriteLine("End of grid");
                        remember.Clear();
                        end = true;
                    }
                }

                foreach (var r in remember.Skip(1))
                {
                    grid[r] = 'O';
                }

                if (remember.Count > 0)
                {
                    var robot = remember.First();
                    grid[robot] = '.';
                    return robot;
                }

                // return coordinate;
            }
        }
        else
        {
            // Console.WriteLine($"Move {direction} from {coordinate} failed");
        }

        return coordinate;
    }

    private static Coordinate Walk2(Grid<char> grid, Coordinate coordinate, Direction direction)
    {
        var neighbours = grid.Directions(coordinate, true);

        if (neighbours.TryGetValue(direction, out var neighbour))
        {
            Console.WriteLine($"Move {direction} from {coordinate} to {neighbour}");

            var c = grid[neighbour];
            if (c is '.')
            {
                Console.WriteLine($"Just walk to [{neighbour}]");
                return new Coordinate(neighbour);
            }

            if (direction is Direction.North)
            {
                var remember = new List<Coordinate> { neighbour };
                Console.WriteLine($"Next step is an '{c}' at [{neighbour}]");

                if (c is '[')
                {
                }
                else if (c is ']')
                {
                    if (neighbours.TryGetValue(Direction.NorthWest, out var neighbourWest))
                        remember.Add(neighbourWest);

                    neighbours = grid.Directions(neighbour, true);
                    if (neighbours.TryGetValue(Direction.North, out var neighbourRight) && neighbours.TryGetValue(Direction.NorthWest, out var neighbourLeft))
                    {
                        var a = X(grid, neighbourRight, ref remember);
                        var b = X(grid, neighbourLeft, ref remember);
                    }
                }
            }
            else if (c is '[' or ']') // Add check for east/west direction?
            {
                var remember = new List<Coordinate> { neighbour };

                Console.WriteLine($"Next step is an '{c}' at [{neighbour}]");
                // loop, check if direction has somewhere space for an 'O'
                var end = false;
                while (!end)
                {
                    neighbours = grid.Directions(neighbour);
                    if (neighbours.TryGetValue(direction, out neighbour))
                    {
                        var nc = grid[neighbour];
                        if (nc is '.')
                        {
                            Console.WriteLine($"Found '{nc}' at {neighbour}!");
                            remember.Add(neighbour);
                            end = true;
                        }
                        else if (nc is '[' or ']')
                        {
                            Console.WriteLine($"Found '{nc}' at {neighbour}, continue! keep {direction} in mind!!");
                            remember.Add(neighbour);
                        }
                        else // # is end
                        {
                            Console.WriteLine($"Found '{nc}' at {neighbour}, failed to move IMPOSSIBLE TO REACH??!");
                            remember.Clear();
                            end = true;
                        }
                    }
                    else
                    {
                        remember.Clear();
                        end = true;
                    }
                }

                for (var i = remember.Count - 2; i >= 0; i--)
                {
                    Console.WriteLine($"Reember1 {i} stuff: {grid[remember[i + 1]]} ({remember[i + 1]}) wordt {grid[remember[i]]} ({remember[i]})");
                    grid[remember[i + 1]] = grid[remember[i]];

                    if (i == 0)
                    {
                        Console.WriteLine($"AND Remeber stuff endd: {grid[remember[i]]} ({remember[i]}) wordt een '.'");
                        grid[remember[i]] = '.';
                    }
                }

                Console.WriteLine();

                if (remember.Count > 0)
                {
                    var robot = remember.First();
                    return robot;
                }
            }
        }

        return coordinate;
    }

    private static bool X(Grid<char> grid, Coordinate neighbour, ref List<Coordinate> remember)
    {
        var nc = grid[neighbour];
        if (nc is '.')
        {
            Console.WriteLine($"Found '{nc}' at {neighbour}!");
            remember.Add(neighbour);
            return true;
        }

        if (nc is '[' or ']')
        {
            Console.WriteLine($"Found '{nc}' at {neighbour}, continue!");
            remember.Add(neighbour);
            return false;
        }

        // # is end
        Console.WriteLine($"Found '{nc}' at {neighbour}, failed to move IMPOSSIBLE TO REACH??!");
        remember.Clear();
        return true;
    }

    private (Grid<char>, Direction[]) Parse()
    {
        var input = GetSplitInput(false).ToList();
        var empty = input.IndexOf(string.Empty);

        var grid = input
            // .Skip(1) // Skip header
            .Take(empty) // Skip footer
            // .Take(empty - 2) // Skip footer
            .Select(s => s
                    .ToCharArray()
                // .Skip(1) // Skip starting border
                // .SkipLast(1) // Skip ending border
                // .ToArray()
            ).ToArray();

        var moves = input
            .Skip(empty + 1)
            .SelectMany(s => s
                .ToCharArray()
                .Select(c => c switch
                {
                    '^' => Direction.North,
                    '>' => Direction.East,
                    'v' => Direction.South,
                    '<' => Direction.West,
                    _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
                })
            ).ToArray();

        return (new Grid<char>(grid), moves);
    }

    private (Grid<char>, Direction[]) ParseB()
    {
        var input = GetSplitInput(false).ToList();
        var empty = input.IndexOf(string.Empty);

        var grid = input
            .Skip(1) // Skip header
            .Take(empty - 2) // Skip footer
            .Select(s => s
                // .Replace("#", "##")
                // .Replace("O", "[]")
                // .Replace(".", "..")
                // .Replace("@", "@.")
                .ToCharArray()
                .Skip(2) // Skip starting border
                .SkipLast(2) // Skip ending border
                .ToArray()
            ).ToArray();

        var moves = input
            .Skip(empty + 1)
            .SelectMany(s => s
                .ToCharArray()
                .Select(c => c switch
                {
                    '^' => Direction.North,
                    '>' => Direction.East,
                    'v' => Direction.South,
                    '<' => Direction.West,
                    _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
                })
            ).ToArray();

        return (new Grid<char>(grid), moves);
    }

    private class Obstacle(Coordinate first, Coordinate second, Type type)
    {
        public Coordinate First { get; set; } = first;
        public Coordinate Second { get; set; } = second;
        public Type Type { get; set; } = type;
    }

    private enum Type
    {
        Wall,
        Box
    }
}
