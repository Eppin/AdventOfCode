namespace AdventOfCode._2015;

public class Day9 : Day
{
    public Day9() : base()
    {
    }

    [Answer("251", Regular)]
    public override object SolveA()
    {
        return Solve(false);
    }

    [Answer("898", Regular)]
    public override object SolveB()
    {
        return Solve(true);
    }

    private int Solve(bool partB)
    {
        var lines = ParseLines();
        var routes = new Dictionary<(string from, string to), int>();

        foreach (var route in lines)
        {
            routes[(route.From, route.To)] = route.Cost;
            routes[(route.To, route.From)] = route.Cost;
        }

        var places = lines
            .SelectMany(route => new[] { route.From, route.To })
            .ToHashSet();

        var minOrMaxCosts = !partB
            ? int.MaxValue
            : int.MinValue;

        var permutations = places.ToArray().Permutations();
        foreach (var permutation in permutations)
        {
            var totalCosts = 0;

            for (var i = 0; i < permutation.Length - 1; i++)
            {
                if (i > permutation.Length - 1)
                    break;

                var current = permutation[i];
                var next = permutation[i + 1];

                if (routes.TryGetValue((current, next), out var cost))
                    totalCosts += cost;
            }

            minOrMaxCosts = !partB
                ? Math.Min(totalCosts, minOrMaxCosts)
                : Math.Max(totalCosts, minOrMaxCosts);
        }

        return minOrMaxCosts;
    }

    private List<Route> ParseLines()
    {
        var routes = new List<Route>();

        foreach (var line in SplitInput)
        {
            var split = line.Split(' ');

            if (!int.TryParse(split[4], out var cost))
                throw new InvalidDataException($"Couldn't parse [{split[4]}]");

            routes.Add(new Route(split[0], split[2], cost));
        }

        return routes;
    }

    private record struct Route(string From, string To, int Cost);
}
