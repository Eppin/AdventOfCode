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
        var result = 0;

        foreach (var line in Parse())
        {
            var safe = false;
            bool? trend = null;

            for (var i = 0; i < line.Length; i++)
            {
                if (i + 1 >= line.Length) continue;

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

            result += safe ? 1 : 0;
        }

        return result.ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private IEnumerable<long[]> Parse()
    {
        return SplitInput
            .Select(s => s
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray());
    }
}
