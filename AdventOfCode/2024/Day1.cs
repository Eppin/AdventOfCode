namespace AdventOfCode._2024;

public class Day1 : Day
{
    public Day1() : base()
    {
    }

    [Answer("11", Example, Data = "3   4{nl}4   3{nl}2   5{nl}1   3{nl}3   9{nl}3   3")]
    [Answer("2769675", Regular)]
    public override string SolveA()
    {
        var (first, second) = Parse();

        return first
            .Select((t, i) => Math.Abs(t - second[i]))
            .Sum()
            .ToString();
    }

    [Answer("31", Example, Data = "3   4{nl}4   3{nl}2   5{nl}1   3{nl}3   9{nl}3   3")]
    [Answer("24643097", Regular)]
    public override string SolveB()
    {
        var (first, second) = Parse();

        long similarity = 0;

        var dict = second
            .GroupBy(s => s)
            .ToDictionary(g => g.Key, g => g.Count());

        foreach (var f in first)
        {
            if (!dict.TryGetValue(f, out var count))
                continue;

            similarity += f * count;
        }

        return similarity.ToString();
    }

    private (long[] First, long[] Second) Parse()
    {
        List<long> list1 = [];
        List<long> list2 = [];

        foreach (var line in GetSplitInput())
        {
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (split.Length != 2) throw new Exception();
            if (!long.TryParse(split[0], out var first)) throw new Exception();
            if (!long.TryParse(split[1], out var second)) throw new Exception();

            list1.Add(first);
            list2.Add(second);
        }

        // Ordering is only needed for Part A...
        return (list1.OrderBy(f => f).ToArray(), list2.OrderBy(f => f).ToArray());
    }
}
