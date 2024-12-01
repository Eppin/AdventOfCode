namespace AdventOfCode._2024;

public class Day1 : Day
{
    private readonly List<long> _first = [];
    private readonly List<long> _second = [];

    public Day1() : base()
    {
        Parse();
    }

    public override string SolveA()
    {
        return _first
            .Select((t, i) => Math.Abs(t - _second[i]))
            .Sum()
            .ToString();
    }

    public override string SolveB()
    {
        long similarity = 0;

        var dict = _second
            .GroupBy(s => s)
            .ToDictionary(g => g.Key, g => g.Count());

        foreach (var f in _first)
        {
            if (!dict.TryGetValue(f, out var count))
                continue;

            similarity += f * count;
        }

        return similarity.ToString();
    }

    private void Parse()
    {
        List<long> list1 = [];
        List<long> list2 = [];

        foreach (var line in SplitInput)
        {
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (split.Length != 2) throw new Exception();
            if (!long.TryParse(split[0], out var first)) throw new Exception();
            if (!long.TryParse(split[1], out var second)) throw new Exception();

            list1.Add(first);
            list2.Add(second);
        }

        // Ordering is only needed for Part A...
        _first.AddRange(list1.OrderBy(f => f));
        _second.AddRange(list2.OrderBy(f => f));
    }
}
