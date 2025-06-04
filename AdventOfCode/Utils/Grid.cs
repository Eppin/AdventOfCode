namespace AdventOfCode.Utils;

public struct Grid<T>(T[][] grid)
{
    public readonly long MaxY = grid.Length;
    public readonly long MaxX = grid[0].Length;

    public T this[Coordinate coordinate]
    {
        get => grid[coordinate.Y][coordinate.X];
        set => grid[coordinate.Y][coordinate.X] = value;
    }

    public T this[int x, int y]
    {
        get => grid[y][x];
        set => grid[y][x] = value;
    }

    public Coordinate? North(Coordinate coordinate) => Validate(Direction.North, coordinate.X, coordinate.Y, 0, -1);
    public Coordinate? East(Coordinate coordinate) => Validate(Direction.East, coordinate.X, coordinate.Y, 1, 0);
    public Coordinate? South(Coordinate coordinate) => Validate(Direction.South, coordinate.X, coordinate.Y, 0, 1);
    public Coordinate? West(Coordinate coordinate) => Validate(Direction.West, coordinate.X, coordinate.Y, -1, 0);

    public Coordinate? NorthEast(Coordinate coordinate) => Validate(Direction.NorthEast, coordinate.X, coordinate.Y, 1, -1);
    public Coordinate? NorthWest(Coordinate coordinate) => Validate(Direction.NorthWest, coordinate.X, coordinate.Y, -1, -1);
    public Coordinate? SouthEast(Coordinate coordinate) => Validate(Direction.SouthEast, coordinate.X, coordinate.Y, 1, 1);
    public Coordinate? SouthWest(Coordinate coordinate) => Validate(Direction.SouthWest, coordinate.X, coordinate.Y, -1, 1);

    public IEnumerable<Coordinate> Neighbours(Coordinate coordinate, bool includeDiagonal = false)
    {
        var n = North(coordinate);
        if (n != null) yield return n.Value;

        var e = East(coordinate);
        if (e != null) yield return e.Value;

        var s = South(coordinate);
        if (s != null) yield return s.Value;

        var w = West(coordinate);
        if (w != null) yield return w.Value;

        if (!includeDiagonal) yield break;

        var ne = NorthEast(coordinate);
        if (ne != null) yield return ne.Value;

        var nw = NorthWest(coordinate);
        if (nw != null) yield return nw.Value;

        var se = SouthEast(coordinate);
        if (se != null) yield return se.Value;

        var sw = SouthWest(coordinate);
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
        var coordinate = new Coordinate(x + dx, y + dy);

        var valid = direction switch
        {
            Direction.North => coordinate.Y >= 0,
            Direction.East => coordinate.X < MaxX,
            Direction.South => coordinate.Y < MaxY,
            Direction.West => coordinate.X >= 0,
            Direction.NorthEast => coordinate.Y >= 0 && coordinate.X < MaxX,
            Direction.NorthWest => coordinate.Y >= 0 && coordinate.X >= 0,
            Direction.SouthEast => coordinate.Y < MaxY && coordinate.X < MaxX,
            Direction.SouthWest => coordinate.Y < MaxY && coordinate.X >= 0,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        return valid ? coordinate : null;
    }
}
