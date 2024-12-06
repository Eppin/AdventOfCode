using System.Drawing;

namespace AdventOfCode._2024;

public class Day6 : Day
{
    private long _blocking;

    public Day6() : base()
    {
    }

    [Answer("41", Example, Data = "....#.....{nl}.........#{nl}..........{nl}..#.......{nl}.......#..{nl}..........{nl}.#..^.....{nl}........#.{nl}#.........{nl}......#...")]
    [Answer("4656", Regular)]
    public override string SolveA()
    {
        return Solve()
            .SelectMany(g => g)
            .Count(c => c == 'H')
            .ToString();
    }

    [Answer("6", Example, Data = "....#.....{nl}.........#{nl}..........{nl}..#.......{nl}.......#..{nl}..........{nl}.#..^.....{nl}........#.{nl}#.........{nl}......#...")]
    [Answer("1575", Regular)]
    public override string SolveB()
    {
        var nGrid = Solve();

        for (var y = 0; y < nGrid[0].Length; y++)
        {
            for (var x = 0; x < nGrid.Length; x++)
            {
                if (nGrid[y][x] is 'H')
                {
                    var grid = Parse();
                    grid[y][x] = 'O';

                    Solve(grid);
                }
            }
        }

        return _blocking.ToString();
    }

    private char[][] Solve(char[][]? grid = null)
    {
        grid ??= Parse();

        var maxX = grid.Length;
        var maxY = grid[0].Length;

        var position = new Point();
        var direction = '-';

        for (var y = 0; y < maxY; y++)
        {
            for (var x = 0; x < maxX; x++)
            {
                if (grid[y][x] is '^' or 'v' or '<' or '>')
                {
                    position = new Point(x, y);
                    direction = grid[y][x];
                    break;
                }
            }

            if (direction != '-') break;
        }

        bool exit;
        var steps = 0;

        do
        {
            exit = !Direction(grid, ref direction, ref position, ref steps);
        } while (!exit);

        _blocking += steps == int.MaxValue ? 1 : 0;

        return grid;
    }

    private char[][] Parse()
    {
        return GetSplitInput()
            .Select(l => l.ToCharArray())
            .ToArray();
    }

    private static bool Direction(char[][] grid, ref char direction, ref Point position, ref int steps)
    {
        if (steps > grid.Length * grid[0].Length)
        {
            steps = int.MaxValue;
            return false;
        }

        if (direction is '^')
        {
            for (var i = position.Y; i >= 0; i--)
            {
                steps++;

                if (grid[i][position.X] is '#' or 'O')
                {
                    direction = '>';
                    position = position with { Y = i + 1 };
                    return true;
                }

                if (grid[i][position.X] is not 'O')
                    grid[i][position.X] = 'H';
            }
        }

        if (direction is '>')
        {
            var maxX = grid.Length;

            for (var i = position.X; i < maxX; i++)
            {
                steps++;

                if (grid[position.Y][i] is '#' or 'O')
                {
                    direction = 'v';
                    position = position with { X = i - 1 };
                    return true;
                }

                if (grid[position.Y][i] is not 'O')
                    grid[position.Y][i] = 'H';
            }
        }

        if (direction is 'v')
        {
            var maxY = grid[0].Length;

            for (var i = position.Y; i < maxY; i++)
            {
                steps++;

                if (grid[i][position.X] is '#' or 'O')
                {
                    direction = '<';
                    position = position with { Y = i - 1 };
                    return true;
                }

                if (grid[i][position.X] is not 'O')
                    grid[i][position.X] = 'H';
            }
        }

        if (direction is '<')
        {
            for (var i = position.X; i >= 0; i--)
            {
                steps++;

                if (grid[position.Y][i] is '#' or 'O')
                {
                    direction = '^';
                    position = position with { X = i + 1 };
                    return true;
                }

                if (grid[position.Y][i] is not 'O')
                    grid[position.Y][i] = 'H';
            }
        }

        return false;
    }
}
