namespace AdventOfCode._2021;

public class Day6 : Day
{
    public Day6() : base()
    {
    }

    [Answer("5934", Example, Data = "3,4,3,1,2")]
    [Answer("388739", Regular)]
    public override object SolveA()
    {
        return Solve(80);
    }

    [Answer("1741362314973", Regular)]
    public override object SolveB()
    {
        return Solve(256);
    }

    private long Solve(int days)
    {
        var fishes = Parse()
            .GroupBy(p => p)
            .ToDictionary(g => g.Key, g => (long)g.Count());

        for (var i = 0; i < 9; i++)
            fishes.TryAdd(i, 0);

        for (var i = 0; i < days; i++)
        {
            var tmp0 = fishes[0];

            fishes[0] = fishes[1];
            fishes[1] = fishes[2];
            fishes[2] = fishes[3];
            fishes[3] = fishes[4];
            fishes[4] = fishes[5];
            fishes[5] = fishes[6];
            fishes[6] = fishes[7] + tmp0;
            fishes[7] = fishes[8];
            fishes[8] = tmp0;
        }

        return fishes.Values.Sum();
    }

    private List<int> Parse()
    {
        return Input
            .Split(',')
            .Select(int.Parse)
            .ToList();
    }
}
