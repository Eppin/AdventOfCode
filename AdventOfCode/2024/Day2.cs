namespace AdventOfCode._2024;

public class Day2 : Day
{
    public Day2() : base()
    {
    }

    [Answer("2", Example, Data = "7 6 4 2 1{nl}1 2 7 8 9{nl}9 7 6 2 1{nl}1 3 2 4 5{nl}8 6 4 4 1{nl}1 3 6 7 9{nl}")]
    [Answer("680", Regular)]
    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    [Answer("4", Example, Data = "7 6 4 2 1{nl}1 2 7 8 9{nl}9 7 6 2 1{nl}1 3 2 4 5{nl}8 6 4 4 1{nl}1 3 6 7 9{nl}")]
    [Answer("710", Regular)]
    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool isPartB)
    {
        var result = 0;

        foreach (var line1 in Parse())
        {
            var lines = new List<List<long>> { line1 };

            if (isPartB)
            {
                // Create combinations of items without a single digit
                for (var i = 0; i < line1.Count; i++)
                    lines.Add([.. line1[..i], .. line1[(i + 1)..]]);
            }

            var safe = false;
            foreach (var line2 in lines)
            {
                safe = CalculateSafe(line2);

                if (safe)
                    break;
            }

            result += safe ? 1 : 0;
        }

        return result;
    }

    private static bool CalculateSafe(List<long> line)
    {
        var safe = false;
        bool? trend = null;

        for (var i = 0; i < line.Count; i++)
        {
            if (i + 1 >= line.Count) continue;

            var current = line[i];
            var next = line[i + 1];
            var diff = current - next;

            if (Math.Abs(diff) is < 1 or > 3)
            {
                safe = false;
                break;
            }

            trend ??= trend switch
            {
                null when diff >= 0 => true,
                null when true => false,
                _ => trend
            };

            if (trend == true && diff < 0) // up
            {
                safe = false;
                break;
            }

            if (trend == false && diff > 0) // down
            {
                safe = false;
                break;
            }

            safe = true;
        }

        return safe;
    }

    private List<List<long>> Parse()
    {
        return SplitInput
            .Select(s => s
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList())
            .ToList();
    }
}
