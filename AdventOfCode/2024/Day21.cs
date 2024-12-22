namespace AdventOfCode._2024;

public class Day21 : Day
{
    public Day21() : base()
    {
    }

    [Answer("126384", Example, Data = "029A{nl}980A{nl}179A{nl}456A{nl}379A")]
    [Answer("", Regular)]
    public override string SolveA()
    {
        return Solve(2).ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private long Solve(int depth)
    {
        var codes = Parse();

        var sum = 0L;

        foreach (var code in codes)//.TakeLast(1))
        {
            Console.WriteLine($"Code:{string.Join(string.Empty, code)}");

            var length = 0L;
            var start = 10; // A
            
            // X.Clear();

            foreach (var c in code)
            {
                var next = Numeric[start][c];
                // Console.WriteLine($"{c}: fr-{start}, to-{c}, n:{next}");

                length += Depth(next, depth, 0);
                start = c;
            }

            var test = code.Where(c => c != 10);
            var test1 = string.Join("", test);
            var test3 = int.Parse(test1);
            sum+= (length * test3);
            Console.WriteLine($"L: {length} * {test3}");// vs {X.Sum()}, {X.Count}");
        }
        
        return sum;
    }

    // private static readonly Dictionary<int, int> _start = [];
        
    private static long Depth(string code, int depth, int currentDepth)
    {
        var start = 1; //_start.GetValueOrDefault(currentDepth, 1);
        
        // var start = 1;
        var length = 0L;

        foreach (var d in code)
        {
            var dc = Convert(d);
            var next2 = Direction[start][dc];
            // Console.WriteLine($"{Tabs(currentDepth)}({currentDepth}), fr-{start}, to-{dc} ({d}), n:{next2}");

            if (currentDepth < depth - 1)
                length += Depth(next2, depth, currentDepth + 1);
            else
            {
                // X.Add(next2.Length);
                length += next2.Length;
            }

            // _start[currentDepth] = dc;
            start = dc;
            // Console.WriteLine($"End {currentDepth}, S:{start}");
        }

        return length;
    }

    private static string Tabs(int depth)
    {
        var str = "\t";
        for (int i = 0; i < depth; i++)
        {
            str += "\t";
        }
        
        return str;
    }

    private static string[][] Numeric
    {
        get
        {
            var numeric = new string[11][]; // 0-9 + A (is 10)
            numeric[0] = ["", "^<A", "^A", ">^A", "^^<A", "^^A", ">^^A", "^^^<A", "^^^A", ">^^^A", ">A"]; // from 0 to ...
            numeric[1] = [">vA", "", ">A", ">>A", "^A", ">^A", ">>^A", "^^A", ">^^A", ">>^^A", ">>vA"]; // from 1 to ...
            numeric[2] = ["vA", "<A", "", ">A", "<^A", "^A", ">^A", "<^^A", "^^A", ">^^A", ">vA"]; // from 2 to ...
            numeric[3] = ["<vA", "<<A", "<A", "", "<<^A", "<^A", "^A", "<<^^A", "<^^A", "^^A", "vA"]; // from 3 to ...
            numeric[4] = [">vvA", "vA", ">vA", ">>vA", "", ">A", ">>A", "^A", ">^A", ">>^A", ">>vvA"]; // from 4 to ...
            numeric[5] = ["vvA", "<vA", "vA", ">vA", "<A", "", ">A", "<^A", "^A", ">^A", ">vvA"]; // from 5 to ...
            numeric[6] = ["<vvA", "<<vA", "<vA", "vA", "<<A", "<A", "", "<<^A", "<^A", "^A", "vvA"]; // from 6 to ...
            numeric[7] = [">vvvA", "vvA", ">vvA", ">>vvA", "vA", ">vA", ">>vA", "", ">A", ">>A", ">>vvvA"]; // from 7 to ...
            numeric[8] = ["vvvA", "<vvA", "vvA", ">vvA", "<vA", "vA", ">vA", "<A", "", ">A", ">vvvA"]; // from 8 to ...
            numeric[9] = ["<vvvA", "<<vvA", "<vvA", "vvA", "<<vA", "<vA", "vA", "<<A", "<A", "", "vvvA"]; // from 9 to ...
            numeric[0xA] = ["<A", "^<<A", "<^A", "^A", "^^<<A", "<^^A", "^^A", "^^^<<A", "<^^^A", "^^^A", ""]; // from A to ...

            return numeric;
        }
    }

    private static string[][] Direction
    {
        get
        {
            // +---+---+---+  +---+---+---+
            // |   | ^ | A |  |   | 0 | 1 |
            // | < | v | > |  | 2 | 3 | 4 |
            // +---+---+---+  +---+---+---+
            var direction = new string[5][];
            direction[0] = ["A", ">A", "v<A", "vA", ">vA"]; // from '^' to ...
            direction[1] = ["<A", "A", "v<<A", "v<A", "vA"]; // from A to ...
            direction[2] = [">^A", ">>^A", "A", ">A", ">>A"]; // from  '<' to ...
            direction[3] = ["^A", ">^A", "<A", "A", ">A"]; // from  'v' to ...
            direction[4] = ["<^A", "^A", "<<A", "<A", "A"]; // from  '>' to ...

            return direction;
        }
    }

    private static int Convert(char c)
    {
        return c switch
        {
            '^' => 0,
            'A' => 1,
            '<' => 2,
            'v' => 3,
            '>' => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }

    private int[][] Parse()
    {
        return GetSplitInput()
            .Select(s => s
                .Select(c => int.Parse($"{(c == 'A' ? "10" : $"{c}")}"))
                .ToArray()
            ).ToArray();
    }
}
