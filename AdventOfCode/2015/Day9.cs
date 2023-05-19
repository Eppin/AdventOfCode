namespace AdventOfCode._2015;

public class Day9 : Day
{
    public Day9() : base()
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

        return FindRoutes(routes, places, partB);
    }

    private static int FindRoutes(Dictionary<(string from, string to), int> routes, IReadOnlyCollection<string> places, bool partB)
    {
        var numbers = (from place in places
            let toGo = places.Except(new[] { place })
            select FindRoutes(place, routes, toGo)
            into possibleRoutes
            select Calculate(possibleRoutes, 0, partB)).ToList();

        return partB
            ? numbers.Max()
            : numbers.Min();
    }

    private static List<Node> FindRoutes(string current, Dictionary<(string from, string to), int> routes, IEnumerable<string> places)
    {
        var results = new List<Node>();

        var possibleRoutes = routes.Where(r => r.Key.from.Equals(current) && places.Contains(r.Key.to));
        foreach (var ((from, to), cost) in possibleRoutes)
        {
            var toGo = places.Except(new[] { from });

            results.Add(new Node
            {
                Place = to,
                Cost = cost,
                Nodes = FindRoutes(to, routes, toGo)
            });
        }

        return results;
    }

    private static int Calculate(IEnumerable<Node> nodes, int costs, bool partB)
    {
        var node = !partB
            ? nodes.MinBy(n => n.Cost)
            : nodes.MaxBy(n => n.Cost);

        if (node != null)
            costs += Calculate(node.Nodes, node.Cost, partB);

        return costs;
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

    private class Node
    {
        public string Place { get; set; }
        public int Cost { get; set; }
        public List<Node> Nodes { get; set; } = new();
    }
}
