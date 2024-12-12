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
        return Solve().ToString();
    }

    [Answer("80", Example, Data = "AAAA{nl}BBCD{nl}BBCC{nl}EEEC")]
    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private int Solve()
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

                total += Fences(grid, walked) * walked.Count;
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
                    //Console.WriteLine($"Continue, s:{point} -> n:{neighbour} = {grid[point]}");
                    queue.Enqueue(neighbour);
                    points.Add(neighbour);
                }
            }
        }

        //Console.WriteLine($"K:{key}, P:{points.Count}");
        //Console.WriteLine();

        return points;
    }

    private static int Fences(Grid<string> grid, List<Point> points)
    {
        var total = 0;

        foreach (var point in points)//.OrderBy(r => r.Key))
        {
            var neighbours = grid.Neighbours(point.X, point.Y).ToList();
            var valid = neighbours.Where(n => points.Any(r => r == n)).ToList();
            var edges = 4 - valid.Count;
            total += edges;

            //Console.WriteLine($"\t{point} -> A:{string.Join(',', neighbours)}, V:{string.Join(',', valid)} = {4 - valid.Count}");
            //var add = edges * points.Count;
            //total += add;
            //Console.WriteLine($"\t{point} -> {edges}*{points.Count}");//={add}");


        }

        //Console.WriteLine($"Total for {total}");
        //Console.WriteLine();

        return total;
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
}
