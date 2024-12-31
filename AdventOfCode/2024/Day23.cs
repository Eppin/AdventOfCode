namespace AdventOfCode._2024;

public class Day23 : Day
{
    public Day23() : base()
    {
    }

    [Answer("7", Example, Data = "kh-tc{nl}qp-kh{nl}de-cg{nl}ka-co{nl}yn-aq{nl}qp-ub{nl}cg-tb{nl}vc-aq{nl}tb-ka{nl}wh-tc{nl}yn-cg{nl}kh-ub{nl}ta-co{nl}de-co{nl}tc-td{nl}tb-wq{nl}wh-td{nl}ta-ka{nl}td-qp{nl}aq-cg{nl}wq-ub{nl}ub-vc{nl}de-ta{nl}wq-aq{nl}wq-vc{nl}wh-yn{nl}ka-de{nl}kh-ta{nl}co-tc{nl}wh-qp{nl}tb-vc{nl}td-yn")]
    [Answer("1419", Regular)]
    public override string SolveA()
    {
        var items = Parse();
        var queue = new Queue<(Node Node, string End, int Depth, HashSet<Node> Visited)>();

        foreach (var node in items)
            queue.Enqueue((node, node.A, 0, [node]));

        var list = new HashSet<Unique>();

        while (queue.Count > 0)
        {
            var (node, end, depth, visited) = queue.Dequeue();

            if (depth == 2)
            {
                if (node.A == end || node.B == end)
                {
                    var uniques = visited
                        .Select(x => new List<string> { x.A, x.B })
                        .SelectMany(x => x)
                        .Distinct()
                        .Order()
                        .ToList();

                    list.Add(new Unique(uniques[0], uniques[1], uniques[2]));
                }

                continue;
            }

            foreach (var next in items.Where(i => !visited.Contains(i) && (i.A == node.B || i.B == node.B)))
            {
                queue.Enqueue((next, end, depth + 1, [.. visited, next]));
            }
        }

        return list
            .Distinct()
            .Count(t => t.A.StartsWith('t') || t.B.StartsWith('t') || t.C.StartsWith('t'))
            .ToString();
    }

    [Answer("co,de,ka,ta", Example, Data = "kh-tc{nl}qp-kh{nl}de-cg{nl}ka-co{nl}yn-aq{nl}qp-ub{nl}cg-tb{nl}vc-aq{nl}tb-ka{nl}wh-tc{nl}yn-cg{nl}kh-ub{nl}ta-co{nl}de-co{nl}tc-td{nl}tb-wq{nl}wh-td{nl}ta-ka{nl}td-qp{nl}aq-cg{nl}wq-ub{nl}ub-vc{nl}de-ta{nl}wq-aq{nl}wq-vc{nl}wh-yn{nl}ka-de{nl}kh-ta{nl}co-tc{nl}wh-qp{nl}tb-vc{nl}td-yn")]
    [Answer("af,aq,ck,ee,fb,it,kg,of,ol,rt,sc,vk,zh", Regular)]
    public override string SolveB()
    {
        var items = Parse();
        var combinations = new Dictionary<string, List<string>>();

        foreach (var item in items)
        {
            if (!combinations.TryAdd(item.A, [item.B]))
                combinations[item.A].Add(item.B);

            if (!combinations.TryAdd(item.B, [item.A]))
                combinations[item.B].Add(item.A);
        }

        var result = new List<string>();

        foreach (var (key, value) in combinations)
        {
            var keys = new HashSet<string> { key };
            var neighbours = combinations.Where(d => value.Contains(d.Key));

            foreach (var c2 in neighbours)
            {
                var v = c2.Value.Where(value.Contains);
                foreach (var y in v)
                    keys.Add(y);
            }

            var valid = false;
            foreach (var key2 in keys)
            {
                if (!keys.All(h => combinations[key2].Contains(h) || h == key2))
                {
                    valid = false;
                    break;
                }

                valid = true;
            }

            if (!valid || keys.Count <= result.Count) continue;

            result.Clear();
            result.AddRange(keys);
        }

        return string.Join(',', result.Order());
    }

    private List<Node> Parse()
    {
        return GetSplitInput()
            .Select(s =>
            {
                var split = s.Split('-');
                return new Node(split[0], split[1]);
            })
            .ToList();
    }

    private record struct Node(string A, string B);
    private record struct Unique(string A, string B, string C);
}
