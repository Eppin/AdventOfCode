namespace AdventOfCode._2024;

public class Day4 : Day
{
    public Day4() : base()
    {
    }

    [Answer("18", Example, Data = "MMMSXXMASM{nl}MSAMXMSMSA{nl}AMXSXMAAMM{nl}MSAMASMSMX{nl}XMASAMXAMM{nl}XXAMMXXAMA{nl}SMSMSASXSS{nl}SAXAMASAAA{nl}MAMMMXMMMM{nl}MXMXAXMASX{nl}")]
    [Answer("2493", Regular)]
    public override object SolveA()
    {
        var grid = GetSplitInput()
            .Select(l => l.ToCharArray())
            .ToArray();

        var yAxis = grid.Length;
        var xAxis = grid[0].Length;

        var total = 0;

        for (var y = 0; y < yAxis; y++)
        {
            for (var x = 0; x < xAxis; x++)
            {
                var c = grid[y][x];

                if (c is not ('X' or 'S')) continue;

                total += Find(grid, x, y, c is 'S', false).Count;
            }
        }

        return total;
    }

    [Answer("9", Example, Data = "MMMSXXMASM{nl}MSAMXMSMSA{nl}AMXSXMAAMM{nl}MSAMASMSMX{nl}XMASAMXAMM{nl}XXAMMXXAMA{nl}SMSMSASXSS{nl}SAXAMASAAA{nl}MAMMMXMMMM{nl}MXMXAXMASX{nl}")]
    [Answer("1890", Regular)]
    public override object SolveB()
    {
        var grid = GetSplitInput()
            .Select(l => l.ToCharArray())
            .ToArray();

        var yAxis = grid.Length;
        var xAxis = grid[0].Length;

        var coords = new List<List<(char Char, Coordinate Coordinate)>>();

        for (var y = 0; y < yAxis; y++)
        {
            for (var x = 0; x < xAxis; x++)
            {
                var c = grid[y][x];

                if (c is not ('M' or 'S')) continue;

                coords.AddRange(Find(grid, x, y, c is 'S', true));
            }
        }

        return coords
                    .SelectMany(x => x)
                    .Where(x => x.Char == 'A')
                    .GroupBy(x => x.Coordinate)
                    .Count(c => c.Count() == 2)
                    .ToString();
    }

    private static List<List<(char Char, Coordinate Coordinate)>> Find(char[][] grid, int x, int y, bool isBackwards, bool isPartB)
    {
        var xmas = "XMAS";
        var samx = "SAMX";
        var length = 4;

        if (isPartB)
        {
            xmas = "MAS";
            samx = "SAM";
            length = 3;
        }

        var maxX = grid.Length;
        var maxY = grid[0].Length;

        var found = false;
        var results = new List<List<(char Char, Coordinate Coordinate)>>();

        if (!isPartB)
        {
            // Horizontal
            var horizontal = new List<(char Char, Coordinate Coordinate)>();
            for (var i = 0; i < length; i++)
            {
                var nextChar = isBackwards ? samx[i] : xmas[i];

                if (y + i >= maxY || grid[y + i][x] != nextChar)
                {
                    found = false;
                    break;
                }

                horizontal.Add((grid[y + i][x], new Coordinate(y + i, x)));
                found = true;
            }

            if (found) results.Add(horizontal);

            // Vertical
            var vertical = new List<(char Char, Coordinate Coordinate)>();
            for (var i = 0; i < length; i++)
            {
                var nextChar = isBackwards ? samx[i] : xmas[i];

                if (x + i >= maxX || grid[y][x + i] != nextChar)
                {
                    found = false;
                    break;
                }

                vertical.Add((grid[y][x + i], new Coordinate(y, x + 1)));
                found = true;
            }

            if (found) results.Add(vertical);
        }

        // Diagonal (down-right)
        var diagonal1 = new List<(char Char, Coordinate Coordinate)>();
        for (var i = 0; i < length; i++)
        {
            var nextChar = isBackwards ? samx[i] : xmas[i];

            if (x + i >= maxX || y + i >= maxY || grid[y + i][x + i] != nextChar)
            {
                found = false;
                break;
            }

            diagonal1.Add((grid[y + i][x + i], new Coordinate(y + i, x + 1)));
            found = true;
        }

        if (found) results.Add(diagonal1);

        // Diagonal (down-left)
        var diagonal2 = new List<(char Char, Coordinate Coordinate)>();
        for (var i = 0; i < length; i++)
        {
            var nextChar = isBackwards ? samx[i] : xmas[i];

            if (x - i < 0 || y + i >= maxY || grid[y + i][x - i] != nextChar)
            {
                found = false;
                break;
            }

            diagonal2.Add((grid[y + i][x - i], new Coordinate(y + i, x - 1)));
            found = true;
        }

        if (found) results.Add(diagonal2);

        return results;
    }
}
