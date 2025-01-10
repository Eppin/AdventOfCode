namespace AdventOfCode._2024;

public class Day11 : Day
{
    public Day11() : base()
    {
    }

    [Answer("55312", Example, Data = "125 17")]
    [Answer("233050", Regular)]
    public override object SolveA()
    {
        return Solve(25);
    }

    [Answer("276661131175807", Regular)]
    public override object SolveB()
    {
        return Solve(75);
    }

    private long Solve(int times)
    {
        var input = Parse()
            .GroupBy(p => p)
            .ToDictionary(g => g.Key, g => (long)g.Count());

        for (var i = 0; i < times; i++)
        {
            var dict = new Dictionary<long, long>();

            foreach (var (key, amount) in input)
            {
                var blink = Blink(key);

                foreach (var bl in blink)
                {
                    if (!dict.TryAdd(bl, amount))
                    {
                        dict[bl] += amount;
                    }
                }
            }

            input = dict;
        }

        return input.Sum(d => d.Value);
    }

    private static long[] Blink(long n)
    {
        if (n == 0)
        {
            return [1];
        }

        var length = n.Length();
        if (length % 2 == 0)
        {
            var split = n.Split(length / 2);
            return [split[0], split[1]];
        }

        return [n * 2024];
    }

    private List<long> Parse()
    {
        return Input
            .Split(' ')
            .Select(long.Parse)
            .ToList();
    }
}
