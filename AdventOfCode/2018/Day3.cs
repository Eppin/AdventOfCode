namespace AdventOfCode._2018;

public partial class Day3 : Day
{
    private List<int>?[,] _grid = null!;

    public Day3() : base()
    {
    }

    [Answer("116140", Regular)]
    public override string SolveA()
    {
        var splittedInput = SplitInput
            .Select(x => new Claim(x))
            .ToList();

        var maxX = splittedInput.Max(c => c.X + c.Width);
        var maxY = splittedInput.Max(c => c.Y + c.Height);

        _grid = new List<int>[maxX, maxY];

        var overlappingSquare = 0;
        foreach (var claim in splittedInput)
        {
            for (var x = 0; x < claim.Width; x++)
            {
                for (var y = 0; y < claim.Height; y++)
                {
                    _grid[claim.X + x, claim.Y + y] ??= [];

                    if (_grid[claim.X + x, claim.Y + y]?.Contains(claim.Id) == false)
                    {
                        _grid[claim.X + x, claim.Y + y]?.Add(claim.Id);
                    }

                    if (_grid[claim.X + x, claim.Y + y]?.Count == 2)
                    {
                        overlappingSquare++;
                    }
                }
            }
        }

        return $"{overlappingSquare}";
    }

    [Answer("574", Regular)]
    public override string SolveB()
    {
        // This generates a list with overlapping Ids and the grid
        SolveA();

        var allIds = new HashSet<int>();
        var overlappingIds = new HashSet<int>();

        foreach (var item in _grid)
        {
            if (item is { Count: >= 2 })
                overlappingIds.UnionWith(item);

            allIds.UnionWith(item ?? Enumerable.Empty<int>());
        }

        return string.Join(", ", allIds.Except(overlappingIds));
    }

    private partial class Claim
    {
        public int Id { get; }
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        public Claim(string line)
        {
            var regex = ParserRegex();
            var match = regex.Match(line);

            Id = int.Parse(match.Groups[1].Value);
            X = int.Parse(match.Groups[2].Value);
            Y = int.Parse(match.Groups[3].Value);
            Width = int.Parse(match.Groups[4].Value);
            Height = int.Parse(match.Groups[5].Value);
        }

        [GeneratedRegex(@"\#(\d+) \@ (\d+),(\d+): (\d+)x(\d+)")]
        private static partial Regex ParserRegex();
    }
}
