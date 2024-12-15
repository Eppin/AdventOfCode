namespace AdventOfCode._2024;

using Coordinate = Coordinate<int>;

public class Day15 : Day
{
    public Day15() : base()
    {
    }

    [Answer("2028", Example, Data = "########{nl}#..O.O.#{nl}##@.O..#{nl}#...O..#{nl}#.#.O..#{nl}#...O..#{nl}#......#{nl}########{nl}{nl}<^^>>>vv<v>>v<<")]
    // [Answer("10092", Example, Data = "##########{nl}#..O..O.O#{nl}#......O.#{nl}#.OO..O.O#{nl}#..O@..O.#{nl}#O#..O...#{nl}#O..O..O.#{nl}#.OO.O.OO#{nl}#....O...#{nl}##########{nl}{nl}<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^{nl}vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v{nl}><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<{nl}<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^{nl}^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><{nl}^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^{nl}>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^{nl}<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>{nl}^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>{nl}v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^")]
    [Answer("", Regular)]
    public override string SolveA()
    {
        var (grid, moves) = Parse();
        var start = Start(grid);

        // Debugging
        Console.WriteLine();
        Console.WriteLine($"-- {start} --");
        // grid.Draw();

        Console.WriteLine(".. START ..");

        foreach (var move in moves)
        {
            start = Walk(grid, start, move);

            // Console.WriteLine();
            // Console.WriteLine($"-- {start} --");
            // grid.Draw();
        }

        Console.WriteLine($"-- STOP -- => {start}");
        return Calculate(grid).ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
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
                if (grid[x, y] != 'O')
                    continue;

                var gps = (100L * (y + 1)) + (x + 1);
                total += gps;
                // Console.WriteLine($"(100 * {y}) + {x} = {gps}");
            }
        }

        return total;
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

    private (Grid<char>, Direction[]) Parse()
    {
        var input = GetSplitInput(false).ToList();
        var empty = input.IndexOf(string.Empty);

        var grid = input
            .Skip(1) // Skip header
            .Take(empty - 2) // Skip footer
            .Select(s => s
                .ToCharArray()
                .Skip(1) // Skip starting border
                .SkipLast(1) // Skip ending border
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
}
