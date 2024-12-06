using System.Drawing;

namespace AdventOfCode._2024;

public class Day6 : Day
{
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
        //var oGrid = Parse();
        var nGrid = Solve();

        for (var y = 0; y < nGrid[0].Length; y++)
        {
            for (var x = 0; x < nGrid.Length; x++)
            {
                if (nGrid[y][x] is 'H')
                {
                    var grid = Parse();
                    grid[y][x] = 'O';

                    _steps = 0;
                    Solve(grid);

                    //break;
                }
            }
        }



        return string.Empty;
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

        do
        {
            exit = !Direction(grid, ref direction, ref position);
        } while (!exit);

        //foreach (var chars in grid)
        //{
        //    Console.WriteLine(string.Join("", chars));
        //}

        _steps2 += (_steps >= maxY * maxX) ? 1 : 0;
        Console.WriteLine($"Steps:{_steps} => {_steps2}");

        return grid;
    }

    private char[][] Parse()
    {
        return GetSplitInput()
            .Select(l => l.ToCharArray())
            .ToArray();
    }

    private long _steps = 0;
    private long _steps2 = 0;

    private bool Direction(char[][] grid, ref char direction, ref Point position)
    {
        if (_steps > grid.Length * grid[0].Length)
            return false;

        if (direction is '^')
        {
            for (var i = position.Y; i >= 0; i--)
            {
                _steps++;

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
                _steps++;

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
                _steps++;

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
                _steps++;

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
