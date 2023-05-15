namespace AdventOfCode._2015;

using System.Collections.ObjectModel;

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

        return FindMin(routes, places, partB);
    }

    private List<Node> _nodes = new();

    private int FindMin(Dictionary<(string from, string to), int> routes, HashSet<string> places, bool partB)
    {
        var numbers = new List<int>();

        foreach (var place in places)
        {
            var toGo = places.Except(new[] { place });
            // Console.WriteLine($"Start: {place} -> {string.Join(',', toGo)}");
            var node = new Node { Place = place };
            var x = FindMin(place, routes, toGo, node);
            
            var j = partB 
                ? CalculateMax(x,0) 
                : CalculateMin(x, 0);
            
            numbers.Add(j);
        }

        return partB 
            ? numbers.Max() 
            : numbers.Min();
    }

    private static List<Node> FindMin(string current, Dictionary<(string from, string to), int> routes, IEnumerable<string> places, Node node)
    {
        var results = new List<Node>();

        var possibleRoutes = routes.Where(r => r.Key.from.Equals(current) && places.Contains(r.Key.to));
        foreach (var ((from, to), cost) in possibleRoutes)
        {
            var toGo = places.Except(new[] { from });
            // Console.WriteLine($"{current} -> {string.Join(',', toGo)}");

            results.Add(new Node
            {
                Place = to,
                Cost = cost,
                Nodes = FindMin(to, routes, toGo, node)
            });
        }

        return results;
    }

    private int CalculateMin(List<Node> nodes, int costs)
    {
        var node = nodes.MinBy(n => n.Cost);
        // Console.WriteLine($"N: {node?.Place}");
        
        if (node != null)
            costs += CalculateMin(node.Nodes, node.Cost);

        return costs;
    }
    
    private int CalculateMax(List<Node> nodes, int costs)
    {
        var node = nodes.MaxBy(n => n.Cost);
        // Console.WriteLine($"N: {node?.Place}");
        
        if (node != null)
            costs += CalculateMax(node.Nodes, node.Cost);

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
