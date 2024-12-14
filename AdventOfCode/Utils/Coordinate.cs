namespace AdventOfCode.Utils;

using System.Numerics;

public struct Coordinate<T>(T x, T y) : IEquatable<Coordinate<T>> where T : struct, INumber<T>
{
    public T X { get; set; } = x;
    public T Y { get; set; } = y;

    public Coordinate<T> Left => new(X - T.One, Y);
    public Coordinate<T> Right => new(X + T.One, Y);
    public Coordinate<T> Up => new(X, Y - T.One);
    public Coordinate<T> Down => new(X, Y + T.One);
    public Coordinate<T> UpLeft => new(X - T.One, Y - T.One);
    public Coordinate<T> UpRight => new(X + T.One, Y - T.One);
    public Coordinate<T> DownLeft => new(X - T.One, Y + T.One);
    public Coordinate<T> DownRight => new(X + T.One, Y + T.One);

    /// <summary>
    /// Returns horizontal and vertical neighbours
    /// </summary>
    public Coordinate<T>[] Neighbours => [Left, Right, Up, Down];

    /// <summary>
    /// Returns horizontal, vertical and diagonal neighbours
    /// </summary>
    public Coordinate<T>[] Adjacents => [Left, Right, Up, Down, UpLeft, UpRight, DownLeft, DownRight];

    // Operators
    public static bool operator ==(Coordinate<T> left, Coordinate<T> right) => left.X == right.X && left.Y == right.Y;
    public static bool operator !=(Coordinate<T> left, Coordinate<T> right) => !(left == right);
    public static Coordinate<T> operator +(Coordinate<T> a, Coordinate<T> b) => new(a.X + b.X, a.Y + b.Y);
    public static Coordinate<T> operator -(Coordinate<T> a, Coordinate<T> b) => new(a.X - b.X, a.Y - b.Y);

    // Equality
    public bool Equals(Coordinate<T> other) => this == other;
    public override bool Equals(object? obj) => obj is Coordinate<T> other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);
}
