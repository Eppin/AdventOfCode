namespace AdventOfCode._2024;

public partial class Day24 : Day
{
    public Day24() : base()
    {
    }

    [Answer("2024", Example, Data = "x00: 1{nl}x01: 0{nl}x02: 1{nl}x03: 1{nl}x04: 0{nl}y00: 1{nl}y01: 1{nl}y02: 1{nl}y03: 1{nl}y04: 1{nl}{nl}ntg XOR fgs -> mjb{nl}y02 OR x01 -> tnw{nl}kwq OR kpj -> z05{nl}x00 OR x03 -> fst{nl}tgd XOR rvg -> z01{nl}vdt OR tnw -> bfw{nl}bfw AND frj -> z10{nl}ffh OR nrd -> bqk{nl}y00 AND y03 -> djm{nl}y03 OR y00 -> psh{nl}bqk OR frj -> z08{nl}tnw OR fst -> frj{nl}gnj AND tgd -> z11{nl}bfw XOR mjb -> z00{nl}x03 OR x00 -> vdt{nl}gnj AND wpb -> z02{nl}x04 AND y00 -> kjc{nl}djm OR pbm -> qhw{nl}nrd AND vdt -> hwm{nl}kjc AND fst -> rvg{nl}y04 OR y02 -> fgs{nl}y01 AND x02 -> pbm{nl}ntg OR kjc -> kwq{nl}psh XOR fgs -> tgd{nl}qhw XOR tgd -> z09{nl}pbm OR djm -> kpj{nl}x03 XOR y03 -> ffh{nl}x00 XOR y04 -> ntg{nl}bfw OR bqk -> z06{nl}nrd XOR fgs -> wpb{nl}frj XOR qhw -> z04{nl}bqk OR frj -> z07{nl}y03 OR x01 -> nrd{nl}hwm AND bqk -> z03{nl}tgd XOR rvg -> z12{nl}tnw OR pbm -> gnj")]
    [Answer("43942008931358", Regular)]
    public override string SolveA()
    {
        var (inputs, wires) = Parse();

        var visited = new HashSet<Wire>();

        while (true)
        {
            if (visited.Count == wires.Count)
                break;

            foreach (var wire in wires.Where(w => !visited.Contains(w)))
            {
                if (!inputs.TryGetValue(wire.In1, out var in1) || !inputs.TryGetValue(wire.In2, out var in2))
                    continue;

                if (!inputs.TryAdd(wire.Out, Calculate(in1, in2, wire.Gate)))
                    throw new Exception("Key shouldn't exist?!");

                visited.Add(wire);
            }
        }

        var z = inputs
            .Where(i => i.Key.StartsWith('z'))
            .OrderByDescending(i => i.Key)
            .Select(z => z.Value);

        var zz = string.Join("", z);
        return Convert.ToInt64(zz, 2).ToString();
    }

    private static int Calculate(int in1, int in2, Gate gate)
    {
        return gate switch
        {
            Gate.Or => in1 | in2,
            Gate.Xor => in1 ^ in2,
            Gate.And => in1 & in2,
            _ => throw new ArgumentOutOfRangeException(nameof(gate), gate, null)
        };
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private (Dictionary<string, int> Inputs, List<Wire> Wires) Parse()
    {
        var split = GetSplitInput(false).ToList();
        var index = split.IndexOf("");

        var inputs = split.Take(index).Select(i =>
        {
            var s = i.Split(':', StringSplitOptions.TrimEntries);
            return new KeyValuePair<string, int>(s[0], int.Parse(s[1]));
        }).ToDictionary();


        var wires = split.Skip(index + 1).Select(w =>
        {
            var match = WireRegex().Match(w);
            if (!match.Success) throw new Exception($"Regex failed for {w}");

            var gate = match.Groups[2].Value switch
            {
                "OR" => Gate.Or,
                "XOR" => Gate.Xor,
                "AND" => Gate.And,
                _ => throw new ArgumentOutOfRangeException()
            };

            return new Wire(match.Groups[1].Value, match.Groups[3].Value, match.Groups[4].Value, gate);
        }).ToList();

        return (inputs, wires);
    }

    private record struct Wire(string In1, string In2, string Out, Gate Gate);

    private enum Gate
    {
        Or,
        Xor,
        And
    }

    [GeneratedRegex("(.*) (XOR|OR|AND) (.*) -> (.*)")]
    private static partial Regex WireRegex();
}
