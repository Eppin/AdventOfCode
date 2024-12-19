namespace AdventOfCode._2024;

public class Day19 : Day
{
    public Day19() : base()
    {
    }

    [Answer("6", Example, Data = "r, wr, b, g, bwu, rb, gb, br{nl}{nl}brwrr{nl}bggr{nl}gbbr{nl}rrbgbr{nl}ubwu{nl}bwurrg{nl}brgr{nl}bbrgwb")]
    public override string SolveA()
    {
        Solve();
        return "";
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private void Solve()
    {
        var (patterns, wanted) = Parse();

        foreach (var want in wanted)
        {
            var success = Loop(patterns, want);
            Console.WriteLine($"{want}: {success}");
        }
    }

    private static bool Loop(List<string> patterns, string want)
    {
        var n = want;
        var success = false;

        var pattern = patterns.Where(want.StartsWith);
        foreach (var p in pattern)
        {
            //Console.WriteLine($"{p} => {want}");

            var index = want.IndexOf(p, StringComparison.Ordinal) + 1;
            n = want[index..];



            if (!string.IsNullOrWhiteSpace(n))
                success |= Loop(patterns, n);
            else
            {
                //Console.WriteLine($"Finished!! {want} vs {n}");
                return true;
            }
        }

        //Console.WriteLine($"Leftover? {want} vs {n}");
        return success;
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