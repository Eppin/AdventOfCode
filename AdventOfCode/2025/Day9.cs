
namespace AdventOfCode._2025;

public class Day9 : Day
{
    public Day9() : base()
    {
    }

    [Answer("50", Example, Data = "7,1{nl}11,1{nl}11,7{nl}9,7{nl}9,5{nl}2,5{nl}2,3{nl}7,3")]
    [Answer("4776100539", Regular)]
    public override object SolveA()
    {
        var input = Parse();
        var maxSize = 0L;

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = i + 1; j < input.Length; j++)
            {
                var x = Math.Abs(input[i].X - input[j].X) + 1;
                var y = Math.Abs(input[i].Y - input[j].Y) + 1;
                var size = (long)x * y;

                if (size > maxSize)
                    maxSize = size;
            }
        }

        return maxSize;
    }

    [Answer("24", Example, Data = "7,1{nl}11,1{nl}11,7{nl}9,7{nl}9,5{nl}2,5{nl}2,3{nl}7,3")]
    [Answer("1476550548", Regular)]
    public override object SolveB()
    {
        var input = Parse();
        var maxSize = 0L;

        var lines = new List<(Coordinate From, Coordinate To)>();
        for (var i = 0; i < input.Length; i++)
        {
            var from = input[i];
            var to = i == input.Length - 1 ? input[0] : input[i + 1];

            var minX = Math.Min(from.X, to.X);
            var maxX = Math.Max(from.X, to.X);

            var minY = Math.Min(from.Y, to.Y);
            var maxY = Math.Max(from.Y, to.Y);

            if (minX == maxX)
            {
                from = new Coordinate(minX, minY);
                to = new Coordinate(minX, maxY);
            }
            else if (minY == maxY)
            {
                from = new Coordinate(minX, minY);
                to = new Coordinate(maxX, minY);
            }
            else throw new DataException();

            lines.Add((from, to));
        }

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = i + 1; j < input.Length; j++)
            {
                var minY = Math.Min(input[i].Y, input[j].Y);
                var maxY = Math.Max(input[i].Y, input[j].Y);

                var minX = Math.Min(input[i].X, input[j].X);
                var maxX = Math.Max(input[i].X, input[j].X);

                var a = new Coordinate(minX, minY);
                var b = new Coordinate(minX, maxY);
                var c = new Coordinate(maxX, minY);
                var d = new Coordinate(maxX, maxY);

                var edges = new[] { (a, c), (c, d), (d, b), (b, a) };
                if (Intersects(edges, lines))
                    continue;

                var coordinates = new[] { a, b, c, d };
                if (!Edges(coordinates, lines))
                    continue;

                var x = Math.Abs(input[i].X - input[j].X) + 1;
                var y = Math.Abs(input[i].Y - input[j].Y) + 1;
                var size = (long)x * y;

                if (size > maxSize)
                    maxSize = size;
            }
        }

        return maxSize;
    }

    private static bool Intersects((Coordinate from, Coordinate to)[] edges, List<(Coordinate From, Coordinate To)> lines)
    {
        foreach (var (from, to) in lines)
        {

            foreach (var (edgeFrom, edgeTo) in edges)
            {
                var minY = Math.Min(edgeFrom.Y, edgeTo.Y);
                var maxY = Math.Max(edgeFrom.Y, edgeTo.Y);

                var minX = Math.Min(edgeFrom.X, edgeTo.X);
                var maxX = Math.Max(edgeFrom.X, edgeTo.X);

                // Vertical line
                if (minX == maxX && from.Y == to.Y &&
                    from.X < minX && to.X > minX &&
                    minY < from.Y && from.Y < maxY)
                {
                    return true;
                }

                // Horizontal line
                if (minY == maxY && from.X == to.X &&
                    from.Y < minY && to.Y > minY &&
                    minX < from.X && from.X < maxX)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool Edges(Coordinate[] coordinates, List<(Coordinate From, Coordinate To)> lines)
    {
        var dict = new Dictionary<int, Edge>();
        for (var i = 0; i < coordinates.Length; i++)
        {
            dict.Add(i, new Edge(false, false, false, false));
        }

        foreach (var (from, to) in lines)
        {
            for (var i = 0; i < coordinates.Length; i++)
            {
                if (dict[i].Valid)
                    continue;

                var coordinate = coordinates[i];

                double minY = Math.Min(from.Y, to.Y);
                double maxY = Math.Max(from.Y, to.Y);

                double minX = Math.Min(from.X, to.X);
                double maxX = Math.Max(from.X, to.X);

                // Calculate for vertical lines only
                if (coordinate.Y >= minY && coordinate.Y <= maxY && from.X == to.X)
                {
                    if (coordinate.X > minX)
                    {
                        // Point is to the left of the line
                        dict[i].Left = true;
                    }
                    else if (coordinate.X < minX)
                    {
                        // Point is to the right of the line
                        dict[i].Right = true;
                    }
                    else
                    {
                        // Point is on the line
                        dict[i].Left = dict[i].Right = true;
                    }
                }

                // Calculate for horizontal lines only
                if (coordinate.X >= minX && coordinate.X <= maxX && from.Y == to.Y)
                {
                    if (coordinate.Y > minY)
                    {
                        // Point is to the top of the line
                        dict[i].Top = true;
                    }
                    else if (coordinate.Y < minY)
                    {
                        // Point is to the bottom of the line
                        dict[i].Bottom = true;
                    }
                    else
                    {
                        // Point is on the line
                        dict[i].Top = dict[i].Bottom = true;
                    }
                }
            }
        }

        return dict.All(e => e.Value.Valid);
    }

    private Coordinate[] Parse()
    {
        return SplitInput.Select(s =>
        {
            var split = s.Split(',');
            return new Coordinate(int.Parse(split[0]), int.Parse(split[1]));
        }).ToArray();
    }

    private class Edge(bool left, bool right, bool top, bool bottom)
    {
        public bool Left { get; set; } = left;
        public bool Right { get; set; } = right;
        public bool Top { get; set; } = top;
        public bool Bottom { get; set; } = bottom;

        public bool Valid => Left && Right && Top && Bottom;
    }
}