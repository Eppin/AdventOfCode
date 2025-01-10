namespace AdventOfCode._2024;

using Coordinate = Coordinate<int>;

public class Day15 : Day
{
    public Day15() : base()
    {
    }

    [Answer("10092", Example, Data = "##########{nl}#..O..O.O#{nl}#......O.#{nl}#.OO..O.O#{nl}#..O@..O.#{nl}#O#..O...#{nl}#O..O..O.#{nl}#.OO.O.OO#{nl}#....O...#{nl}##########{nl}{nl}<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^{nl}vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v{nl}><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<{nl}<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^{nl}^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><{nl}^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^{nl}>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^{nl}<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>{nl}^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>{nl}v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^")]
    [Answer("1487337", Regular)]
    public override object SolveA()
    {
        return Solve(false);
    }

    [Answer("9021", Example, Data = "##########{nl}#..O..O.O#{nl}#......O.#{nl}#.OO..O.O#{nl}#..O@..O.#{nl}#O#..O...#{nl}#O..O..O.#{nl}#.OO.O.OO#{nl}#....O...#{nl}##########{nl}{nl}<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^{nl}vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v{nl}><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<{nl}<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^{nl}^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><{nl}^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^{nl}>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^{nl}<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>{nl}^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>{nl}v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^")]
    [Answer("1521952", Regular)]
    public override object SolveB()
    {
        return Solve(true);
    }

    private int Solve(bool isPartB)
    {
        var (grid, moves) = Parse();

        var start = new Coordinate();
        var obstacles = new List<Obstacle>();

        // Get obstacles from grid (and extend)
        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                if (isPartB)
                {
                    if (grid[x, y] is '@') start = new Coordinate(x * 2, y);
                    if (grid[x, y] is 'O') obstacles.Add(new Obstacle(new Coordinate(x * 2, y), new Coordinate(x * 2 + 1, y), Type.Box));
                    if (grid[x, y] is '#') obstacles.Add(new Obstacle(new Coordinate(x * 2, y), new Coordinate(x * 2 + 1, y), Type.Wall));
                }
                else
                {
                    if (grid[x, y] is '@') start = new Coordinate(x, y);
                    if (grid[x, y] is 'O') obstacles.Add(new Obstacle(new Coordinate(x, y), new Coordinate(x, y), Type.Box));
                    if (grid[x, y] is '#') obstacles.Add(new Obstacle(new Coordinate(x, y), new Coordinate(x, y), Type.Wall));
                }
            }
        }

        foreach (var move in moves)
            start = Walk(obstacles, start, move);

        return obstacles
            .Where(o => o.Type == Type.Box)
            .Sum(o => o.First.Y * 100 + o.First.X);
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
                break;

            if (obstaclesFound.Any(o => o.Type == Type.Wall))
            {
                remembers.Clear();
                failed = true;
                break;
            }

            oCoordinates.Clear();

            foreach (var obstacle in obstaclesFound)
            {
                if (obstacle.Type == Type.Wall)
                    break;

                int bestX;
                switch (direction)
                {
                    case Direction.West:
                        bestX = Math.Min(obstacle.First.X, obstacle.Second.X) - 1;
                        oCoordinates.Add(new Coordinate(bestX, coordinate.Y));
                        break;

                    case Direction.East:
                        bestX = Math.Max(obstacle.First.X, obstacle.Second.X) + 1;
                        oCoordinates.Add(new Coordinate(bestX, coordinate.Y));
                        break;
                }

                int bestY;
                switch (direction)
                {
                    case Direction.North:
                        bestY = Math.Min(obstacle.First.Y, obstacle.Second.Y) - 1;
                        oCoordinates.Add(new Coordinate(obstacle.First.X, bestY));
                        oCoordinates.Add(new Coordinate(obstacle.Second.X, bestY));
                        break;

                    case Direction.South:
                        bestY = Math.Max(obstacle.First.Y, obstacle.Second.Y) + 1;
                        oCoordinates.Add(new Coordinate(obstacle.First.X, bestY));
                        oCoordinates.Add(new Coordinate(obstacle.Second.X, bestY));
                        break;
                }

                remembers.Add(obstacle);
            }
        } while (true);

        foreach (var remember in remembers)
        {
            switch (direction)
            {
                case Direction.North:
                    remember.First = remember.First.Up;
                    remember.Second = remember.Second.Up;
                    break;

                case Direction.South:
                    remember.First = remember.First.Down;
                    remember.Second = remember.Second.Down;
                    break;

                case Direction.East:
                    remember.First = remember.First.Right;
                    remember.Second = remember.Second.Right;
                    break;

                case Direction.West:
                    remember.First = remember.First.Left;
                    remember.Second = remember.Second.Left;
                    break;
            }
        }

        return failed ? coordinate : dCoordinate;
    }

    private (Grid<char>, Direction[]) Parse()
    {
        var input = GetSplitInput(false).ToList();
        var empty = input.IndexOf(string.Empty);

        var grid = input
            .Take(empty) // Skip footer
            .Select(s => s.ToCharArray())
            .ToArray();

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
        public Type Type { get; } = type;
    }

    private enum Type
    {
        Wall,
        Box
    }
}
