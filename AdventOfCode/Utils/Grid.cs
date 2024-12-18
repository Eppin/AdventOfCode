namespace AdventOfCode.Utils;

using Coordinate = Coordinate<int>;
using System.Drawing;

public struct Grid<T>(T[][] grid)
{
    public readonly long MaxY = grid.Length;
    public readonly long MaxX = grid[0].Length;

    [Obsolete("Use coordinate")] public T this[Point point] => grid[point.Y][point.X];

    public T this[Coordinate point]
    {
        get => grid[point.Y][point.X];
        set => grid[point.Y][point.X] = value;
    }

    public T this[int x, int y]
    {
        get => grid[y][x];
        set => grid[y][x] = value;
    }

    #region Old usage of Point class
    [Obsolete("Use coordinate")] public Point? North(int x, int y) => ValidateOld(Direction.North, x, y, 0, -1);
    [Obsolete("Use coordinate")] public Point? East(int x, int y) => ValidateOld(Direction.East, x, y, 1, 0);
    [Obsolete("Use coordinate")] public Point? South(int x, int y) => ValidateOld(Direction.South, x, y, 0, 1);
    [Obsolete("Use coordinate")] public Point? West(int x, int y) => ValidateOld(Direction.West, x, y, -1, 0);

    [Obsolete("Use coordinate")] public Point? NorthEast(int x, int y) => ValidateOld(Direction.NorthEast, x, y, 1, -1);
    [Obsolete("Use coordinate")] public Point? NorthWest(int x, int y) => ValidateOld(Direction.NorthWest, x, y, -1, -1);
    [Obsolete("Use coordinate")] public Point? SouthEast(int x, int y) => ValidateOld(Direction.SouthEast, x, y, 1, 1);
    [Obsolete("Use coordinate")] public Point? SouthWest(int x, int y) => ValidateOld(Direction.SouthWest, x, y, -1, 1);
    #endregion

    public Coordinate? North(Coordinate coordinate) => Validate(Direction.North, coordinate.X, coordinate.Y, 0, -1);
    public Coordinate? East(Coordinate coordinate) => Validate(Direction.East, coordinate.X, coordinate.Y, 1, 0);
    public Coordinate? South(Coordinate coordinate) => Validate(Direction.South, coordinate.X, coordinate.Y, 0, 1);
    public Coordinate? West(Coordinate coordinate) => Validate(Direction.West, coordinate.X, coordinate.Y, -1, 0);

    public Coordinate? NorthEast(Coordinate coordinate) => Validate(Direction.NorthEast, coordinate.X, coordinate.Y, 1, -1);
    public Coordinate? NorthWest(Coordinate coordinate) => Validate(Direction.NorthWest, coordinate.X, coordinate.Y, -1, -1);
    public Coordinate? SouthEast(Coordinate coordinate) => Validate(Direction.SouthEast, coordinate.X, coordinate.Y, 1, 1);
    public Coordinate? SouthWest(Coordinate coordinate) => Validate(Direction.SouthWest, coordinate.X, coordinate.Y, -1, 1);
    
    [Obsolete("Rewrite to not use Point class")]
    public IEnumerable<Point> Neighbours(Point point, bool includeDiagonal = false) => Neighbours(point.X, point.Y, includeDiagonal);

    [Obsolete("Rewrite to not use Point class")]
    public IEnumerable<Point> Neighbours(int x, int y, bool includeDiagonal = false)
    {
        var n = North(x, y);
        if (n != null) yield return n.Value;

        var e = East(x, y);
        if (e != null) yield return e.Value;

        var s = South(x, y);
        if (s != null) yield return s.Value;

        var w = West(x, y);
        if (w != null) yield return w.Value;

        if (!includeDiagonal) yield break;

        var ne = NorthEast(x, y);
        if (ne != null) yield return ne.Value;

        var nw = NorthWest(x, y);
        if (nw != null) yield return nw.Value;

        var se = SouthEast(x, y);
        if (se != null) yield return se.Value;

        var sw = SouthWest(x, y);
        if (sw != null) yield return sw.Value;
    }
    
