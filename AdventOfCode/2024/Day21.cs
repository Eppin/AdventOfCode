namespace AdventOfCode._2024;

using Coordinate = Coordinate<int>;

public class Day21 : Day
{
    public Day21() : base()
    {
    }

    [Answer("126384", Example, Data = "029A{nl}980A{nl}179A{nl}456A{nl}379A")]
    [Answer("157908", Regular)]
    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    [Answer("196910339808654", Regular)]
    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    // Keep a list of current positions of the robots
    // hovering a keypad
    private static readonly Dictionary<int, char> Start = [];

    // Cache the 'from' and 'to' char and avoid very long recursive loops
    // instead, return cached value when a certain 'from' and 'to' is detected
    private static readonly Dictionary<(char From, char To, int Depth), long> FromToCache = new();

    private long Solve(bool isPartB)
    {
        // Always clear...
        // otherwise running Part A and B one after the other will result in wrong answers
        FromToCache.Clear();

        var codes = Parse();
        var numpad = NumpadCombinations();

        var total = 0L;
        var depth = 2;
        if (isPartB) depth += 23;

        foreach (var code in codes)
        {
            var start = 10; // A
            var length = 0L;

            foreach (var to in code)
            {
                var next = numpad[start][to];
                var shortest = long.MaxValue;

                foreach (var direction in next)
                {
                    Start.Clear();
                    shortest = Math.Min(shortest, Depth(direction, depth, 0));
                }

                length += shortest;
                start = to;
            }

            var skipA = string.Join(string.Empty, code.SkipLast(1));
            var codeAsInt = int.Parse(skipA);

            total += length * codeAsInt;
        }

        return total;
    }

    private static long Depth(List<char> next, int depth, int currentDepth)
    {
        var list = 0L;

        var dirpad = DirpadCombinations();
        var start = Start.GetValueOrDefault(currentDepth, 'A');

        if (depth == currentDepth)
            return next.Count;

        foreach (var direction in next)
        {
            if (FromToCache.TryGetValue((start, direction, currentDepth), out var cacheValue))
                list += cacheValue;
            else
            {
                var possibilities = dirpad[start][Convert(direction)];

                var shortest = long.MaxValue;
                foreach (var possible in possibilities)
                    shortest = Math.Min(shortest, Depth(possible, depth, currentDepth + 1));

                FromToCache.TryAdd((start, direction, currentDepth), shortest);

                list += shortest;
            }

            start = direction;

            if (!Start.TryAdd(currentDepth, direction))
                Start[currentDepth] = direction;
        }

        return list;
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
                queue.Enqueue((coordinate, [.. path, Convert(direction)], [.. visited, node]));
        }

        return paths;
    }

    private static Dictionary<int, List<List<List<char>>>> NumpadCombinations()
    {
        var result = new Dictionary<int, List<List<List<char>>>>();

        var (grid, numpad) = Numpad();

        foreach (var (key1, coordinate1) in numpad)
        {
            var key = ConvertNumpad(key1);
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
                    .Select(c => c.Count > 0 ? [.. c, 'A'] : c)
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
        var result = new Dictionary<char, List<List<List<char>>>>();

        var (grid, numpad) = Dirpad();

        foreach (var (key1, coordinate1) in numpad)
        {
            result.Add(key1, []);

            foreach (var (key2, coordinate2) in numpad)
            {
                if (key1 == key2)
                {
                    result[key1].Add([['A']]);
                    continue;
                }

                var paths = GetPath(grid, coordinate1, coordinate2)
                    .GroupBy(p => p.Count)
                    .OrderBy(g => g.Key)
                    .First()
                    .Select(c => { c.Add('A'); return c; })
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

    private static char Convert(Direction direction)
    {
        return direction switch
        {
            Direction.North => '^',
            Direction.East => '>',
            Direction.South => 'v',
            Direction.West => '<',
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private static int ConvertNumpad(char c)
    {
        if (c is 'A') return 10;
        return c - '0';
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
