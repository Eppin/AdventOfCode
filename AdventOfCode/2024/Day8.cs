namespace AdventOfCode._2024;

public class Day8 : Day
{
    public Day8() : base()
    {
    }

    [Answer("14", Example, Data = "............{nl}........0...{nl}.....0......{nl}.......0....{nl}....0.......{nl}......A.....{nl}............{nl}............{nl}........A...{nl}.........A..{nl}............{nl}............{nl}")]
    [Answer("379", Regular)]
    public override object SolveA()
    {
        return Solve(false);
    }

    [Answer("9", Example, Data = "T.........{nl}...T......{nl}.T........{nl}..........{nl}..........{nl}..........{nl}..........{nl}..........{nl}..........{nl}..........")]
    [Answer("1339", Regular)]
    public override object SolveB()
    {
        return Solve(true);
    }

    private int Solve(bool isPartB)
    {
        var input = Parse();

        var coordinates = new List<CoordinateChar>();

        // Determine coordinates of antennas
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                var c = input[y][x];
                if (c != '.')
                    coordinates.Add(new(new Coordinate(x, y), c));
            }
        }

        // Determine anti-nodes
        var result = new List<CoordinateChar>();

        var groups = coordinates
            .GroupBy(c => c.Char)
            .Select(g => new { g.Key, Coordinates = g.Select(c => c.Coordinate) });

        foreach (var group in groups)
        {
            foreach (var coordinate1 in group.Coordinates)
            {
                foreach (var coordinate2 in group.Coordinates.Where(c => c != coordinate1))
                {
                    result.AddRange(AntiNodes(group.Key, coordinate1, coordinate2, input[0].Length, input.Length, isPartB));
                }
            }
        }

        return result
            .DistinctBy(c => c.Coordinate)
            .Count(r => r.Coordinate.Y < input.Length && r.Coordinate.X < input[0].Length && r.Coordinate is { Y: >= 0, X: >= 0 });
    }

    private static IEnumerable<CoordinateChar> AntiNodes(char key, Coordinate coordinate1, Coordinate coordinate2, int maxX, int maxY, bool isPartB)
    {
        var diffX = coordinate2.X - coordinate1.X;
        var diffY = coordinate2.Y - coordinate1.Y;

        var isPlus = true;
        var coordinate = new Coordinate(coordinate1.X + diffX, coordinate1.Y + diffY);

        if (coordinate == coordinate2)
        {
            coordinate = new Coordinate(coordinate1.X - diffX, coordinate1.Y - diffY);
            isPlus = false;
        }

        yield return new CoordinateChar(coordinate, key);

        if (!isPartB)
            yield break;

        yield return new CoordinateChar(coordinate1, key);

        while (coordinate.Y < maxY && coordinate.X < maxX && coordinate.Y >= 0 && coordinate.X >= 0)
        {
            coordinate = isPlus
                ? new Coordinate(coordinate.X + diffX, coordinate.Y + diffY)
                : new Coordinate(coordinate.X - diffX, coordinate.Y - diffY);

            yield return new CoordinateChar(coordinate, key);
        }
    }

    private char[][] Parse()
    {
        return GetSplitInput()
            .Select(l => l.ToCharArray())
            .ToArray();
    }

    private record struct CoordinateChar(Coordinate Coordinate, char Char);
}
