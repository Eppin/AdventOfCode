namespace AdventOfCode.Utils;

using System.Drawing;

public class Grid<T>(T[][] grid)
{
    public readonly long MaxY = grid.Length;
    public readonly long MaxX = grid[0].Length;

    public T this[Point point] => grid[point.Y][point.X];
    public T this[int x, int y] => grid[y][x];

    public Point? North(int x, int y) => Validate(Direction.North, x, y, 0, -1);
    public Point? East(int x, int y) => Validate(Direction.East, x, y, 1, 0);
    public Point? South(int x, int y) => Validate(Direction.South, x, y, 0, 1);
    public Point? West(int x, int y) => Validate(Direction.West, x, y, -1, 0);

    public Point? NorthEast(int x, int y) => Validate(Direction.NorthEast, x, y, 1, -1);
    public Point? NorthWest(int x, int y) => Validate(Direction.NorthWest, x, y, -1, -1);
    public Point? SouthEast(int x, int y) => Validate(Direction.SouthEast, x, y, 1, 1);
    public Point? SouthWest(int x, int y) => Validate(Direction.SouthWest, x, y, -1, 1);

    public IEnumerable<Point> Neighbours(Point point, bool includeDiagonal = false) => Neighbours(point.X, point.Y, includeDiagonal);

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

    private Point? Validate(Direction direction, int x, int y, int dx, int dy)
    {
        var point = new Point(x + dx, y + dy);

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
}
