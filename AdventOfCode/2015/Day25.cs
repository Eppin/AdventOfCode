namespace AdventOfCode._2015;

using System.Text.RegularExpressions;

public partial class Day25 : Day
{
    public Day25() : base()
    {
    }

    public override string SolveA()
    {
        long start = 20151125;

        var maxY = 1;
        var maxX = 1;

        var rows = new List<int>();
        var columns = new List<int>();

        // Sort of brute forced while loop..
        while (maxY <= 10_000 && maxX <= 10_000)
        {
            for (var y = maxY - 1; y > 0; y--)
                rows.Add(y);

            for (var x = 1; x < maxX; x++)
                columns.Add(x);

            maxY++;
            maxX++;
        }

        var (searchRow, searchColumn) = Parse();

        var found = false;
        var zip = columns.Zip(rows, (c, r) => (c, r));
        foreach (var (c, r) in zip)
        {
            if (r == searchRow && c == searchColumn)
            {
                found = true;
                break;
            }

            start = (start * 252533) % 33554393;
        }

        return found
            ? start.ToString()
            : int.MinValue.ToString();
    }

    public override string SolveB()
    {
        return "christmas \ud83c\udf84";
    }

    private (int row, int column) Parse()
    {
        var regex = ParseRegex();
        var match = regex.Match(Input);

        if (!match.Success)
            throw new FormatException("Invalid input format");

        var row = int.Parse(match.Groups[1].Value);
        var column = int.Parse(match.Groups[2].Value);

        return (row, column);
    }

    [GeneratedRegex(@"row (\d+), column (\d+)")]
    private static partial Regex ParseRegex();
}
