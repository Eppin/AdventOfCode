namespace AdventOfCode._2024;

using System.Security.Cryptography;

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

        VisitWires(wires, inputs);

        var z = inputs
            .Where(i => i.Key.StartsWith('z'))
            .OrderByDescending(i => i.Key)
            .Select(z => z.Value);

        var zz = string.Join("", z);
        return Convert.ToInt64(zz, 2).ToString();
    }

    private static bool VisitWires(List<Wire> wires, Dictionary<string, int> inputs)
    {
        var visited = new HashSet<Wire>();

        while (true)
        {
            if (visited.Count == wires.Count)
                break;

            var found = wires
                .Where(w => !visited.Contains(w) && (inputs.ContainsKey(w.In1) || inputs.ContainsKey(w.In2)))
                .ToList();

            if (found.Count == 0)
                return false;

            foreach (var wire in found)
            {
                if (!inputs.TryGetValue(wire.In1, out var in1) || !inputs.TryGetValue(wire.In2, out var in2))
                    continue;

                if (!inputs.TryAdd(wire.Out, Calculate(in1, in2, wire.Gate)))
                {
                    // throw new Exception("Key shouldn't exist?!");
                    inputs[wire.Out] = Calculate(in1, in2, wire.Gate);
                }

                visited.Add(wire);
            }
        }

        return true;
    }

    [Answer("", Example, Data = "x00: 0{nl}x01: 1{nl}x02: 0{nl}x03: 1{nl}x04: 0{nl}x05: 1{nl}y00: 0{nl}y01: 0{nl}y02: 1{nl}y03: 1{nl}y04: 0{nl}y05: 1{nl}{nl}x00 AND y00 -> z05{nl}x01 AND y01 -> z02{nl}x02 AND y02 -> z01{nl}x03 AND y03 -> z03{nl}x04 AND y04 -> z04{nl}x05 AND y05 -> z00")]
    // [Answer("", Example, Data = "x00: 0{nl}x01: 1{nl}x02: 0{nl}x03: 1{nl}x04: 0{nl}x05: 1{nl}y00: 0{nl}y01: 0{nl}y02: 1{nl}y03: 1{nl}y04: 0{nl}y05: 1{nl}{nl}x00 AND y00 -> z05{nl}x01 AND y01 -> z02{nl}x02 AND y02 -> z01{nl}x03 AND y03 -> z03{nl}x04 AND y04 -> z04{nl}x05 AND y05 -> z00")]
    // [Answer("", Example, Data = "x00: 0{nl}x01: 1{nl}x02: 0{nl}x03: 1{nl}x04: 0{nl}x05: 1{nl}y00: 0{nl}y01: 0{nl}y02: 1{nl}y03: 1{nl}y04: 0{nl}y05: 1{nl}{nl}x00 AND y00 -> z00{nl}x01 AND y01 -> z01{nl}x02 AND y02 -> z03{nl}x03 AND y03 -> z05{nl}x04 AND y04 -> z04{nl}x05 AND y05 -> z02")]
    // [Answer("", Example, Data = "x00: 0{nl}x01: 1{nl}x02: 0{nl}x03: 1{nl}x04: 0{nl}x05: 1{nl}y00: 0{nl}y01: 0{nl}y02: 1{nl}y03: 1{nl}y04: 0{nl}y05: 1{nl}{nl}x00 AND y00 -> z00{nl}x01 AND y01 -> z01{nl}x02 AND y02 -> z02{nl}x03 AND y03 -> z03{nl}x04 AND y04 -> z04{nl}x05 AND y05 -> z05")]
    [Answer("", Regular)]
    public override string SolveB()
    {
        X2();
        return "";

        var (_, wires) = Parse();
        // var visited = new HashSet<Wire>();

        // var cameFrom = new Dictionary<Wire, List<Wire>>();

        // while (true)
        // {
        // if (visited.Count == wires.Count)
        //     break;

        // foreach (var wire in wires.Where(w => w.In1 is "x01" or "y01"))//.Where(w => !visited.Contains(w)))
        // {
        //     
        //     
        //     
        //     // if (!inputs.TryGetValue(wire.In1, out var in1) || !inputs.TryGetValue(wire.In2, out var in2))
        //     //     continue;
        //     //
        //     // if (!inputs.TryAdd(wire.Out, Calculate(in1, in2, wire.Gate)))
        //     //     throw new Exception("Key shouldn't exist?!");
        //
        //     // visited.Add(wire);
        // }
        // }

        var outs = wires; //.Where(w => w.In1 == "x10" || w.In2 == "x10"); //.Take(10);

        foreach (var wire in outs)
        {
            Console.WriteLine($"START Wire {wire.In1}, {wire.Gate}, {wire.In2} => {wire.Out}");

            Tree(wires, wire);

            // X(wires, wire, 0);

            // var ins = wires.Where(w => w.In1 == wire.In1 || w.In2 == wire.In1 || w.In2 == wire.In1 || w.In2 == wire.In2);
            // foreach (var @in in ins)
            // {
            //     Console.WriteLine($"\tWire {@in.In1}, {@in.In2}, {@in.Out} => {@in.Gate}");
            // }
        }

        // var x = visited.Where(w => w.Key.In1 == "x14" || w.Key.In2 == "x14");

        return "";
    }

    private static void X(List<Wire> wires, Wire wire, int depth)
    {
        if (wire.Out.StartsWith('z'))
        {
            Console.WriteLine($"STOP!! Wire {wire.In1}, {wire.Gate}, {wire.In2} => {wire.Out}");
        }

        var ins = wires.Where(w => w.In1 == wire.Out || w.In2 == wire.Out);
        foreach (var @in in ins)
        {
            // Console.WriteLine($"{Tabs(depth)}Wire {@in.In1}, {@in.Gate}, {@in.In2} => {@in.Out}");
            X(wires, @in, depth + 1);
        }
    }

    private static string Tabs(int l)
    {
        var tabs = "\t";
        for (var i = 0; i < l; i++)
        {
            tabs += "\t";
        }

        return tabs;
    }

    // private static List<List<Wire>> ReconstructPaths(Dictionary<Wire, List<Wire>> cameFrom, Wire endNode, bool includeEnd)
    // {
    //     var allPaths = new List<List<Wire>>();
    //
    //     var queue = new Queue<(Wire Node, List<Wire> Path)>();
    //     queue.Enqueue((endNode, []));
    //
    //     while (queue.Count > 0)
    //     {
    //         var (end, path) = queue.Dequeue();
    //
    //         if (!cameFrom.TryGetValue(end, out var nodes))
    //         {
    //             path.Add(end);
    //             path.Reverse();
    //
    //             allPaths.Add(includeEnd ? path : path[..^1]);
    //             continue;
    //         }
    //
    //         foreach (var node in nodes)
    //             queue.Enqueue((node, [.. path, end]));
    //     }
    //
    //     return allPaths;
    // }

    private readonly Dictionary<Wire, List<Wire>> visited2 = new();

    private void Tree(List<Wire> wires, Wire wire)
    {
        var queue = new Queue<Wire>();
        queue.Enqueue(wire);

        // var visited = new Dictionary<Wire, List<Wire>>();

        while (queue.Count > 0)
        {
            wire = queue.Dequeue();

            var ins = wires.Where(w => w.In1 == wire.Out || w.In2 == wire.Out);
            foreach (var @in in ins)
            {
                Console.WriteLine($"Wire {@in.In1}, {@in.Gate}, {@in.In2} => {@in.Out}");
                // X(wires, @in, depth + 1);
                queue.Enqueue(@in);

                if (!visited2.TryAdd(wire, [@in]) && !visited2[wire].Contains(@in))
                    visited2[wire].Add(@in);
            }
        }

        Console.WriteLine();
    }

    private void X2()
    {
        var faulty = new List<string>();
        var faulty2 = new List<(int, int)>();

        var (inputs, wires) = Parse();
        foreach (var input in inputs)
            inputs[input.Key] = 0;

        for (var j = 0; j < 45; j++) // 45
        {
            var round = 0;
            var visitedCombos = new HashSet<(int, int)>();

            while (true)
            {
                (_, wires) = Parse();

                var testX = $"x{j.ToString().PadLeft(2, '0')}";
                var testY = $"y{j.ToString().PadLeft(2, '0')}";

                foreach (var (from, to) in faulty2)
                {
                    var tmp = wires[from].Out;
                    wires[from] = wires[from] with { Out = wires[to].Out };
                    wires[to] = wires[to] with { Out = tmp };
                }

                foreach (var input in inputs)
                {
                    if (input.Key == testX || input.Key == testY)
                        inputs[input.Key] = 1;
                }

                // inputs = inputs.Where(w => !w.Key.StartsWith('z')).ToDictionary(); // Skip all non-original

                if (round > 0)
                {
                    var from = 0;
                    var to = 0;

                    do
                    {
                        from = RandomNumberGenerator.GetInt32(wires.Count);
                        to = RandomNumberGenerator.GetInt32(wires.Count);

                        // if ((from == 0 && to == 5) || (from == 5 && to == 0))
                        //     Console.WriteLine();
                    } while (visitedCombos.Contains((from, to)));

                    visitedCombos.Add((from, to));

                    var tmp = wires[from].Out;
                    wires[from] = wires[from] with { Out = wires[to].Out };
                    wires[to] = wires[to] with { Out = tmp };
                }

                VisitWires(wires, inputs);

                // var visited = new HashSet<Wire>();
                //
                // var ij = 0;
                // while (true)
                // {
                //     ij++;
                //     if (ij > 5_000)
                //         break;
                //
                //     if (visited.Count == wires.Count)
                //         break;
                //
                //     // var found = 
                //
                //     foreach (var wire in wires.Where(w => !visited.Contains(w)))
                //     {
                //         if (!inputs.TryGetValue(wire.In1, out var in1) || !inputs.TryGetValue(wire.In2, out var in2))
                //             continue;
                //
                //         if (!inputs.TryAdd(wire.Out, Calculate(in1, in2, wire.Gate)))
                //             throw new Exception("Key shouldn't exist?!");
                //
                //         visited.Add(wire);
                //     }
                // }

                var x = inputs
                    .Where(i => i.Key.StartsWith('x'))
                    .OrderByDescending(i => i.Key)
                    .Select(x => x.Value);

                var xx = string.Join("", x);
                var xxx = Convert.ToInt64(xx, 2).ToString();

                var y = inputs
                    .Where(i => i.Key.StartsWith('y'))
                    .OrderByDescending(i => i.Key)
                    .Select(y => y.Value);

                var yy = string.Join("", y);
                var yyy = Convert.ToInt64(yy, 2).ToString();

                var z = inputs
                    .Where(i => i.Key.StartsWith('z'))
                    .OrderByDescending(i => i.Key)
                    .Select(z => z.Value);

                var zz = string.Join("", z);
                var zzz = Convert.ToInt64(zz, 2).ToString();

                if (xxx != zzz)
                {
                    // Console.WriteLine($"T:{testX}");
                    // Console.WriteLine($"X:{xx}, {xxx}");
                    // Console.WriteLine($"Y:{yy}, {yyy}");
                    // Console.WriteLine($"Z:{zz}, {zzz}");

                    round++;
                }
                else
                {
                    if (round > 0)
                    {
                        var vc = visitedCombos.Last();
                        Console.WriteLine($"{wires[vc.Item1].Out}, {wires[vc.Item2].Out}");
                        faulty.Add(wires[vc.Item1].Out);
                        faulty.Add(wires[vc.Item2].Out);

                        faulty2.Add((vc.Item1, vc.Item2));
                    }

                    break;
                }
            }
        }

        Console.WriteLine(string.Join(',', faulty.Order()));
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
