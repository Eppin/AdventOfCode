namespace AdventOfCode._2015;

public class Day18 : Day
{
    private int _gridSize;

    public Day18() : base()
    {
    }

    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool isPartB)
    {
        var grid = SplitInput
            .Select(split => split.ToCharArray())
            .ToArray();

        _gridSize = grid.Length;

        for (var i = 0; i < 100; i++)
            grid = Next(grid, isPartB);

        return grid.SelectMany(g => g).Count(g => g == '#');
    }

    private char[][] Next(IReadOnlyList<char[]> current, bool isPartB)
    {
        var next = new char[_gridSize][];
        for (var i = 0; i < _gridSize; i++)
            next[i] = new char[_gridSize];

        if (isPartB)
            ForceCornersOn(current);

        for (var y = 0; y < _gridSize; y++)
        {
            for (var x = 0; x < _gridSize; x++)
            {
                var center = current[y][x] == '#';

                var grid = GetNeighbours(current, x, y);

                if (center)
                    next[y][x] = grid is 2 or 3 ? '#' : '.';
                else
                    next[y][x] = grid is 3 ? '#' : '.';
            }
        }

        if (isPartB)
            ForceCornersOn(next);

        return next;
    }

    private void ForceCornersOn(IReadOnlyList<char[]> grid)
    {
        grid[0][0] = '#'; // Left top
        grid[0][_gridSize - 1] = '#'; // Right top
        grid[_gridSize - 1][_gridSize - 1] = '#'; // Right bottom
        grid[_gridSize - 1][0] = '#'; // Left bottom
    }

    private int GetNeighbours(IReadOnlyList<char[]> current, int x, int y)
    {
        var lt = GetGridValue(current, y - 1, x - 1);
        var l = GetGridValue(current, y, x - 1);
        var lb = GetGridValue(current, y + 1, x - 1);

        var rt = GetGridValue(current, y - 1, x + 1);
        var r = GetGridValue(current, y, x + 1);
        var rb = GetGridValue(current, y + 1, x + 1);

        var t = GetGridValue(current, y - 1, x);
        var b = GetGridValue(current, y + 1, x);

        return lt + l + lb + rt + r + rb + t + b;
    }

    private int GetGridValue(IReadOnlyList<char[]> current, int y, int x)
    {
        if (x >= 0 && x < _gridSize && y >= 0 && y < _gridSize)
            return current[y][x] == '#' ? 1 : 0;

        return 0;
    }
}
