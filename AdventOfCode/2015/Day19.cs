namespace AdventOfCode._2015;

using Utils;

public class Day19 : Day
{
    public Day19() : base()
    {
    }

    public override string SolveA()
    {
        var (replacements, starting) = Parse();

        var results = new List<string>();
        foreach (var replacement in replacements)
        {
            for (var i = 0;; i += replacement.Search.Length)
            {
                i = starting.IndexOf(replacement.Search, i, StringComparison.Ordinal);

                if (i == -1)
                    break;

                var newS = starting.ReplaceAt(i, replacement.Search.Length, replacement.Replace);
                results.Add(newS);
            }
        }

        return results.Distinct().Count().ToString();
    }

    public override string SolveB()
    {
        var (replacements, result) = Parse();

        // Remember original string
        var original = result;
        var steps = 0;

        Shuffle(replacements);

        do
        {
            result = Solve(replacements, result);

            if (string.IsNullOrWhiteSpace(result))
            {
                steps = 0;
                result = original;
                Shuffle(replacements);
            }
            else
                steps++;

        } while (result != "e");

        return steps.ToString();
    }

    private static string Solve(List<Replacement> replacements, string value)
    {
        foreach (var replacement in replacements)
        {
            var i = value.IndexOf(replacement.Replace, 0, StringComparison.Ordinal);

            if (i == -1)
                continue;

            return value.ReplaceAt(i, replacement.Replace.Length, replacement.Search);
        }

        return string.Empty;
    }

    private static readonly Random Random = new();

    private static void Shuffle<T>(IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = Random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    private (List<Replacement> Replacements, string Starting) Parse()
    {
        var replacements = SplitInput.Take(SplitInput.Count() - 1).Select(s =>
        {
            var split = s.Split("=>", StringSplitOptions.TrimEntries);
            return new Replacement(split[0], split[1]);
        }).ToList();

        return (replacements, SplitInput.Last());
    }

    private record struct Replacement(string Search, string Replace);
}
