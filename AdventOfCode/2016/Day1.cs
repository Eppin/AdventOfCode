namespace AdventOfCode._2016;

using static Int32;

public class Day1 : Day
{
    public Day1() : base()
    {
    }

    [Answer("230", Regular)]
    public override object SolveA()
    {
        return Solve(false);
    }

    [Answer("154", Regular)]
    public override object SolveB()
    {
        return Solve(true);
    }

    private string Solve(bool isPartB)
    {
        var directions = Input.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        int x = 0, y = 0;
        var face = Face.North;

        var coordinates = new List<(int x, int y)> { new(0, 0) };

        foreach (var direction in directions)
        {
            if (!TryParse(direction[1..], out var distance))
                throw new DataException($"Invalid direction [{direction[0]}/{direction[1..]}]");

            switch (direction[0])
            {
                case 'R':
                    face = (Face)(((int)face + 1 + 4) % 4);
                    break;

                case 'L':
                    face = (Face)(((int)face - 1 + 4) % 4);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            for (var i = 0; i < distance; i++)
            {
                switch (face)
                {
                    case Face.North:
                        y++;
                        break;
                    case Face.East:
                        x++;
                        break;
                    case Face.South:
                        y--;
                        break;
                    case Face.West:
                        x--;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (isPartB && coordinates.Any(c => c.x == x && c.y == y))
                    return $"{Math.Abs(x) + Math.Abs(y)}";

                coordinates.Add((x, y));
            }
        }

        return $"{Math.Abs(x) + Math.Abs(y)}";
    }

    private enum Face
    {
        North,
        East,
        South,
        West
    }
}