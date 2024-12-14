namespace AdventOfCode._2024;

using System.Drawing;

public partial class Day14 : Day
{
    // temp
    private const int Width = 101; //11;
    private const int Height = 103; //7;

    public Day14() : base()
    {
    }

    [Answer("", Example, Data = "p=0,4 v=3,-3{nl}p=6,3 v=-1,-3{nl}p=10,3 v=-1,2{nl}p=2,0 v=2,-1{nl}p=0,0 v=1,3{nl}p=3,0 v=-2,-2{nl}p=7,6 v=-1,-3{nl}p=3,0 v=-1,-2{nl}p=9,3 v=2,3{nl}p=7,3 v=-1,2{nl}p=2,4 v=2,-3{nl}p=9,5 v=-3,-3")]
    [Answer("", Regular)]
    public override string SolveA()
    {
        var robots = Parse();

        for (int i = 0; i < 100; i++)
        {
            Console.WriteLine($"After {i + 1} second(s)");

            foreach (var robot in robots)
            {
                // Console.WriteLine($"B, Start: {robot.Position.X}, {robot.Position.Y}, Velocity: {robot.Velocity.X}, {robot.Velocity.Y} ");
                Move(robot);
            }

            Console.WriteLine();
        }

        // State
        Draw(robots, Width, Height);
        Console.WriteLine();

        // Quadrant

        var halfWidth = Width / 2;
        var halfHeight = Height / 2;

        // 0 -> halfWidth -1 == X
        // 0 -> halfHeight -1 == Y

        // halfWidth -> width == X
        // 0 -> halfHeight -1 == Y

        // halfWidth -> width == X
        // 0 -> halfHeight -1 == Y

        var q1 = 0;
        var q2 = 0;
        var q3 = 0;
        var q4 = 0;

        Console.WriteLine($"{new Point(0, 0)} => {new Point(halfWidth - 1, halfHeight - 1)}");
        Console.WriteLine($"{new Point(0, halfHeight + 1)} => {new Point(halfWidth - 1, Height - 1)}");

        Console.WriteLine($"{new Point(halfWidth + 1, 0)} => {new Point(Width - 1, halfHeight - 1)}");
        Console.WriteLine($"{new Point(halfWidth + 1, halfHeight + 1)} => {new Point(Width - 1, Height - 1)}");

        foreach (var robot in robots)
        {
            if (robot.Position.X >= 0 && robot.Position.X <= halfWidth - 1 && robot.Position.Y >= 0 && robot.Position.Y <= halfHeight - 1)
                q1++;

            if (robot.Position.X >= 0 && robot.Position.X <= halfWidth - 1 && robot.Position.Y >= halfHeight + 1 && robot.Position.Y <= Height - 1)
                q2++;

            if (robot.Position.X >= halfWidth + 1 && robot.Position.X <= Width - 1 && robot.Position.Y >= 0 && robot.Position.Y <= halfHeight - 1)
                q3++;

            if (robot.Position.X >= halfWidth + 1 && robot.Position.X <= Width - 1 && robot.Position.Y >= halfHeight + 1 && robot.Position.Y <= Height - 1)
                q4++;
        }

        Console.WriteLine($"{q1}, {q2}, {q3}, {q4}");


        /*
q1  0,0 -> 4,2
q2  0,4  -> 4,5

q3  5,0 -> 9,2
q4  5,3   -> 9,5
         */


        return (q1 * q2 * q3 * q4).ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private static void Move(Robot robot)
    {
        var velocityX = robot.Velocity.X;
        var velocityY = robot.Velocity.Y;

        if (robot.Position.X + velocityX < 0) // Jump left of the grid
        {
            robot.Position = robot.Position with { X = Width + (robot.Position.X + velocityX) };
        }
        else if (robot.Position.X + velocityX >= Width) // Jump right of the grid
        {
            robot.Position = robot.Position with { X = robot.Position.X + velocityX - Width };
        }
        else // Default change
        {
            robot.Position = robot.Position with { X = robot.Position.X + velocityX };
        }

        if (robot.Position.Y + velocityY < 0) // Jump at top of the grid
        {
            robot.Position = robot.Position with { Y = Height + (robot.Position.Y + velocityY) };
        }
        else if (robot.Position.Y + velocityY >= Height) // Jump at bottom of the grid
        {
            robot.Position = robot.Position with { Y = robot.Position.Y + velocityY - Height };
        }
        else // Default change
        {
            robot.Position = robot.Position with { Y = robot.Position.Y + velocityY };
        }
    }

    private static void Draw(List<Robot> robots, int width, int height)
    {
        var grid = CreateGrid(width, height);

        foreach (var robot in robots)
            grid[robot.Position.X, robot.Position.Y] += 1;

        for (int y = 0; y < grid.MaxY; y++)
        {
            for (int x = 0; x < grid.MaxX; x++)
            {
                Console.Write(grid[x, y]);
            }

            Console.WriteLine();
        }
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

            list.Add(new Robot(new Point(px, py), new Point(vx, vy)));
        }

        return list;
    }

    private class Robot(Point position, Point velocity)
    {
        public Point Position { get; set; } = position;
        public Point Velocity { get; set; } = velocity;
    }

    [GeneratedRegex(@"p=(-\d+|\d+),(-\d+|\d+) v=(-\d+|\d+),(-\d+|\d+)")]
    private static partial Regex Regex();
}
