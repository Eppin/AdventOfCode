using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode._2024;

public class Day11 : Day
{
    public Day11() : base()
    {
    }

    [Answer("55312", Example, Data = "125 17")]
    [Answer("233050", Regular)]
    public override string SolveA()
    {
        Solve(25);
        return "";
    }

    [Answer("", Example, Data = "125 17")]
    [Answer("276661131175807", Regular)]
    public override string SolveB()
    {
        Solve(75);
        return "";
    }

    private void Solve(int times)
    {
        var input = Parse()
            .GroupBy(p => p)
            .ToDictionary(g => g.Key, g => (long)g.Count());

        for (var i = 0; i <= times; i++)
        {
            //var remember = new List<long>();

            var dict = new Dictionary<long, long>(); // Number, count/amount

            foreach (var (key, amount) in input)
            {
                var blink = Blink(key);

                //var sw = Stopwatch.StartNew();

                foreach (var bl in blink)
                {
                    if (!dict.TryAdd(bl, amount))
                    {
                        dict[bl] += amount;
                    }
                }

                //Console.WriteLine($"After dict: {sw.Elapsed}, {dict.Count}");

                //remember.AddRange(blink);
                //sw.Stop();
                //Console.Write(" <> " + string.Join(",", blink));
                //Console.WriteLine($"After total: {sw.Elapsed}");
            }

            //Console.WriteLine($"-- Stones: {dict.Sum(d => d.Value)} after {i + 1} blinks (dict size: {dict.Count})");// vs input: {remember.Count})");
            //Console.WriteLine($"{string.Join(',', dict.Select(d => $"{d.Key}-{d.Value}"))}");
            //Console.WriteLine();

            //input = remember;
            input = dict;

        }
    }

    private static long[] Blink(long n)
    {
        if (n == 0) { return [1]; }

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
