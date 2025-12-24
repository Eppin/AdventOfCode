using System.Xml.Linq;

namespace AdventOfCode._2025;

public class Day11 : Day
{
    public Day11() : base()
    {
        FlowCache.Clear();
    }

    [Answer("5", Example, Data = "aaa: you hhh{nl}you: bbb ccc{nl}bbb: ddd eee{nl}ccc: ddd eee fff{nl}ddd: ggg{nl}eee: out{nl}fff: out{nl}ggg: out{nl}hhh: ccc fff iii{nl}iii: out")]
    [Answer("428", Regular)]
    public override object SolveA()
    {
        var parsed = Parse();
        var you = parsed["you"];

        // For this part we don't need to track if 'fft' and 'dac' are visited
        return Solve(parsed, you, true, true);
    }

    [Answer("2", Example, Data = "svr: aaa bbb{nl}aaa: fft{nl}fft: ccc{nl}bbb: tty{nl}tty: ccc{nl}ccc: ddd eee{nl}ddd: hub{nl}hub: fff{nl}eee: dac{nl}dac: fff{nl}fff: ggg hhh{nl}ggg: out{nl}hhh: out")]
    [Answer("331468292364745", Regular)]
    public override object SolveB()
    {
        var parsed = Parse();
        var svr = parsed["svr"];

        return Solve(parsed, svr, false, false);
    }

    // Cache stores counts for each combination of visited flags:
    // index 0 = neither visited, 1 = fft visited, 2 = dac visited, 3 = both visited
    private static readonly Dictionary<string, long[]> FlowCache = new();

    private static long Solve(Dictionary<string, string[]> patterns, string[] current, bool visitedFourier, bool visitedDac)
    {
        var key = string.Join(",", current);
        var idx = (visitedFourier ? 1 : 0) | (visitedDac ? 2 : 0);

        if (!FlowCache.TryGetValue(key, out var cached))
        {
            cached = new long[4];
            for (var i = 0; i < 4; i++) cached[i] = -1;
            FlowCache[key] = cached;
        }

        if (cached[idx] != -1)
        {
            // cached value for this state + flags
            return cached[idx];
        }

        long total = 0;

        if (current[0] == "out")
        {
            total = (visitedFourier && visitedDac) ? 1 : 0;
            cached[idx] = total;
            return total;
        }

        foreach (var next in current)
        {
            if (!patterns.TryGetValue(next, out var neighbors)) continue;

            var newF = visitedFourier || next == "fft";
            var newD = visitedDac || next == "dac";

            total += Solve(patterns, neighbors, newF, newD);
        }

        cached[idx] = total;
        return total;
    }

    private Dictionary<string, string[]> Parse()
    {
        return SplitInput.Select(s =>
        {
            var parts = s.Split(':', StringSplitOptions.TrimEntries);

            var key = parts[0];
            var values = parts[1].Split(' ', StringSplitOptions.TrimEntries);

            return new KeyValuePair<string, string[]>(key, values);
        }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}
