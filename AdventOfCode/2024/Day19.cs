namespace AdventOfCode._2024;

using System.Diagnostics;

public class Day19 : Day
{
    public Day19() : base()
    {
    }

    [Answer("6", Example, Data = "r, wr, b, g, bwu, rb, gb, br{nl}{nl}brwrr{nl}bggr{nl}gbbr{nl}rrbgbr{nl}ubwu{nl}bwurrg{nl}brgr{nl}bbrgwb")]
    [Answer("", Regular)]
    public override string SolveA()
    {
        return Solve().ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private int Solve()
    {
        var (patterns, wanted) = Parse();

        var success = 0;

        foreach (var want in wanted)//.Take(3))//.Skip(0).Take(1))//.Take(1))
        {
            var sw = Stopwatch.StartNew();

            var possiblePatterns = patterns
                .Where(p => want.Contains(p))
                .OrderByDescending(w => w.Length)
                .ToList();

            cache.Clear();
            if (Loop(possiblePatterns, want))
                success++;

            Console.WriteLine($"{want}: {success}, {sw.Elapsed}");

            Console.WriteLine("Cache");
            foreach (var (key, value) in cache)
            {
                Console.WriteLine($"\t{key} = {value}");
            }

            //if (Test(possiblePatterns, want))
            //{
            //    success++;
            //    Console.WriteLine($"Success, {want}: {success}, {sw.Elapsed}");
            //}
            //else
            //    Console.WriteLine($"Failed: {want}: {success}, {sw.Elapsed}");
        }

        return success;
    }

    // private static void Loop(List<string> patterns, string want)
    // {
    //     var shouldStop = false;
    //     
    //     while (!shouldStop)
    //     {
    //         foreach (var pattern in patterns)
    //         {
    //             var index = want.IndexOf(pattern, StringComparison.Ordinal);
    //             if (index == -1)
    //             {
    //                 // Console.WriteLine($"No match found: {match.Want}/{match.New} => {pattern}");
    //                 shouldStop = true;
    //                 break;
    //             }
    //
    //             var n = match.New[(index + pattern.Length)..];
    //
    //             if (!string.IsNullOrWhiteSpace(n))
    //                 queue.Enqueue(match with { New = n });
    //             else
    //             {
    //                 Console.WriteLine($"Match found for {match.Want}/{match.New}");
    //                 finishes[match.Want] = true;
    //             }
    //         }
    //     }
    // }

    //private static bool Test2(List<string> patterns, string want)
    //{
    //    // var finished = false;
    //    var success = false;

    //    var queue = new PriorityQueue<Match, int>();
    //    queue.Enqueue(new Match(want, want), want.Length);

    //    while (queue.TryDequeue(out var match, out _))
    //    {
    //        // if (finished) break;

    //        foreach (var pattern in patterns)
    //        {
    //            if (!match.New.StartsWith(pattern))
    //                continue;

    //            var index = match.New.IndexOf(pattern, StringComparison.Ordinal);
    //            if (index == -1)
    //                continue;

    //            var n = match.New[(index + pattern.Length)..];

    //            if (string.IsNullOrWhiteSpace(n))
    //            {
    //                Console.WriteLine($"Match found for {match.Want}/{match.New}");
    //                // finished = true;
    //                success = true; // double break!! Stop loop, return?!
    //            }
    //            else if (!patterns.Any(p => n.Contains(p)))
    //            {
    //                // finished = true;
    //                success = false;
    //            }
    //            else
    //            {
    //                queue.Enqueue(match with { New = n }, n.Length);
    //                Console.WriteLine($"Failed, continue?! {n}");
    //            }
    //        }

    //        Console.WriteLine($"Failure?!: {match.Want}: {match.New}");
    //    }

    //    return success;
    //}

    private static bool Test(List<string> patterns, string want)
    {
        var cache = new Dictionary<string, int>();

        // var finished = false;
        var success = false;

        var queue = new PriorityQueue<Match, int>();
        queue.Enqueue(new Match(want, want), want.Length);

        while (queue.TryDequeue(out var match, out _))
        {
            if (cache.TryGetValue(match.New, out var cacheValue))
            {
                Console.WriteLine($"Want {match.Want}/{match.New} found in cache {cacheValue}");
            }

            foreach (var pattern in patterns)
            {
                if (!match.New.StartsWith(pattern))
                    continue;

                var index = match.New.IndexOf(pattern, StringComparison.Ordinal);
                if (index == -1)
                    continue;

                var n = match.New[(index + pattern.Length)..];

                if (string.IsNullOrWhiteSpace(n))
                {
                    Console.WriteLine($"Match found for {match.Want}/{match.New}");
                    // finished = true;
                    success = true; // double break!! Stop loop, return?!
                    //return true;
                }
                else if (!patterns.Any(p => n.Contains(p)))
                {
                    // finished = true;
                    success = false;
                }
                else
                {
                    queue.Enqueue(match with { New = n }, n.Length);
                    Console.WriteLine($"Failed, continue?! {n}");
                    //break;
                }
            }

            Console.WriteLine($"Failure?!: {match.Want}: {match.New}");
        }

        return success;
    }

    private static Dictionary<string, bool> cache = new();

    private static bool Loop(List<string> patterns, string want)
    {
        if (cache.TryGetValue(want, out var cacheValue))
        {
            Console.WriteLine($"CacheValue for {want} is {cacheValue}");
            return cacheValue;
        }

        var pattern = patterns.Where(want.StartsWith);
        foreach (var p in pattern)
        {
            var index = want.IndexOf(p, StringComparison.Ordinal) + p.Length;
            var n = want[index..];

            if (!string.IsNullOrWhiteSpace(n))
            {
                var loop = Loop(patterns, n);
                cache.TryAdd(n, loop);

                if (loop)
                    return true;

                //if (Loop(patterns, n))
                //    return true;
            }
            else
                return true;
        }

        return false;
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

    private record struct Match(string Want, string New);
}
