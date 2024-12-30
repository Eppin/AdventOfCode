namespace AdventOfCode._2024;

public class Day23 : Day
{
    public Day23() : base()
    {
    }

    public override string SolveA()
    {
        throw new NotImplementedException();
    }

    [Answer("co,de,ka,ta", Example, Data = "kh-tc{nl}qp-kh{nl}de-cg{nl}ka-co{nl}yn-aq{nl}qp-ub{nl}cg-tb{nl}vc-aq{nl}tb-ka{nl}wh-tc{nl}yn-cg{nl}kh-ub{nl}ta-co{nl}de-co{nl}tc-td{nl}tb-wq{nl}wh-td{nl}ta-ka{nl}td-qp{nl}aq-cg{nl}wq-ub{nl}ub-vc{nl}de-ta{nl}wq-aq{nl}wq-vc{nl}wh-yn{nl}ka-de{nl}kh-ta{nl}co-tc{nl}wh-qp{nl}tb-vc{nl}td-yn")]
    [Answer("af,aq,ck,ee,fb,it,kg,of,ol,rt,sc,vk,zh", Regular)]
    public override string SolveB()
    {
        var items = Parse();
        var combinations = new Dictionary<string, List<string>>();

        foreach (var item in items)
        {
            if (!combinations.TryAdd(item[0], [item[1]]))
                combinations[item[0]].Add(item[1]);

            if (!combinations.TryAdd(item[1], [item[0]]))
                combinations[item[1]].Add(item[0]);
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

    private string[][] Parse()
    {
        return GetSplitInput()
            .Select(s => s.Split('-'))
            .ToArray();
    }
}
