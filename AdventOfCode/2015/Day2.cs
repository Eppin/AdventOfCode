namespace AdventOfCode._2015;

public class Day2 : Day
{
    public Day2() : base()
    {
    }

    public override string SolveA()
    {
        var result = 0;
        foreach (var str in SplitInput)
        {
            var split = str.Split('x');

            var l = int.Parse(split[0]);
            var w = int.Parse(split[1]);
            var h = int.Parse(split[2]);

            var area1 = l * w;
            var area2 = w * h;
            var area3 = h * l;

            var areas = new[] { area1, area2, area3 };

            result += areas.Select(a => 2 * a).Sum();
            result += areas.Min();
        }

        return $"{result}";
    }

    public override string SolveB()
    {
        var result = 0;
        foreach (var str in SplitInput)
        {
            var split = str.Split('x');

            var l = int.Parse(split[0]);
            var w = int.Parse(split[1]);
            var h = int.Parse(split[2]);

            var wrap = new[] { l, w, h }
                .Order()
                .Take(2)
                .Sum(i => i + i);

            var ribbon = l * w * h;
            result += wrap + ribbon;
        }

        return $"{result}";
    }
}