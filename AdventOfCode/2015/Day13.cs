namespace AdventOfCode._2015;

public partial class Day13 : Day
{
    public Day13() : base()
    {
    }

    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool partB)
    {
        var places = Parse();
        var people = places.SelectMany(p => new[] { p.Key.Item1, p.Key.Item2 }).ToHashSet();

        if (partB)
            people.Add("me");

        var maxHappiness = int.MinValue;
        var permutations = people.ToArray().Permutations();
        foreach (var persons in permutations)
        {
            var totalHappiness = 0;
            var prevPerson = persons[^1];

            foreach (var person in persons)
            {
                if (places.TryGetValue((prevPerson, person), out var prevHappiness))
                    totalHappiness += prevHappiness;

                if (places.TryGetValue((person, prevPerson), out var happiness))
                    totalHappiness += happiness;

                prevPerson = person;
            }

            maxHappiness = Math.Max(totalHappiness, maxHappiness);
        }

        return maxHappiness;
    }

    private Dictionary<(string, string), int> Parse()
    {
        var dict = new Dictionary<(string, string), int>();
        foreach (var input in SplitInput)
        {
            var match = ParseRegex().Match(input);

            if (!match.Success)
                continue;

            var person1 = match.Groups[1].Value;
            var direction = match.Groups[2].Value;
            var happiness = int.Parse(match.Groups[3].Value);
            var person2 = match.Groups[4].Value;

            dict[(person1, person2)] = direction.Equals("gain", StringComparison.InvariantCultureIgnoreCase)
                ? happiness
                : happiness * -1;
        }

        return dict;
    }

    [GeneratedRegex("^(\\w+) would (gain|lose) (\\d+) .* (\\w+).$")]
    private static partial Regex ParseRegex();
}
