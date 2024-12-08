namespace AdventOfCode._2024;

using System.Drawing;

public class Day8 : Day
{
    public Day8() : base()
    {
    }

    [Answer("14", Example, Data = "............{nl}........0...{nl}.....0......{nl}.......0....{nl}....0.......{nl}......A.....{nl}............{nl}............{nl}........A...{nl}.........A..{nl}............{nl}............{nl}")]
    [Answer("379", Regular)]
    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    [Answer("9", Example, Data = "T.........{nl}...T......{nl}.T........{nl}..........{nl}..........{nl}..........{nl}..........{nl}..........{nl}..........{nl}..........")]
    [Answer("1339", Regular)]
    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool isPartB)
    {
        var input = Parse();

        var coordinates = new List<Coordinate>();

        // Determine coordinates of antennas
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                var c = input[y][x];
                if (c != '.')
                    coordinates.Add(new(new Point(x, y), c));
            }
        }

        // Determine anti-nodes
        var result = new List<Coordinate>();

        var groups = coordinates
            .GroupBy(c => c.Char)
            .Select(g => new { g.Key, Coordinates = g.Select(c => c.Point) });

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
            .DistinctBy(c => c.Point)
            .Count(r => r.Point.Y < input.Length && r.Point.X < input[0].Length && r.Point is { Y: >= 0, X: >= 0 });
    }

    private static IEnumerable<Coordinate> AntiNodes(char key, Point coordinate1, Point coordinate2, int maxX, int maxY, bool isPartB)
    {
        var diffX = coordinate2.X - coordinate1.X;
        var diffY = coordinate2.Y - coordinate1.Y;

        var isPlus = true;
        var point = new Point(coordinate1.X + diffX, coordinate1.Y + diffY);

        if (point == coordinate2)
        {
            point = new Point(coordinate1.X - diffX, coordinate1.Y - diffY);
            isPlus = false;
        }

        yield return new Coordinate(point, key);

        if (!isPartB)
            yield break;

        yield return new Coordinate(coordinate1, key);

        while (point.Y < maxY && point.X < maxX && point.Y >= 0 && point.X >= 0)
        {
            point = isPlus
                ? new Point(point.X + diffX, point.Y + diffY)
                : new Point(point.X - diffX, point.Y - diffY);

            yield return new Coordinate(point, key);
        }
    }

    private char[][] Parse()
    {
        return GetSplitInput()
            .Select(l => l.ToCharArray())
            .ToArray();
    }

    private record struct Coordinate(Point Point, char Char);
}
