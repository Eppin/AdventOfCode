namespace AdventOfCode._2024;

public class Day19 : Day
{
    public Day19() : base()
    {
    }

    [Answer("6", Example, Data = "r, wr, b, g, bwu, rb, gb, br{nl}{nl}brwrr{nl}bggr{nl}gbbr{nl}rrbgbr{nl}ubwu{nl}bwurrg{nl}brgr{nl}bbrgwb")]
    [Answer("322", Regular)]
    public override object SolveA()
    {
        return Solve(false);
    }

    [Answer("16", Example, Data = "r, wr, b, g, bwu, rb, gb, br{nl}{nl}brwrr{nl}bggr{nl}gbbr{nl}rrbgbr{nl}ubwu{nl}bwurrg{nl}brgr{nl}bbrgwb")]
    [Answer("715514563508258", Regular)]
    public override object SolveB()
    {
        return Solve(true);
    }

    private long Solve(bool isPartB)
    {
        var (patterns, wanted) = Parse();

        var total = 0L;

        foreach (var want in wanted)
        {
            var possiblePatterns = patterns
                .Where(want.Contains)
                .OrderByDescending(w => w.Length)
                .ToList();

            DesignCache.Clear();
            var loop = Loop(possiblePatterns, want);

            // For part A we're only interested in valid combinations, but for part 2
            // we want to know how many different combinations are possible
            if (isPartB) total += loop;
            else if (loop > 0) total++;
        }

        return total;
    }

    // Cache designs and avoid very long recursive loops
    // instead, return cached value when a certain pattern is detected
    private static readonly Dictionary<string, long> DesignCache = new();

    private static long Loop(List<string> patterns, string want)
    {
        var total = 0L;

        if (DesignCache.TryGetValue(want, out var cacheValue))
            total += cacheValue;
        else
        {
            var pattern = patterns.Where(want.StartsWith);
            foreach (var p in pattern)
            {
                var index = want.IndexOf(p, StringComparison.Ordinal) + p.Length;
                var n = want[index..];

                if (!string.IsNullOrWhiteSpace(n))
                {
                    var loop = Loop(patterns, n);
                    total += loop;
                    DesignCache.TryAdd(n, loop);
                }
                else
                    total = 1;
            }
        }

        return total;
    }

    private (List<string> Patterns, List<string> Wanted) Parse()
    {
        var lines = GetSplitInput();

        var patterns = lines[0]
            .Split(',', StringSplitOptions.TrimEntries)
            .ToList();

        var wanted = lines.Skip(1).ToList();

        return (patterns, wanted);
    }
}
