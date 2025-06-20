namespace AdventOfCode._2024;

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
                Find(grid, new Coordinate(x, y), isPartB);
            }
        }
    }

    private int _total;
    private readonly List<Coordinate> _reached = [];

    private void Find(Grid<int> grid, Coordinate coordinate, bool isPartB)
    {
        var current = grid[coordinate];

        if (current == 9 && (isPartB || _reached.All(c => c != coordinate)))
        {
            _total++;

            if (!isPartB) _reached.Add(coordinate);

            return;
        }

        foreach (var neighbour in grid.Neighbours(coordinate))
        {
            var next = grid[neighbour];
            if (next == current + 1)
                Find(grid, neighbour, isPartB);
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
