namespace AdventOfCode._2024;

using System.Drawing;

public class Day10 : Day
{
    public Day10() : base()
    {
    }

    [Answer("36", Example, Data = "89010123{nl}78121874{nl}87430965{nl}96549874{nl}45678903{nl}32019012{nl}01329801{nl}10456732")]
    [Answer("644", Regular)]
    public override string SolveA()
    {
        Solve(false);
        return _total.ToString();
    }

    [Answer("81", Example, Data = "89010123{nl}78121874{nl}87430965{nl}96549874{nl}45678903{nl}32019012{nl}01329801{nl}10456732")]
    [Answer("1366", Regular)]
    public override string SolveB()
    {
        Solve(true);
        return _total.ToString();
    }

    private void Solve(bool isPartB)
    {
        var grid = Parse();

        var yAxis = grid.Length;
        var xAxis = grid[0].Length;

        for (var y = 0; y < yAxis; y++)
        {
            for (var x = 0; x < xAxis; x++)
            {
                var c = grid[y][x];

                if (c is not 0) continue;

                _reached.Clear();
                Find(grid, x, y, isPartB);
            }
        }
    }

    private int _total;
    private readonly List<Point> _reached = [];

    private void Find(int[][] grid, int x, int y, bool isPartB)
    {
        var current = grid[y][x];

        if (current == 9 && (isPartB || !_reached.Any(p => p.X == x && p.Y == y)))
        {
            _total++;

            if (!isPartB) _reached.Add(new Point(x, y));

            return;
        }

        var nexts = Next(grid, x, y);

        foreach (var point in nexts)
        {
            var next = grid[point.Y][point.X];
            if (next == current + 1)
                Find(grid, point.X, point.Y, isPartB);
        }
    }

    private static IEnumerable<Point> Next(int[][] grid, int x, int y)
    {
        var yAxis = grid.Length;
        var xAxis = grid[0].Length;

        var left = new Point(x - 1, y);
        if (left.X >= 0) yield return left;

        var right = new Point(x + 1, y);
        if (right.X < xAxis) yield return right;

        var up = new Point(x, y - 1);
        if (up.Y >= 0) yield return up;

        var down = new Point(x, y + 1);
        if (down.Y < yAxis) yield return down;
    }

    private int[][] Parse()
    {
        return GetSplitInput()
            .Select(l => l
                .ToCharArray()
                .Select(c => int.Parse($"{c}"))
                .ToArray()
            ).ToArray();
    }
}
