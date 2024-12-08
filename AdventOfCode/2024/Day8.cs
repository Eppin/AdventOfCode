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
        return Solve().ToString();
    }

    [Answer("9", Example, Data = "T.........{nl}...T......{nl}.T........{nl}..........{nl}..........{nl}..........{nl}..........{nl}..........{nl}..........{nl}..........")]
    [Answer("", Regular)]
    public override string SolveB()
    {
        return Solve().ToString();
    }

    private int Solve()
    {
        var input = Parse();

        var coordinates = new List<Coordinate>();

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                var c = input[y][x];
                if (c != '.')
                    coordinates.Add(new(new Point(x, y), c));
            }
        }

        //
        var groups = coordinates
            .GroupBy(c => c.Char)
            .Select(g => new { g.Key, Coordinates = g.Select(c => c.Point) });

        var result = new List<Coordinate>();

        foreach (var group in groups)
        {
            foreach (var c1 in group.Coordinates)
            {
                foreach (var c2 in group.Coordinates.Where(c => c != c1))
                {
                    var diffX = c2.X - c1.X;
                    var diffY = c2.Y - c1.Y;

                    var plus1 = true;
                    var point1 = new Point(c1.X + diffX, c1.Y + diffY);

                    if (point1 == c2)
                    {
                        point1 = new Point(c1.X - diffX, c1.Y - diffY);
                        plus1 = false;
                    }

                    result.Add(new Coordinate(c1, group.Key));
                    result.Add(new Coordinate(point1, group.Key));

                    while (point1.Y < input.Length && point1.X < input[0].Length && point1.Y >= 0 && point1.X >= 0)
                    {
                        if (plus1)
                        {
                            point1 = new Point(point1.X + diffX, point1.Y + diffY);
                        }
                        else
                            point1 = new Point(point1.X - diffX, point1.Y - diffY);

                        result.Add(new Coordinate(point1, group.Key));
                    }

                    ////


                    var point2 = new Point(c2.X + diffX, c2.Y + diffY);
                    if (point2 == c1)
                        point2 = new Point(c2.X - diffX, c2.Y - diffY);


                    result.Add(new Coordinate(point2, group.Key));

                    Console.WriteLine($"{group.Key}: {c1.X},{c1.Y} vs {c2.X},{c2.Y} = {point1.X},{point1.Y} / {point2.X},{point2.Y}");
                }
            }
        }

        // Draw initial
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                var c = input[y][x];
                Console.Write(c);
            }
        
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine();

        // Draw new
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                var c = input[y][x];
                if (result.Any(r => r.Point == new Point(x, y)))
                    c = '#';
        
                Console.Write(c);
            }
        
            Console.WriteLine();
        }

        var xy = result
            .DistinctBy(c => c.Point)
            .Where(r => r.Point.Y < input.Length && r.Point.X < input[0].Length && r.Point.Y >= 0 && r.Point.X >= 0)
            .ToList();

        // foreach (var c in xy)
        // {
        //     Console.WriteLine(c);
        // }

        // Console.WriteLine();
        // Console.WriteLine($"{input.Length}/{input[0].Length}");
        // Console.WriteLine(xy.Count);

        return xy.Count;
    }

    private char[][] Parse()
    {
        return GetSplitInput()
            .Select(l => l.ToCharArray())
            .ToArray();
    }

    private record struct Coordinate(Point Point, char Char);

    // private record struct Coordinate(Point Point1, Point Point2, char Char);
}
