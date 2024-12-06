namespace AdventOfCode._2021;

public partial class Day5 : Day
{
    public Day5() : base()
    {
    }

    [Answer("8111", Regular)]
    public override string SolveA()
    {
        var coordinates = GetCoordinates().ToArray();

        // 1000 should be calculated...
        var grid = CreateGrid(1000).ToArray();

        foreach (var coordinate in coordinates)
        {
            if (!coordinate.HorizontalVertical)
                continue;

            NewMethod(coordinate, grid);
        }

        return CountPoints(grid);
    }

    [Answer("22088", Regular)]
    public override string SolveB()
    {
        var coordinates = GetCoordinates().ToArray();

        // 1000 should be calculated...
        var grid = CreateGrid(1000).ToArray();

        foreach (var coordinate in coordinates)
        {
            if (coordinate is { HorizontalVertical: false, Diagonal: false })
                continue;

            NewMethod(coordinate, grid);
        }

        return CountPoints(grid);
    }

    private static void NewMethod(Coordinates coordinate, IReadOnlyList<int[]> grid)
    {
        var increaseX = coordinate.DiffX >= 0;
        var increaseY = coordinate.DiffY >= 0;

        var listCoords = new List<(int, int)>();

        if (coordinate.HorizontalVertical)
        {
            if (increaseX)
            {
                for (var i = 0; i <= Math.Abs(coordinate.DiffX); i++)
                    listCoords.Add((coordinate.X1 + i, coordinate.Y1));
            }
            else
            {
                for (var i = Math.Abs(coordinate.DiffX); i >= 0; i--)
                    listCoords.Add((coordinate.X1 - i, coordinate.Y1));
            }

            if (increaseY)
            {
                for (var i = 0; i <= Math.Abs(coordinate.DiffY); i++)
                    listCoords.Add((coordinate.X1, coordinate.Y1 + i));
            }
            else
            {
                for (var i = Math.Abs(coordinate.DiffY); i >= 0; i--)
                    listCoords.Add((coordinate.X1, coordinate.Y1 - i));
            }
        }
        else
        {
            if (increaseX && increaseY) // 1,1
            {
                for (var i = 0; i <= Math.Abs(coordinate.DiffX); i++)
                    listCoords.Add((coordinate.X1 + i, coordinate.Y1 + i));
            }
            else if (increaseX && !increaseY) // 1,-1
            {
                for (var i = Math.Abs(coordinate.DiffX); i >= 0; i--)
                    listCoords.Add((coordinate.X1 + i, coordinate.Y1 - i));
            }

            if (!increaseX && increaseY) // -1, 1
            {
                for (var i = 0; i <= Math.Abs(coordinate.DiffY); i++)
                    listCoords.Add((coordinate.X1 - i, coordinate.Y1 + i));
            }
            else if (!increaseX && !increaseY) // -1,-1
            {
                for (var i = Math.Abs(coordinate.DiffY); i >= 0; i--)
                    listCoords.Add((coordinate.X1 - i, coordinate.Y1 - i));
            }
        }

        foreach (var (x, y) in listCoords.Distinct())
            grid[y][x] += 1;
    }

    private static string CountPoints(IReadOnlyCollection<int[]> grid)
    {
        var count = 0;

        foreach (var t in grid)
        {
            for (var j = 0; j < grid.Count; j++)
            {
                count += t[j] >= 2
                    ? 1
                    : 0;
            }
        }

        return $"{count}";
    }

    private static IEnumerable<int[]> CreateGrid(int size = 11)
    {
        for (var i = 0; i < size; i++)
            yield return new int[size];
    }

    private IEnumerable<Coordinates> GetCoordinates()
    {
        const string regex = @"(?'x1'\d+),(?'y1'\d+).->.(?'x2'\d+),(?'y2'\d+)";

        foreach (var s in GetSplitInput())
        {
            var m = Regex().Match(s);

            if (!m.Success)
                throw new DataException();

            var x1 = int.Parse(m.Groups["x1"].Value);
            var y1 = int.Parse(m.Groups["y1"].Value);
            var x2 = int.Parse(m.Groups["x2"].Value);
            var y2 = int.Parse(m.Groups["y2"].Value);

            yield return new Coordinates(x1, y1, x2, y2);
        }
    }

    private class Coordinates(int x1, int y1, int x2, int y2)
    {
        public int X1 { get; } = x1;

        public int Y1 { get; } = y1;

        public int X2 { get; } = x2;

        public int Y2 { get; } = y2;

        public int DiffX => X2 - X1;

        public int DiffY => Y2 - Y1;

        public bool HorizontalVertical => DiffX == 0 || DiffY == 0;

        public bool Diagonal => Math.Abs(DiffX) == Math.Abs(DiffY);

        public override string ToString() => $"{X1},{Y1} => {X2},{Y2}";
    }

    [GeneratedRegex(@"(?'x1'\d+),(?'y1'\d+).->.(?'x2'\d+),(?'y2'\d+)")]
    private static partial Regex Regex();
}
