using System.Drawing;

namespace AdventOfCode._2024;

public class Day6 : Day
{
    public Day6() : base()
    {
    }

    [Answer("41", Example, Data = "....#.....{nl}.........#{nl}..........{nl}..#.......{nl}.......#..{nl}..........{nl}.#..^.....{nl}........#.{nl}#.........{nl}......#...")]
    [Answer("", Regular)]
    public override string SolveA()
    {
        return Solve().ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private int Solve()
    {
        var grid = GetSplitInput()
            .Select(l => l.ToCharArray())
            .ToArray();

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

        return grid
            .SelectMany(g => g)
            .Count(c => c == 'H');
    }

    private static bool Direction(char[][] grid, ref char direction, ref Point position)
    {
        if (direction is '^')
        {
            for (var i = position.Y; i >= 0; i--)
            {
                if (grid[i][position.X] is '#')
                {
                    direction = '>';
                    position = position with { Y = i + 1 };
                    return true;
                }

                grid[i][position.X] = 'H';
            }
        }

        if (direction is '>')
        {
            var maxX = grid.Length;

            for (var i = position.X; i < maxX; i++)
            {
                if (grid[position.Y][i] is '#')
                {
                    direction = 'v';
                    position = position with { X = i - 1 };
                    return true;
                }

                grid[position.Y][i] = 'H';
            }
        }

        if (direction is 'v')
        {
            var maxY = grid[0].Length;

            for (var i = position.Y; i < maxY; i++)
            {
                if (grid[i][position.X] is '#')
                {
                    direction = '<';
                    position = position with { Y = i - 1 };
                    return true;
                }

                grid[i][position.X] = 'H';
            }
        }

        if (direction is '<')
        {
            for (var i = position.X; i >= 0; i--)
            {
                if (grid[position.Y][i] is '#')
                {
                    direction = '^';
                    position = position with { X = i + 1 };
                    return true;
                }

                grid[position.Y][i] = 'H';
            }
        }

        return false;
    }
}
