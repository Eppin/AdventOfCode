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

    public IEnumerable<Point> Neighbours(int x, int y)
    {
        var n = North(x, y);
        if (n != null) yield return n.Value;

        var e = East(x, y);
        if (e != null) yield return e.Value;

        var s = South(x, y);
        if (s != null) yield return s.Value;

        var w = West(x, y);
        if (w != null) yield return w.Value;
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
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        return valid ? point : null;
    }
}
