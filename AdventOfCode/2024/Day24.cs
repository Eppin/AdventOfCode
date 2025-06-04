namespace AdventOfCode._2024;

public partial class Day24 : Day
{
    public Day24() : base()
    {
    }

    [Answer("2024", Example, Data = "x00: 1{nl}x01: 0{nl}x02: 1{nl}x03: 1{nl}x04: 0{nl}y00: 1{nl}y01: 1{nl}y02: 1{nl}y03: 1{nl}y04: 1{nl}{nl}ntg XOR fgs -> mjb{nl}y02 OR x01 -> tnw{nl}kwq OR kpj -> z05{nl}x00 OR x03 -> fst{nl}tgd XOR rvg -> z01{nl}vdt OR tnw -> bfw{nl}bfw AND frj -> z10{nl}ffh OR nrd -> bqk{nl}y00 AND y03 -> djm{nl}y03 OR y00 -> psh{nl}bqk OR frj -> z08{nl}tnw OR fst -> frj{nl}gnj AND tgd -> z11{nl}bfw XOR mjb -> z00{nl}x03 OR x00 -> vdt{nl}gnj AND wpb -> z02{nl}x04 AND y00 -> kjc{nl}djm OR pbm -> qhw{nl}nrd AND vdt -> hwm{nl}kjc AND fst -> rvg{nl}y04 OR y02 -> fgs{nl}y01 AND x02 -> pbm{nl}ntg OR kjc -> kwq{nl}psh XOR fgs -> tgd{nl}qhw XOR tgd -> z09{nl}pbm OR djm -> kpj{nl}x03 XOR y03 -> ffh{nl}x00 XOR y04 -> ntg{nl}bfw OR bqk -> z06{nl}nrd XOR fgs -> wpb{nl}frj XOR qhw -> z04{nl}bqk OR frj -> z07{nl}y03 OR x01 -> nrd{nl}hwm AND bqk -> z03{nl}tgd XOR rvg -> z12{nl}tnw OR pbm -> gnj")]
    [Answer("43942008931358", Regular)]
    public override object SolveA()
    {
        var (inputs, wires) = Parse();

        VisitWires(wires, inputs);

        return GetInt64(inputs, i => i.Key.StartsWith('z')).Numeric;
    }

    [Answer("z00,z01,z02,z05", Example, Data = "x00: 0{nl}x01: 1{nl}x02: 0{nl}x03: 1{nl}x04: 0{nl}x05: 1{nl}y00: 0{nl}y01: 0{nl}y02: 1{nl}y03: 1{nl}y04: 0{nl}y05: 1{nl}{nl}x00 AND y00 -> z05{nl}x01 AND y01 -> z02{nl}x02 AND y02 -> z01{nl}x03 AND y03 -> z03{nl}x04 AND y04 -> z04{nl}x05 AND y05 -> z00")]
    [Answer("dvb,fhg,fsq,tnc,vcf,z10,z17,z39", Regular)]
    public override object SolveB()
    {
        var (_, wires) = Parse();

        var falseWires = new List<string>();

        var outputWires = wires
            .Select(w => w.Out)
            .Where(w => w.StartsWith('z'))
            .ToList();

        // Proper order of gates are:
        // AND -> OR
        // XOR -> AND
        // But different ruling for inputs and outputs 
        foreach (var wire in wires)
        {
            // starting wires should be followed by OR if AND, and by AND if XOR, except for the first one
            if ((wire.In1.StartsWith('x') || wire.In2.StartsWith('x')) && !wire.In1.Contains("00") && !wire.In2.Contains("00"))
            {
                foreach (var secondWire in wires)
                {
                    if (wire.Out != secondWire.In1 && wire.Out != secondWire.In2) continue;
                    if ((wire.Gate == Gate.And && secondWire.Gate == Gate.And) || (wire.Gate == Gate.Xor && secondWire.Gate == Gate.Or))
                        falseWires.Add(wire.Out);
                }
            }

            // wires in the middle should not have XOR operators
            if (!wire.In1.StartsWith('x') && !wire.In2.StartsWith('x') && !wire.Out.StartsWith('z') && wire.Gate == Gate.Xor)
                falseWires.Add(wire.Out);

            // wires at the end should always have XOR operators, except for the last one
            if (outputWires.Contains(wire.Out) && !wire.Out.Equals($"z{outputWires.Count - 1}") && wire.Gate != Gate.Xor)
                falseWires.Add(wire.Out);
        }

        return string.Join(",", falseWires.Order());
    }

    private static void VisitWires(List<Wire> wires, Dictionary<string, int> inputs)
    {
        var visited = new HashSet<Wire>();

        while (true)
        {
            if (visited.Count == wires.Count)
                break;

            var availableWires = wires
                .Where(w => !visited.Contains(w) && inputs.ContainsKey(w.In1) && inputs.ContainsKey(w.In2))
                .ToList();

            if (availableWires.Count == 0) return;

            foreach (var wire in availableWires)
            {
                if (!inputs.TryGetValue(wire.In1, out var in1) || !inputs.TryGetValue(wire.In2, out var in2))
                    continue;

                if (!inputs.TryAdd(wire.Out, Calculate(in1, in2, wire.Gate)))
                    throw new Exception("Key shouldn't exist?!");

                visited.Add(wire);
            }
        }
    }

    private static (long Numeric, string Bytes) GetInt64(Dictionary<string, int> inputs, Func<KeyValuePair<string, int>, bool> predicate)
    {
        var z = inputs
            .Where(predicate)
            .OrderByDescending(i => i.Key)
            .Select(z => z.Value);

        var zz = string.Join("", z);
        return (Convert.ToInt64(zz, 2), zz);
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
