namespace AdventOfCode._2015;

public partial class Day16 : Day
{
    public Day16() : base()
    {
    }

    public override string SolveA()
    {
        var tickerTapeA = new Dictionary<string, int>
        {
            { "children", 3 },
            { "cats", 7 },
            { "samoyeds", 2 },
            { "pomeranians", 3 },
            { "akitas", 0 },
            { "vizslas", 0 },
            { "goldfish", 5 },
            { "trees", 3 },
            { "cars", 2 },
            { "perfumes", 1 }
        };

        var parsed = Parse().Where(p =>
            tickerTapeA[p.Kind1] == p.Value1 &&
            tickerTapeA[p.Kind2] == p.Value2 &&
            tickerTapeA[p.Kind3] == p.Value3);

        return parsed.Single().Sue.ToString();
    }

    public override string SolveB()
    {
        var tickerTapeB = new Dictionary<string, Func<int, bool>>
        {
            { "children", v => v == 3 },
            { "cats", v => v > 7 },
            { "samoyeds", v => v == 2 },
            { "pomeranians", v => v < 3 },
            { "akitas", v => v == 0 },
            { "vizslas", v => v == 0 },
            { "goldfish", v => v < 5 },
            { "trees", v => v > 3 },
            { "cars", v => v == 2 },
            { "perfumes", v => v == 1 }
        };

        var parsed = Parse().Where(p =>
            tickerTapeB[p.Kind1].Invoke(p.Value1) &&
            tickerTapeB[p.Kind2].Invoke(p.Value2) &&
            tickerTapeB[p.Kind3].Invoke(p.Value3));

        return parsed.Single().Sue.ToString();
    }

    private IEnumerable<Ingredient> Parse()
    {
        foreach (var input in SplitInput)
        {
            var regex = SueRegex().Match(input);
            if (!regex.Success)
                throw new DataException("Regex can't fail");

            var sue = int.Parse(regex.Groups[1].Value);

            var kind1 = regex.Groups[2].Value;
            var value1 = int.Parse(regex.Groups[3].Value);

            var kind2 = regex.Groups[4].Value;
            var value2 = int.Parse(regex.Groups[5].Value);

            var kind3 = regex.Groups[6].Value;
            var value3 = int.Parse(regex.Groups[7].Value);

            yield return new(sue, kind1, value1, kind2, value2, kind3, value3);
        }
    }

    private record struct Ingredient(int Sue, string Kind1, int Value1, string Kind2, int Value2, string Kind3, int Value3);

    [GeneratedRegex(@"^Sue (\d+): (.*): (\d+), (.*): (\d+), (.*): (\d+)$")]
    private static partial Regex SueRegex();
}
