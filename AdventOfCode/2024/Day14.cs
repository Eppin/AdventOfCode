namespace AdventOfCode._2024;

using Coordinate = Coordinate<int>;
using System.Text;

public partial class Day14 : Day
{
    private const int Width = 101;
    private const int Height = 103;

    public Day14() : base()
    {
    }

    [Answer("218619324", Regular)]
    public override string SolveA()
    {
        var robots = Parse();

        for (var i = 0; i < 100; i++)
        {
            foreach (var robot in robots)
                Move(robot);
        }

        // Quadrant
        const int halfWidth = Width / 2;
        const int halfHeight = Height / 2;

        var quadrant1 = 0;
        var quadrant2 = 0;
        var quadrant3 = 0;
        var quadrant4 = 0;

        foreach (var robot in robots)
        {
            switch (robot.Position.X)
            {
                case >= 0 and <= halfWidth - 1 when robot.Position.Y is >= 0 and <= halfHeight - 1:
                    quadrant1++;
                    break;

                case >= halfWidth + 1 and <= Width - 1 when robot.Position.Y is >= halfHeight + 1 and <= Height - 1:
                    quadrant4++;
                    break;
            }

            switch (robot.Position.X)
            {
                case >= 0 and <= halfWidth - 1 when robot.Position.Y is >= halfHeight + 1 and <= Height - 1:
                    quadrant2++;
                    break;

                case >= halfWidth + 1 and <= Width - 1 when robot.Position.Y is >= 0 and <= halfHeight - 1:
                    quadrant3++;
                    break;
            }
        }

        return (quadrant1 * quadrant2 * quadrant3 * quadrant4).ToString();
    }

    [Answer("6446", Regular)]
    public override string SolveB()
    {
        var robots = Parse();

        for (var i = 0; i < Height * Width; i++)
        {
            foreach (var robot in robots)
                Move(robot);

            // Found these string by forcing all grids to a file and search for the christmas tree
            var draw = Draw(robots, Width, Height);
            if (draw.Contains("1111111111111111111111111111111"))
                return (i + 1).ToString();
        }

        return 0.ToString();
    }

    private static void Move(Robot robot)
    {
        robot.Position = (robot.Position.X + robot.Velocity.X) switch
        {
            < 0 => robot.Position with { X = Width + robot.Position.X + robot.Velocity.X },
            >= Width => robot.Position with { X = robot.Position.X + robot.Velocity.X - Width },
            _ => robot.Position with { X = robot.Position.X + robot.Velocity.X }
        };

        robot.Position = (robot.Position.Y + robot.Velocity.Y) switch
        {
            < 0 => robot.Position with { Y = Height + robot.Position.Y + robot.Velocity.Y },
            >= Height => robot.Position with { Y = robot.Position.Y + robot.Velocity.Y - Height },
            _ => robot.Position with { Y = robot.Position.Y + robot.Velocity.Y }
        };
    }

    private static string Draw(List<Robot> robots, int width, int height)
    {
        var grid = CreateGrid(width, height);
        var sb = new StringBuilder();

        foreach (var robot in robots)
            grid[robot.Position.X, robot.Position.Y] += 1;

        for (var y = 0; y < grid.MaxY; y++)
        {
            for (var x = 0; x < grid.MaxX; x++)
                sb.Append(grid[x, y]);

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static Grid<int> CreateGrid(int width, int height)
    {
        var grid = new int[height][];

        for (var length = 0; length < grid.Length; length++)
            grid[length] = new int[width];

        return new Grid<int>(grid);
    }

    private List<Robot> Parse()
    {
        var list = new List<Robot>();

        foreach (var line in GetSplitInput())
        {
            var match = Regex().Match(line);

            if (!match.Success)
                throw new Exception($"Regex failed for [{line}]");

            var px = int.Parse(match.Groups[1].Value);
            var py = int.Parse(match.Groups[2].Value);

            var vx = int.Parse(match.Groups[3].Value);
            var vy = int.Parse(match.Groups[4].Value);

            list.Add(new Robot(new Coordinate(px, py), new Coordinate(vx, vy)));
        }

        return list;
    }

    private class Robot(Coordinate position, Coordinate velocity)
    {
        public Coordinate Position { get; set; } = position;
        public Coordinate Velocity { get; set; } = velocity;
    }

    [GeneratedRegex(@"p=(-\d+|\d+),(-\d+|\d+) v=(-\d+|\d+),(-\d+|\d+)")]
    private static partial Regex Regex();
}
