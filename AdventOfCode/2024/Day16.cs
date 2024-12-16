namespace AdventOfCode._2024;

using System.Linq;
using Coordinate = Coordinate<int>;

public class Day16 : Day
{
    public Day16() : base()
    {
    }

    //[Answer("7036", Example, Data = "###############{nl}#.......#....E#{nl}#.#.###.#.###.#{nl}#.....#.#...#.#{nl}#.###.#####.#.#{nl}#.#.#.......#.#{nl}#.#.#####.###.#{nl}#...........#.#{nl}###.#.#####.#.#{nl}#...#.....#.#.#{nl}#.#.#.###.#.#.#{nl}#.....#...#.#.#{nl}#.###.#.#.#.#.#{nl}#S..#.....#...#{nl}###############")]
    [Answer("11048", Example, Data = "#################{nl}#...#...#...#..E#{nl}#.#.#.#.#.#.#.#.#{nl}#.#.#.#...#...#.#{nl}#.#.#.#.###.#.#.#{nl}#...#.#.#.....#.#{nl}#.#.#.#.#.#####.#{nl}#.#...#.#.#.....#{nl}#.#.#####.#.###.#{nl}#.#.#.......#...#{nl}#.#.###.#####.###{nl}#.#.#...#.....#.#{nl}#.#.#.#####.###.#{nl}#.#.#.........#.#{nl}#.#.#.#########.#{nl}#S#.............#{nl}#################")]
    [Answer("", Regular)]
    public override string SolveA()
    {
        var parsed = Parse();

        var grid = new Grid<char>(parsed);

        var start = new Coordinate();
        var end = new Coordinate();
        var walls = new List<Coordinate>();

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                if (grid[x, y] is 'S')
                {
                    start = new Coordinate(x, y);
                    grid[x, y] = '.';
                }

                if (grid[x, y] is 'E')
                {
                    end = new Coordinate(x, y);
                    grid[x, y] = '.';
                }

                if (grid[x, y] is '#') walls.Add(new Coordinate(x, y));
            }
        }

        return Walk5(grid, start, end, Direction.East).ToString();
        //return Walk4(grid, start, end, Direction.East, walls).ToString();
        //return Walk3(grid, new Reindeer2(start, Direction.East, 0, []), end).ToString();
        //return Walk2(grid, start, end, Direction.East).ToString();
        //return Walk(start, end, Direction.East, walls).ToString();
    }

    private static int Walk3(Grid<char> grid, Reindeer2 reindeer, Coordinate end)
    {
        if (reindeer.Position == end)
        {
            grid[reindeer.Position] = '.';

            //Console.WriteLine($"DONE!! {reindeer.Position}, {reindeer.Direction}, {reindeer.Points}");
            return reindeer.Points;
        }

        var neighbours = grid.Directions(reindeer.Position)
            .Where(d => grid[d.Value] is '.' && Rotate(reindeer.Direction).Contains(d.Key) && !reindeer.Visited.Contains(d.Value));

        var t = 0;
        var a = new List<int>();

        foreach (var (dir, neighbour) in neighbours)
        {
            var n = dir == reindeer.Direction
                ? new Reindeer2(neighbour, dir, reindeer.Points + 1, [.. reindeer.Visited, reindeer.Position])
                : new Reindeer2(neighbour, dir, reindeer.Points + 1000 + 1, [.. reindeer.Visited, reindeer.Position]);

            var w3 = Walk3(grid, n, end);
            //if (w3 > 0)
            //    a.Add(w3);
        }

        return 0;
    }

    private int Walk4(Grid<char> grid, Coordinate start, Coordinate end, Direction direction, List<Coordinate> walls)
    {
        var queue = new Queue<Reindeer2>();
        queue.Enqueue(new Reindeer2(start, direction, 0, []));

        var t = new List<int>();

        while (queue.Count > 0)
        {
            var reindeer = queue.Dequeue();

            //Console.SetCursorPosition(0, 0);
            //grid.Draw();

            //Thread.Sleep(0_500);

            if (reindeer.Position == end)
            {
                //grid[reindeer.Position] = '.';

                Console.WriteLine($"DONE!! {reindeer.Position}, {reindeer.Direction}, {reindeer.Points}");
                //if (bestScore > reindeer.Points)
                //    bestScore = reindeer.Points;
                t.Add(reindeer.Points);

            }
            else
            {
                grid[reindeer.Position] = '-';

                var neighbours = grid.Directions(reindeer.Position)
                    .Where(d => /*grid[d.Value] is '.' &&*/ !walls.Contains(d.Value) && Rotate(reindeer.Direction).Contains(d.Key) && !reindeer.Visited.Contains(d.Value)); // ADD WALLS?! 

                foreach (var (dir, neighbour) in neighbours)
                {
                    queue.Enqueue(dir == reindeer.Direction
                        ? new Reindeer2(neighbour, dir, reindeer.Points + 1, [.. reindeer.Visited, reindeer.Position])
                        : new Reindeer2(neighbour, dir, reindeer.Points + 1000 + 1, [.. reindeer.Visited, reindeer.Position]));
                }
            }
        }

        return 0;
    }

    private int Walk5(Grid<char> grid, Coordinate start, Coordinate end, Direction direction)
    {
        var queue = new Queue<Reindeer3>();
        queue.Enqueue(new Reindeer3(start, direction, 0, grid));

        var t = new List<int>();

        while (queue.Count > 0)
        {
            var reindeer = queue.Dequeue();

            Console.SetCursorPosition(0, 0);
            var reindeerGrid = reindeer.Grid;
            reindeerGrid.Draw();

            //Thread.Sleep(0_500);

            if (reindeer.Position == end)
            {
                //grid[reindeer.Position] = '.';

                Console.WriteLine($"DONE!! {reindeer.Position}, {reindeer.Direction}, {reindeer.Points}");
                //if (bestScore > reindeer.Points)
                //    bestScore = reindeer.Points;
                t.Add(reindeer.Points);

            }
            else
            {
                reindeerGrid[reindeer.Position] = '-'; // TODO COPY OF?!

                var neighbours = reindeerGrid.Directions(reindeer.Position)
                    .Where(d => reindeerGrid[d.Value] is '.' && Rotate(reindeer.Direction).Contains(d.Key));

                foreach (var (dir, neighbour) in neighbours)
                {
                    queue.Enqueue(dir == reindeer.Direction
                        ? new Reindeer3(neighbour, dir, reindeer.Points + 1, reindeerGrid)
                        : new Reindeer3(neighbour, dir, reindeer.Points + 1000 + 1, reindeerGrid));
                }
            }
        }

        return 0;
    }

    private int Walk2(Grid<char> grid, Coordinate start, Coordinate end, Direction direction)
    {
        var queue = new Queue<Reindeer>();
        queue.Enqueue(new Reindeer(start, direction, 0));

        var bestScore = int.MaxValue;

        while (queue.Count > 0)
        {
            var reindeer = queue.Dequeue();


            //Console.SetCursorPosition(0, 0);
            //grid.Draw();

            //Thread.Sleep(0_500);

            if (reindeer.Position == end)
            {
                grid[reindeer.Position] = '.';

                Console.WriteLine($"DONE!! {reindeer.Position}, {reindeer.Direction}, {reindeer.Points}");
                if (bestScore > reindeer.Points)
                    bestScore = reindeer.Points;

                continue;
            }

            grid[reindeer.Position] = '-';

            var neighbours = grid.Directions(reindeer.Position)
                .Where(d => Rotate(reindeer.Direction).Contains(d.Key) && grid[d.Value] is '.');

            foreach (var (dir, neighbour) in neighbours)
            {
                queue.Enqueue(dir == reindeer.Direction
                    ? new Reindeer(neighbour, dir, reindeer.Points + 1) // [.. reindeer.Visited, reindeer.Position]);
                    : new Reindeer(neighbour, dir, reindeer.Points + 1000 + 1));// [.. reindeer.Visited, reindeer.Position]));
            }
        }

        return bestScore;
    }

    private int Walk(Coordinate start, Coordinate end, Direction direction, List<Coordinate> walls)
    {
        var queue = new Queue<Reindeer>();
        queue.Enqueue(new Reindeer(start, direction, 0));

        var bestScore = int.MaxValue;

        var allVisited = new List<Coordinate>();
        var i = 0;

        //Console.Clear();

        while (queue.Count > 0)
        {
            var reindeer = queue.Dequeue();

            i++;

            if (i % 10_000 == 0)
                Console.WriteLine(i);

            //var parsed = Parse();
            //var grid = new Grid<char>(parsed);

            //foreach (var visited in allVisited)
            //{
            //    grid[visited] = '*';
            //}

            //Console.SetCursorPosition(0,0);
            //Console.WriteLine($"I:{i}");
            //grid.Draw();

            //Thread.Sleep(0_050);



            var neighbours = reindeer.Position.Directions
                .Where(n => !walls.Contains(n.Value)
                            //&& !reindeer.Visited.Contains(n.Value)
                            && !allVisited.Contains(n.Value)
                            && Rotate(reindeer.Direction).Contains(n.Key));

            if (reindeer.Points >= bestScore)
                continue;

            if (reindeer.Position == end)
            {
                //Console.WriteLine($"DONE!! {reindeer.Position}, {reindeer.Direction}, {reindeer.Points}");
                if (bestScore > reindeer.Points)
                    bestScore = reindeer.Points;

                continue;
            }

            //if (!neighbours.Any())
            //{
            //    Console.WriteLine($"Geen buren?! {reindeer.Position}, {reindeer.Direction}, {reindeer.Points}");
            //}

            //var visited1 = new List<Coordinate>(reindeer.Visited) { reindeer.Position };

            allVisited.Add(reindeer.Position);

            foreach (var (dir, neighbour) in neighbours)
            {
                queue.Enqueue(dir == reindeer.Direction
                    ? new Reindeer(neighbour, dir, reindeer.Points + 1) // [.. reindeer.Visited, reindeer.Position]);
                    : new Reindeer(neighbour, dir, reindeer.Points + 1000 + 1));// [.. reindeer.Visited, reindeer.Position]));
            }
        }

        return bestScore;
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private static Direction[] Rotate(Direction direction)
    {
        // Only allow current direction and (counter)clockwise
        return direction switch
        {
            Direction.North => [Direction.North, Direction.East, Direction.West],
            Direction.East => [Direction.East, Direction.North, Direction.South],
            Direction.South => [Direction.South, Direction.West, Direction.East],
            Direction.West => [Direction.West, Direction.North, Direction.South],
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private char[][] Parse()
    {
        return GetSplitInput()
            .Select(s => s.ToCharArray())
            .ToArray();
    }

    private record struct Reindeer3(Coordinate Position, Direction Direction, int Points, Grid<char> Grid);
    private record struct Reindeer2(Coordinate Position, Direction Direction, int Points, List<Coordinate> Visited);
    private record struct Reindeer(Coordinate Position, Direction Direction, int Points);//, List<Coordinate> Visited);

    //private class Reindeer(Coordinate position, Direction direction, int points, List<Coordinate> visited)
    //{
    //    public Coordinate Position { get; set; } = position;
    //    public Direction Direction { get; set; } = direction;
    //    public int Points { get; set; } = points;

    //    public List<Coordinate> Visited { get; set; } = visited;
    //}
}
