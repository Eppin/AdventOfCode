namespace AdventOfCode._2015;

using Utils;

public class Day19 : Day
{
    public Day19() : base()
    {
    }
    
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
        var (replacements, starting) = Parse();

        var results = new List<string>();
        foreach (var replacement in replacements)
        {
            Console.WriteLine($"{replacement}");

            for (var i = 0;; i += replacement.Search.Length)
            {
                i = starting.IndexOf(replacement.Search, i, StringComparison.Ordinal);
                
                if(i == -1)
                    break;
                
                var newS = starting.ReplaceAt(i, replacement.Search.Length, replacement.Replace);
                results.Add(newS);
            }
        }

        return results.Distinct().Count();
    }

    private (IEnumerable<Replacement> Replacements, string Starting) Parse()
    {
        var replacements = SplitInput.Take(SplitInput.Count() - 1).Select(s =>
        {
            var split = s.Split("=>", StringSplitOptions.TrimEntries);
            return new Replacement(split[0], split[1]);
        });

        return (replacements, SplitInput.Last());
    }
    
    private record struct Replacement(string Search, string Replace);
}
