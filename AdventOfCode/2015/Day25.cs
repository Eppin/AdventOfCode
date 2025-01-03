namespace AdventOfCode._2015;

public partial class Day25 : Day
{
    public Day25() : base()
    {
    }

    [Answer("2650453", Regular)]
    public override string SolveA()
    {
        long start = 20151125;

        var maxY = 1;
        var maxX = 1;

        var rows = new List<int>();
        var columns = new List<int>();

        var (searchRow, searchColumn) = Parse();
        var maxValue = searchRow + searchColumn;

        while (maxY <= maxValue && maxX <= maxValue)
        {
            for (var y = maxY - 1; y > 0; y--)
                rows.Add(y);

            for (var x = 1; x < maxX; x++)
                columns.Add(x);

            maxY++;
            maxX++;
        }
        
        var found = false;
        var zip = columns.Zip(rows);
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

    [Answer("christmas \ud83c\udf84", Regular)]
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