    public Dictionary<Direction, Coordinate> Directions(Coordinate coordinate, bool includeDiagonal = false)
    {
        var directions = new Dictionary<Direction, Coordinate>();

        var n = North(coordinate);
        if (n != null) directions.Add(Direction.North, n.Value);

        var e = East(coordinate);
        if (e != null) directions.Add(Direction.East, e.Value);

        var s = South(coordinate);
        if (s != null) directions.Add(Direction.South, s.Value);

        var w = West(coordinate);
        if (w != null) directions.Add(Direction.West, w.Value);

        if (!includeDiagonal) return directions;

        var ne = NorthEast(coordinate);
        if (ne != null) directions.Add(Direction.NorthEast, ne.Value);

        var nw = NorthWest(coordinate);
        if (nw != null) directions.Add(Direction.NorthWest, nw.Value);

        var se = SouthEast(coordinate);
        if (se != null) directions.Add(Direction.SouthEast, se.Value);

        var sw = SouthWest(coordinate);
        if (sw != null) directions.Add(Direction.SouthWest, sw.Value);

        return directions;
    }

    public Dictionary<Direction, Point> Directions(Point point, bool includeDiagonal = false) => Directions(point.X, point.Y, includeDiagonal);

    public Dictionary<Direction, Point> Directions(int x, int y, bool includeDiagonal = false)
    {
        var directions = new Dictionary<Direction, Point>();

        var n = North(x, y);
        if (n != null) directions.Add(Direction.North, n.Value);

        var e = East(x, y);
        if (e != null) directions.Add(Direction.East, e.Value);

        var s = South(x, y);
        if (s != null) directions.Add(Direction.South, s.Value);

        var w = West(x, y);
        if (w != null) directions.Add(Direction.West, w.Value);

        if (!includeDiagonal) return directions;

        var ne = NorthEast(x, y);
        if (ne != null) directions.Add(Direction.NorthEast, ne.Value);

        var nw = NorthWest(x, y);
        if (nw != null) directions.Add(Direction.NorthWest, nw.Value);

        var se = SouthEast(x, y);
        if (se != null) directions.Add(Direction.SouthEast, se.Value);

        var sw = SouthWest(x, y);
        if (sw != null) directions.Add(Direction.SouthWest, sw.Value);

        return directions;
    }

    public void Draw()
    {
        for (var y = 0; y < MaxY; y++)
        {
            for (var x = 0; x < MaxX; x++)
                Console.Write(this[x, y]);

            Console.WriteLine();
        }
    }
    
    public void Fill(T value)
    {
        for (var y = 0; y < MaxY; y++)
        {
            for (var x = 0; x < MaxX; x++)
                this[x, y] = value;
        }
    }

    private Coordinate? Validate(Direction direction, int x, int y, int dx, int dy)
    {
        var point = new Coordinate(x + dx, y + dy);

        var valid = direction switch
        {
            Direction.North => point.Y >= 0,
            Direction.East => point.X < MaxX,
            Direction.South => point.Y < MaxY,
            Direction.West => point.X >= 0,
            Direction.NorthEast => point.Y >= 0 && point.X < MaxX,
            Direction.NorthWest => point.Y >= 0 && point.X >= 0,
            Direction.SouthEast => point.Y < MaxY && point.X < MaxX,
            Direction.SouthWest => point.Y < MaxY && point.X >= 0,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        return valid ? point : null;
    }

    [Obsolete("Use coordinate")]
    private Point? ValidateOld(Direction direction, int x, int y, int dx, int dy)
    {
        var coordinate = Validate(direction, x, y, dx, dy);

        return coordinate != null
            ? new Point(coordinate.Value.X, coordinate.Value.Y)
            : null;
    }
}
