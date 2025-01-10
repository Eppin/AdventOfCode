namespace AdventOfCode._2024;

using System.Drawing;

public class Day10 : Day
{
    public Day10() : base()
    {
    }

    [Answer("36", Example, Data = "89010123{nl}78121874{nl}87430965{nl}96549874{nl}45678903{nl}32019012{nl}01329801{nl}10456732")]
    [Answer("644", Regular)]
    public override object SolveA()
    {
        Solve(false);
        return _total;
    }

    [Answer("81", Example, Data = "89010123{nl}78121874{nl}87430965{nl}96549874{nl}45678903{nl}32019012{nl}01329801{nl}10456732")]
    [Answer("1366", Regular)]
    public override object SolveB()
    {
        Solve(true);
        return _total;
    }

    private void Solve(bool isPartB)
    {
        var grid = Parse();

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                var c = grid[x, y];

                if (c is not 0) continue;

                _reached.Clear();
                Find(grid, x, y, isPartB);
            }
        }
    }

    private int _total;
    private readonly List<Point> _reached = [];

    private void Find(Grid<int> grid, int x, int y, bool isPartB)
    {
        var current = grid[x, y];

        if (current == 9 && (isPartB || !_reached.Any(p => p.X == x && p.Y == y)))
        {
            _total++;

            if (!isPartB) _reached.Add(new Point(x, y));

            return;
        }

        foreach (var neighbour in grid.Neighbours(x, y))
        {
            var next = grid[neighbour];
            if (next == current + 1)
                Find(grid, neighbour.X, neighbour.Y, isPartB);
        }
    }

    private Grid<int> Parse()
    {
        var grid = GetSplitInput()
            .Select(l => l
                .ToCharArray()
                .Select(c => int.Parse($"{c}"))
                .ToArray()
            ).ToArray();

        return new Grid<int>(grid);
    }
}
