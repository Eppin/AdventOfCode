namespace AdventOfCode._2025;

public class Day4 : Day
{
    public Day4() : base()
    {
    }

    [Answer("13", Example, Data = "..@@.@@@@.{nl}@@@.@.@.@@{nl}@@@@@.@.@@{nl}@.@@@@..@.{nl}@@.@@@@.@@{nl}.@@@@@@@.@{nl}.@.@.@.@@@{nl}@.@@@.@@@@{nl}.@@@@@@@@.{nl}@.@.@@@.@.")]
    [Answer("1478", Regular)]
    public override object SolveA()
    {
        var grid = Parse();
        var result = 0;

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                var cell = grid[x, y];
                if (cell == '.')
                    continue;

                var neighbours = grid
                    .Neighbours(new Coordinate(x, y), true)
                    .Select(c => grid[c])
                    .Count(c => c == '@');

                if (neighbours < 4)
                    result++;
            }
        }

        return result;
    }

    [Answer("43", Example, Data = "..@@.@@@@.{nl}@@@.@.@.@@{nl}@@@@@.@.@@{nl}@.@@@@..@.{nl}@@.@@@@.@@{nl}.@@@@@@@.@{nl}.@.@.@.@@@{nl}@.@@@.@@@@{nl}.@@@@@@@@.{nl}@.@.@@@.@.")]
    [Answer("9120", Regular)]
    public override object SolveB()
    {
        var grid = Parse();
        var result = 0;

        for (var i = 0;; i++)
        {
            var changed = false;
            var current = Convert.ToChar(i + 65);

            for (var y = 0; y < grid.MaxY; y++)
            {
                for (var x = 0; x < grid.MaxX; x++)
                {
                    var cell = grid[x, y];
                    if (cell != '@')
                        continue;

                    var neighbours = grid
                        .Neighbours(new Coordinate(x, y), true)
                        .Select(c => grid[c])
                        .Count(c => c == '@' || c == current);

                    if (neighbours < 4)
                    {
                        changed = true;
                        grid[x, y] = current;
                        result++;
                    }
                }
            }

            if (!changed)
                break;
        }

        return result;
    }

    private Grid<char> Parse()
    {
        var grid = SplitInput
            .Select(c => c.ToCharArray())
            .ToArray();

        return new Grid<char>(grid);
    }
}
