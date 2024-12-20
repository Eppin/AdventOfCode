namespace AdventOfCode._2024;

using System.Diagnostics;

public class Day19 : Day
{
    public Day19() : base()
    {
    }

    [Answer("6", Example, Data = "r, wr, b, g, bwu, rb, gb, br{nl}{nl}brwrr{nl}bggr{nl}gbbr{nl}rrbgbr{nl}ubwu{nl}bwurrg{nl}brgr{nl}bbrgwb")]
    [Answer("322", Regular)]
    public override string SolveA()
    {
        return SolveA2().ToString();
    }

    [Answer("16", Example, Data = "r, wr, b, g, bwu, rb, gb, br{nl}{nl}brwrr{nl}bggr{nl}gbbr{nl}rrbgbr{nl}ubwu{nl}bwurrg{nl}brgr{nl}bbrgwb")]
    [Answer("715514563508258", Regular)]
    public override string SolveB()
    {
        return Solve().ToString();
    }

    private int SolveA2()
    {
        var (patterns, wanted) = Parse();
        var success = 0;

        foreach (var want in wanted)//.Take(1))
        {
            var sw = Stopwatch.StartNew();

            var possiblePatterns = patterns
                .Where(p => want.Contains(p))
                .OrderByDescending(w => w.Length)
                .ToList();

            cache.Clear();

            if (LoopA(possiblePatterns, want, ""))
                success++;

            //if ()
            //{
            //    //success++;
            //    _test += LoopB(possiblePatterns, want);
            //    Console.WriteLine($"{want}: {success}, {sw.Elapsed} => {_test}");

            //    //Console.WriteLine("Cache");
            //    //foreach (var (key, value) in cache)
            //    //{
            //    //    Console.WriteLine($"\t{key} = {value}");
            //    //}
            //}

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

    private IEnumerable<string> SolveB3()
    {
        var (patterns, wanted) = Parse();

        foreach (var want in wanted)//.Take(1))
        {
            var sw = Stopwatch.StartNew();

            var possiblePatterns = patterns
                .Where(p => want.Contains(p))
                .OrderByDescending(w => w.Length)
                .ToList();

            cache.Clear();

            if (LoopA(possiblePatterns, want, ""))
                yield return want;

            //if ()
            //{
            //    //success++;
            //    _test += LoopB(possiblePatterns, want);
            //    Console.WriteLine($"{want}: {success}, {sw.Elapsed} => {_test}");

            //    //Console.WriteLine("Cache");
            //    //foreach (var (key, value) in cache)
            //    //{
            //    //    Console.WriteLine($"\t{key} = {value}");
            //    //}
            //}

            //if (Test(possiblePatterns, want))
            //{
            //    success++;
            //    Console.WriteLine($"Success, {want}: {success}, {sw.Elapsed}");
            //}
            //else
            //    Console.WriteLine($"Failed: {want}: {success}, {sw.Elapsed}");
        }

        //return success;
    }

    //private int SolveB2()
    //{
    //    var (patterns, wanted) = Parse();

    //    foreach (var want in wanted)//.Take(1))
    //    {
    //        var sw = Stopwatch.StartNew();

    //        var possiblePatterns = patterns
    //            .Where(p => want.Contains(p))
    //            .OrderByDescending(w => w.Length)
    //            .ToList();

    //        cache.Clear();

    //        if (!LoopA(possiblePatterns, want, "")) continue;

    //        _test += LoopB(possiblePatterns, want);
    //        Console.WriteLine($"{want} {sw.Elapsed} => {_test}");

    //        //if ()
    //        //{
    //        //    //success++;
    //        //    _test += LoopB(possiblePatterns, want);
    //        //    Console.WriteLine($"{want}: {success}, {sw.Elapsed} => {_test}");

    //        //    //Console.WriteLine("Cache");
    //        //    //foreach (var (key, value) in cache)
    //        //    //{
    //        //    //    Console.WriteLine($"\t{key} = {value}");
    //        //    //}
    //        //}

    //        //if (Test(possiblePatterns, want))
    //        //{
    //        //    success++;
    //        //    Console.WriteLine($"Success, {want}: {success}, {sw.Elapsed}");
    //        //}
    //        //else
    //        //    Console.WriteLine($"Failed: {want}: {success}, {sw.Elapsed}");
    //    }

    //    return _test;
    //}

    private long Solve()
    {
        var (patterns, wanted) = Parse();

        var success = 0;

        _test = 0L;

        //var c = SolveB3().ToList();
        //Console.WriteLine($"C: {c.Count}");

        var l = new List<long>();

        foreach (var want in wanted)//.Skip(3).Take(1))
        {
            var sw = Stopwatch.StartNew();

            var possiblePatterns = patterns
                .Where(p => want.Contains(p))
                .OrderByDescending(w => w.Length)
                .ToList();

            cache2.Clear();

            //if ()
            {
                //success++;
                var t = LoopB(possiblePatterns, want);
                l.Add(t);
                _test += t;
                Console.WriteLine($"{want}: {success}, {sw.Elapsed} => {t} => {_test}");

                //Console.WriteLine("Cache");
                //foreach (var (key, value) in cache2)
                //{
                //    Console.WriteLine($"\t{key} = {value}");
                //}

                //Console.WriteLine("Cache");
                //foreach (var (key, value) in cache)
                //{
                //    Console.WriteLine($"\t{key} = {value}");
                //}
            }

            //if (Test(possiblePatterns, want))
            //{
            //    success++;
            //    Console.WriteLine($"Success, {want}: {success}, {sw.Elapsed}");
            //}
            //else
            //    Console.WriteLine($"Failed: {want}: {success}, {sw.Elapsed}");
        }

        Console.WriteLine($"{_test} => {l.Sum()}");

        return _test;
    }

    private static Dictionary<string, bool> cache = new();
    private static long _test = 0;

    //private static string _used = "";

    private static bool LoopA(List<string> patterns, string want, string used)
    {
        //Console.WriteLine($"Want: {want}, {used}");

        var loop = false;

        if (cache.TryGetValue(want, out var cacheValue))
        {
            //Console.WriteLine($"Cache hit!! {cacheValue}, {want}");
            loop |= cacheValue;
        }
        else
        {
            var pattern = patterns.Where(want.StartsWith);
            foreach (var p in pattern)
            {
                var index = want.IndexOf(p, StringComparison.Ordinal) + p.Length;
                var n = want[index..];

                if (!string.IsNullOrWhiteSpace(n))
                {
                    loop |= LoopA(patterns, n, $"{used},{p}");
                    cache.TryAdd(n, loop);
                }
                else
                {
                    loop = true;
                    //Console.WriteLine($"Path: {used}+{p}");
                }
            }
        }

        //Console.WriteLine("END LOOP");
        return loop;
    }

    private static Dictionary<string, long> cache2 = new();


    private static long LoopB(List<string> patterns, string want)
    {
        //Console.WriteLine($"Want: {want}");

        var loop = 0L;

        if (cache2.TryGetValue(want, out var cacheValue))
        {
            //Console.WriteLine($"Cache hit!! {cacheValue}, {want}");
            loop += cacheValue;
        }
        else
        {
            var pattern = patterns.Where(want.StartsWith);
            foreach (var p in pattern)
            {
                var index = want.IndexOf(p, StringComparison.Ordinal) + p.Length;
                var n = want[index..];

                //Console.WriteLine($"P: {p}, {index}, {n}");

                if (!string.IsNullOrWhiteSpace(n))
                {
                    var xy = LoopB(patterns, n);
                    loop += xy;
                    cache2.TryAdd(n, xy);
                }
                else
                {
                    loop = 1;
                    //Console.WriteLine($"Loop = 1");
                }
            }
        }

        //Console.WriteLine("END LOOP");
        return loop;
    }

    //private static int LoopB(List<string> patterns, string want)
    //{
    //    //Console.WriteLine($"Want: {want}");

    //    var count = 0;

    //    var pattern = patterns.Where(want.StartsWith);
    //    foreach (var p in pattern)
    //    {
    //        var index = want.IndexOf(p, StringComparison.Ordinal) + p.Length;
    //        var n = want[index..];

    //        if (!string.IsNullOrWhiteSpace(n))
    //        {
    //            count += LoopB(patterns, n);
    //        }
    //        else
    //        {
    //            count = 1;
    //            //Console.WriteLine($"Path: end");
    //        }
    //    }

    //    //Console.WriteLine("END LOOP");
    //    return count;
    //}



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
