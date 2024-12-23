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
        var numpad = NumpadCombinations();

        // van 0 naar 8
        var a1 = numpad[0][9];
        // var b1 = dirpad['A'][Convert('<')];

        var codes = Parse();

        foreach (var code in codes.Skip(3).Take(1))
        {
            Console.WriteLine($"Code:{string.Join(string.Empty, code)}");

            var start = 10; // A

            // X.Clear();

            foreach (var c in code)
            {
                var next = numpad[start][c];
                Console.WriteLine($"{c}: fr-{start}, to-{c}, n:{string.Join(", ", next.Select(x => string.Join('-', x)))}");

                foreach (var direction in next)
                {
                    Depth3(direction, 2, 1);
                }

                // length += Depth2(next, depth, 0);
                start = c;
            }
        }

        return "";
    }

    private static void Depth3(List<char> next, int depth, int currentDepth)
    {
        var dirpad = DirpadCombinations();
        var start = 'A';

        if (depth == currentDepth)
            return;

        foreach (var direction in next)
        {
            var possibilities = dirpad[start][Convert(direction)];
            Console.WriteLine($"{Tabs(currentDepth)}({currentDepth}), fr-{start}, to-{direction}, n:{string.Join(", ", possibilities.Select(x => string.Join('-', x)))}");

            foreach (var possible in possibilities)
            {
                Depth3(possible, depth, currentDepth + 1);
            }

            start = direction;
        }
    }

    private record Robot(List<char> Directions, char Start, int Depth, int CurrentDepth);

    private static void Depth2(List<char> next, int depth)
    {
        var dirpad = DirpadCombinations();

        var queue = new Queue<Robot>();
        queue.Enqueue(new Robot(next, 'A', depth, 1));

        while (queue.Count > 0)
        {
            var robot = queue.Dequeue();

            foreach (var directions in robot.Directions)
            {
                var k = dirpad['A'][Convert(directions)];

                // queue.Enqueue(new Robot(dirpad['A'][directions], robot.Start, depth + 1, depth + 1));

                // Multiple possible directions
                // foreach (var direction in directions)
                // {
                //     queue.Enqueue(new Robot(direction, direction, robot.Depth + 1, depth));
                // }
            }
        }

        // var start = 1; //_start.GetValueOrDefault(currentDepth, 1);
        //
        // // var start = 1;
        // var length = 0L;
        //
        // foreach (var d in code)
        // {
        //     var dc = Convert(d);
        //     var next2 = Direction[start][dc];
        //     // Console.WriteLine($"{Tabs(currentDepth)}({currentDepth}), fr-{start}, to-{dc} ({d}), n:{next2}");
        //
        //     if (currentDepth < depth - 1)
        //         length += Depth(next2, depth, currentDepth + 1);
        //     else
        //     {
        //         // X.Add(next2.Length);
        //         length += next2.Length;
        //     }
        //
        //     // _start[currentDepth] = dc;
        //     start = dc;
        //     // Console.WriteLine($"End {currentDepth}, S:{start}");
        // }
        //
        // return length;
    }

    private static List<List<char>> GetPath(Grid<char> grid, Coordinate start, Coordinate end)
    {
        var paths = new List<List<char>>();

        var queue = new Queue<(Coordinate Node, List<char> Path, HashSet<Coordinate> Visited)>();
        queue.Enqueue((start, [], []));

        while (queue.Count > 0)
        {
            var (node, path, visited) = queue.Dequeue();

            if (node == end)
            {
                paths.Add(path);
                continue;
            }

            var directions = grid
                .Directions(node)
                .Where(d => grid[d.Value] is not '.' && !visited.Contains(d.Value));

            foreach (var (direction, coordinate) in directions)
                queue.Enqueue((coordinate, [..path, Convert(direction)], [..visited, node]));
        }

        return paths;
    }

    private static Dictionary<int, List<List<List<char>>>> NumpadCombinations()
    {
        // TODO
        // Should char be int?!
        // From int to int (with list of possibilities)
        var result = new Dictionary<int, List<List<List<char>>>>();

        var (grid, numpad) = Numpad();

        foreach (var (key1, coordinate1) in numpad)
        {
            var key = Convert2(key1);
            result.Add(key, []);

            foreach (var (key2, coordinate2) in numpad)
            {
                if (key1 == key2)
                {
                    result[key].Add([]);
                    continue;
                }

                var paths = GetPath(grid, coordinate1, coordinate2)
                    .GroupBy(p => p.Count)
                    .OrderBy(g => g.Key)
                    .First()
                    .ToList();

                result[key].Add(paths);
            }
        }

        return result;
    }

    private static (Grid<char>, Dictionary<char, Coordinate>) Numpad()
    {
        var array = new char[4][];
        array[0] = ['7', '8', '9'];
        array[1] = ['4', '5', '6'];
        array[2] = ['1', '2', '3'];
        array[3] = ['.', '0', 'A'];

        var grid = new Grid<char>(array);
        var coordinates = new Dictionary<char, Coordinate>();

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                if (grid[x, y] is not '.')
                    coordinates.Add(grid[x, y], new Coordinate(x, y));
            }
        }

        var ordered = coordinates
            .OrderBy(c => c.Key)
            .ToDictionary();

        return (grid, ordered);
    }

    private static Dictionary<char, List<List<List<char>>>> DirpadCombinations()
    {
        // TODO
        // Should char be int?!
        // From int to int (with list of possibilities)
        var result = new Dictionary<char, List<List<List<char>>>>();

        var (grid, numpad) = Dirpad();

        foreach (var (key1, coordinate1) in numpad)
        {
            result.Add(key1, []);

            foreach (var (key2, coordinate2) in numpad)
            {
                if (key1 == key2)
                {
                    result[key1].Add([]);
                    continue;
                }

                var paths = GetPath(grid, coordinate1, coordinate2)
                    .GroupBy(p => p.Count)
                    .OrderBy(g => g.Key)
                    .First()
                    .ToList();

                result[key1].Add(paths);
            }
        }

        return result;
    }

    private static (Grid<char>, Dictionary<char, Coordinate>) Dirpad()
    {
        var array = new char[2][];
        array[0] = ['.', '^', 'A'];
        array[1] = ['<', 'v', '>'];

        var grid = new Grid<char>(array);
        var coordinates = new Dictionary<char, Coordinate>();

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
            {
                if (grid[x, y] is not '.')
                    coordinates.Add(grid[x, y], new Coordinate(x, y));
            }
        }

        var ordered = coordinates
            .OrderBy(c => c.Key)
            .ToDictionary();

        return (grid, ordered);
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private long Solve(int depth)
    {
        var codes = Parse();

        var sum = 0L;

        foreach (var code in codes) //.TakeLast(1))
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
            Console.WriteLine($"L: {length} * {test3}"); // vs {X.Sum()}, {X.Count}");
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
            '<' => 0,
            '>' => 1,
            'A' => 2,
            '^' => 3,
            'v' => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }

    private static int Convert2(char c)
    {
        if (c is 'A') return 10;
        return c - '0';
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

    private static char Convert(Direction direction)
    {
        return direction switch
        {
            Models.Direction.North => '^',
            Models.Direction.East => '<',
            Models.Direction.South => 'v',
            Models.Direction.West => '>',
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
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
