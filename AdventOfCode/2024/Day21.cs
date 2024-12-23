namespace AdventOfCode._2024;

using Coordinate = Coordinate<int>;

public class Day21 : Day
{
    public Day21() : base()
    {
    }

    [Answer("126384", Example, Data = "029A{nl}980A{nl}179A{nl}456A{nl}379A")]
    [Answer("", Regular)]
    public override string SolveA()
    {
        //var numpad = new List<List<List<string>>>(11)
        //{
        //    new() { new List<string> {"a", "b"}, new List<string> {"B", "A"} }
        //};

        //var k = numpad[0][0];
        //var j = numpad[0][1];

        int t = 0;


        var grid = Numpad();

        for (var a = 0; a < 11; a++)
        {
            for (var b = 0; b < 11; b++)
            {
                var c_a = Convert(a);
                var c_b = Convert(b);

                Console.WriteLine($"F:{a} ({c_a}) -> T:{b} ({c_b})");
                Dijkstra(grid, c_a, c_b);
            }
        }

        return Solve(2).ToString();
    }

    private void Dijkstra(Grid<char> grid, char startC, char endC)
    {
        //
        var start = new Coordinate();
        var end = new Coordinate();

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                if (grid[x, y] == startC)
                    start = new Coordinate(x, y);

                if (grid[x, y] == endC)
                    end = new Coordinate(x, y);
            }
        }
        //

        var dijkstra = new Dijkstra<(Coordinate Coordinate, char Char)>();

        //var start = new Coordinate(0, 0);
        //var end = new Coordinate(70, 70); // Example uses 6,6

        dijkstra.GetNeighbours = reindeer => grid.Neighbours(reindeer.Coordinate.X, reindeer.Coordinate.Y).Where(n => grid[n.X, n.Y] is not '.').Select(n => (new Coordinate(n.X, n.Y), grid[n.X, n.Y]));
        dijkstra.EndReached = current => current.Coordinate == end;
        dijkstra.Draw = list =>
        {
            for (var y = 0; y < grid.MaxY; y++)
            {
                for (var x = 0; x < grid.MaxX; x++)
                {
                    var c = list.Count(l => l.Coordinate == new Coordinate(x, y));
                    if (c > 0)
                    {
                        Console.Write(c);
                    }
                    else
                        Console.Write(grid[x, y]);
                }

                Console.WriteLine();
            }
        };

        var paths = dijkstra.ShortestPaths((start, grid[start.X, start.Y]), true);
        Console.WriteLine();
    }

    private static Grid<char> Numpad()
    {
        var array = new char[4][];
        array[0] = ['7', '8', '9'];
        array[1] = ['4', '5', '6'];
        array[2] = ['1', '2', '3'];
        array[3] = ['.', '0', 'A'];

        return new Grid<char>(array);
    }

    private static Grid<char> Dirpad()
    {
        var array = new char[2][];
        array[0] = ['.', '^', 'A'];
        array[1] = ['<', 'v', '>'];

        return new Grid<char>(array);
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
            sum += (length * test3);
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
        for (var i = 0; i < depth; i++)
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

    private static char Convert(int i)
    {
        return i switch
        {
            <= 9 => (char)(i + '0'),
            10 => 'A',
            _ => throw new ArgumentOutOfRangeException(nameof(i), i, null)
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
