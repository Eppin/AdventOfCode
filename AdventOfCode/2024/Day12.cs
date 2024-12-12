namespace AdventOfCode._2024;

using System.Drawing;

public class Day12 : Day
{
    public Day12() : base()
    {
    }

    [Answer("1930", Example, Data = "RRRRIICCFF{nl}RRRRIICCCF{nl}VVRRRCCFFF{nl}VVRCCCJFFF{nl}VVVVCJJCFE{nl}VVIVCCJJEE{nl}VVIIICJJEE{nl}MIIIIIJJEE{nl}MIIISIJEEE{nl}MMMISSJEEE")]
    [Answer("1461806", Regular)]
    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    [Answer("1206", Example, Data = "RRRRIICCFF{nl}RRRRIICCCF{nl}VVRRRCCFFF{nl}VVRCCCJFFF{nl}VVVVCJJCFE{nl}VVIVCCJJEE{nl}VVIIICJJEE{nl}MIIIIIJJEE{nl}MIIISIJEEE{nl}MMMISSJEEE")]
    [Answer("887932", Regular)]
    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool isPartB)
    {
        var visited = new List<Point>();
        var grid = Parse();

        var total = 0;

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                var point = new Point(x, y);
                var key = grid[x, y];

                if (visited.Contains(point)) continue;

                var walked = Walk(grid, point, key);
                visited.AddRange(walked);

                Console.WriteLine(key);

                var fences = isPartB
                    ? FencesB(grid, walked)
                    : FencesA(grid, walked);

                total += fences * walked.Count;
            }
        }

        return total;
    }

    private static List<Point> Walk(Grid<string> grid, Point point, string key)
    {
        var points = new List<Point>();
        var queue = new Queue<Point>();

        queue.Enqueue(point);
        points.Add(point);

        while (queue.Count > 0)
        {
            point = queue.Dequeue();

            var neighbours = grid.Neighbours(point).Where(p => grid[p] == key);
            foreach (var neighbour in neighbours)
            {
                if (!points.Contains(neighbour))
                {
                    queue.Enqueue(neighbour);
                    points.Add(neighbour);
                }
            }
        }

        return points;
    }

    private static int FencesA(Grid<string> grid, List<Point> points)
    {
        var total = 0;

        foreach (var point in points)
        {
            var neighbours = grid.Neighbours(point.X, point.Y);
            var valid = neighbours.Where(n => points.Any(r => r == n));
            var edges = 4 - valid.Count();
            total += edges;
        }

        return total;
    }

    private static int FencesB(Grid<string> grid, List<Point> points)
    {
        var total = 0;
        
        // var visited = new HashSet<Point>();
        var fences = points
            .OrderBy(p => p.Y)
            .ThenBy(p => p.X)
            .Select(p => new Fence(p, 0))
            .ToList();

        foreach (var fence in fences)
        {
            var neighbours = grid.Directions(fence.Point, true)
                .Where(n => points.Any(r => r == n.Value))
                .ToDictionary(d => d.Key, d => d.Value);

            if (!neighbours.ContainsKey(Direction.North))
            {
                if (neighbours.TryGetValue(Direction.East, out var east) && !neighbours.ContainsKey(Direction.NorthEast))
                {
                    var eastFence = fences.First(p => p.Point == east);
                    eastFence.Mark +=1;
                    Console.WriteLine($"Mark east(ne) {eastFence.Mark}");
                }

                if (neighbours.TryGetValue(Direction.West, out var west) && !neighbours.ContainsKey(Direction.NorthWest))
                {
                    var westFence = fences.First(p => p.Point == west);
                    westFence.Mark +=1;
                    Console.WriteLine($"Mark west(nw) {westFence.Mark}");
                }
            }

            if (!neighbours.ContainsKey(Direction.East))
            {
                if (neighbours.TryGetValue(Direction.North, out var north) && !neighbours.ContainsKey(Direction.NorthEast))
                {
                    var northFence = fences.First(p => p.Point == north);
                    northFence.Mark +=1;
                    Console.WriteLine($"Mark north(ne) {northFence.Mark}");
                }
                
                if (neighbours.TryGetValue(Direction.South, out var south) && !neighbours.ContainsKey(Direction.SouthEast))
                {
                    var southFence = fences.First(p => p.Point == south);
                    southFence.Mark +=1;
                    Console.WriteLine($"Mark south(se) {southFence.Mark}");
                }
            }

            //
            if (!neighbours.ContainsKey(Direction.South))
            {
                if (neighbours.TryGetValue(Direction.East, out var south) && !neighbours.ContainsKey(Direction.SouthEast))
                {
                    var southFence = fences.First(p => p.Point == south);
                    southFence.Mark += 1;
                    Console.WriteLine($"Mark south(se) {southFence.Mark}");
                }

                if (neighbours.TryGetValue(Direction.West, out var west) && !neighbours.ContainsKey(Direction.SouthWest))
                {
                    var westFence = fences.First(p => p.Point == west);
                    westFence.Mark +=1;
                    Console.WriteLine($"Mark west(sw) {westFence.Mark}");
                }
            }
            
            if (!neighbours.ContainsKey(Direction.West))
            {
                if (neighbours.TryGetValue(Direction.North, out var north) && !neighbours.ContainsKey(Direction.NorthWest))
                {
                    var northFence = fences.First(p => p.Point == north);
                    northFence.Mark +=1;
                    Console.WriteLine($"Mark north(nw) {northFence.Mark}");
                }
                
                if (neighbours.TryGetValue(Direction.South, out var south) && !neighbours.ContainsKey(Direction.SouthWest))
                {
                    var southFence = fences.First(p => p.Point == south);
                    southFence.Mark +=1;
                    Console.WriteLine($"Mark south(sw) {southFence.Mark}");
                }
            }

            var count = neighbours.Count(n => n.Key is Direction.North or Direction.East or Direction.South or Direction.West);
            var sum = 4 - fence.Mark - count;
            Console.WriteLine($"C: {fence.Point} -> 4-{fence.Mark}-{count}={sum}");
            
            total += sum;

            // var isNorth = neighbours.Any(n => n == grid.North(point.X, point.Y));
            // var isSouth = neighbours.Any(n => n == grid.South(point.X, point.Y));
            // var isEast = neighbours.Any(n => n == grid.East(point.X, point.Y));
            // var isWest = neighbours.Any(n => n == grid.West(point.X, point.Y));

            // var isNorthEast = neighbours.Any(n => n == grid.NorthEast(point.X, point.Y));
            // var isNorthWest = neighbours.Any(n => n == grid.NorthWest(point.X, point.Y));
            // var isSouthEast = neighbours.Any(n => n == grid.SouthEast(point.X, point.Y));
            // var isSouthWest = neighbours.Any(n => n == grid.SouthWest(point.X, point.Y));


            // if (!isNorth)
            // {
            //     total += 1;
            // }
            //
            // if (!isEast)
            // {
            //     total += 1;
            // }
            //
            // if (!isSouth)
            // {
            //     total += 1;
            //     
            // }
            //
            // if (!isWest)
            // {
            //     total += 1;
            // }

            // Console.WriteLine($"C:{point}, V:{string.Join(",", neighbours)}, N:{isNorth}, NE:{isNorthEast}, E:{isEast}, SE:{isSouthEast}, S:{isSouth}, SW:{isSouthWest} W:{isWest}, NW:{isNorthWest}, {total}");

            // visited.Add(point);
        }

        Console.WriteLine();

        return total;

        // var point = points.First();
        // var queue = new Queue<Point>();
        // var visited = new HashSet<Point>();
        //
        // queue.Enqueue(point);
        // visited.Add(point);
        //
        // Direction? direction = null;
        // var total = 0;
        //
        // while (queue.Count > 0)
        // {
        //     point = queue.Dequeue();
        //
        //     var neighbours = grid.Neighbours(point).Where(p => points.Contains(p) && !visited.Contains(p)).Take(1);
        //     foreach (var neighbour in neighbours)
        //     {
        //         if (point != neighbour && !visited.Contains(neighbour))
        //         {
        //             var isNorth = grid.North(point.X, point.Y) == neighbour;
        //             var isSouth = grid.South(point.X, point.Y) == neighbour;
        //             var isEast = grid.East(point.X, point.Y) == neighbour;
        //             var isWest = grid.West(point.X, point.Y) == neighbour;
        //
        //             var newDirection = isNorth ? Direction.North : isSouth ? Direction.South : isEast ? Direction.East : Direction.West;
        //
        //             if (direction != null && newDirection != direction)
        //                 total += 1;
        //
        //             Console.WriteLine($"P:{point} -> N:{neighbour} ({direction} vs {newDirection}, {total})"); // (N:{grid.North(point.X, point.Y) == neighbour}, S:{grid.South(point.X, point.Y) == neighbour}, E:{grid.East(point.X, point.Y) == neighbour}, W:{grid.West(point.X, point.Y) == neighbour})");
        //             queue.Enqueue(neighbour);
        //             visited.Add(neighbour);
        //
        //             direction = newDirection;
        //         }
        //
        //         // if (!points.Contains(neighbour))
        //         // {
        //         //     //Console.WriteLine($"Continue, s:{point} -> n:{neighbour} = {grid[point]}");
        //         //     queue.Enqueue(neighbour);
        //         //     points.Add(neighbour);
        //         // }
        //     }
        // }
        //
        // Console.WriteLine();
        // return 0; // TODO
    }


    private Grid<string> Parse()
    {
        var split = GetSplitInput()
            .Select(s => s
                .ToCharArray()
                .Select(c => c.ToString())
                .ToArray())
            .ToArray();

        return new Grid<string>(split);
    }

    private class Fence(Point point, int mark)
    {
        public Point Point { get; set; } = point;
        public int Mark { get; set; } = mark;
    }
}
