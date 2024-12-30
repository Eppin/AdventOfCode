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

        foreach (var node in items)//.Skip(1).Take(1))
        {
            queue.Enqueue((node, node.A, 0, [node]));
            //queue.Enqueue((node, node.B, 0));
        }

        var list = new HashSet<Unique>();

        while (queue.Count > 0)
        {
            var (node, end, depth, visited) = queue.Dequeue();
            //Console.WriteLine($"N:{node}, E:{end}, D:{depth}, V:{string.Join(',', visited)}");

            if (depth == 2)
            {
                if (node.A == end || node.B == end)
                {
                    //Console.WriteLine("OK!");

                    var k = visited.Select(x => new List<string> { x.A, x.B }).SelectMany(x => x).Distinct().Order().ToList();
                    //Console.WriteLine($"N:{node}, E:{end}, D:{depth}, V:{string.Join(',', k)}"); // {string.Join(',', visited)}
                    list.Add(new Unique(k[0], k[1], k[2]));
                    //Console.WriteLine($"ADD: {string.Join(',', k)}");
                }

                continue;
            }

            foreach (var next in items.Where(i => !visited.Contains(i) && (i.A == node.B || i.B == node.B)))
            {
                queue.Enqueue((next, end, depth + 1, [.. visited, next]));
            }
        }

        var count = 0;
        foreach (var k in list.Distinct().Where(t => t.A.StartsWith('t') || t.B.StartsWith('t') || t.C.StartsWith('t')))//.OrderBy(x => x[0]))
        {
            Console.WriteLine($"V:{string.Join(',', k)}");
            count++;
        }

        return count.ToString();
    }

    private static void Loop(List<Node> items, Node node, string end, int depth)
    {


        Console.WriteLine($"N:{node}, E:{end}, D:{depth}");

        if (depth == 3)
        {
            if (node.A == end)
                Console.WriteLine("OK!");
            else
            {
                Console.WriteLine("FAIL!!");
                return;
            }
        }

        foreach (var next in items.Where(i => i != node && (i.A == node.B || i.B == node.B)))
        {
            //Loop(items, a, b, depth + 1);
            Loop(items, next, end, depth + 1);
        }
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
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
