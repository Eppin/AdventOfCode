namespace AdventOfCode._2015;

public class Day3 : Day
{
    public Day3() : base()
    {
    }

    [Answer("2", Example, Data = "^v^v^v^v^v")]
    [Answer("2081", Regular)]
    public override object SolveA()
    {
        int x = 0, y = 0;
        var results = CreateResults(x, y);

        foreach (var c in Input)
        {
            (x, y) = GetCoords(c, x, y);

            var result = results.SingleOrDefault(g => g.X == x && g.Y == y);
            if (result == null)
                results.Add(new(x, y, 1));
            else
                result.Count += 1;
        }

        return results.Count;
    }

    [Answer("11", Example, Data = "^v^v^v^v^v")]
    [Answer("2341", Regular)]
    public override object SolveB()
    {
        int xSanta = 0, ySanta = 0;
        int xRobot = 0, yRobot = 0;
        var isRobot = false;
        var results = CreateResults(0, 0);
        
        foreach (var c in Input)
        {
            int x, y;

            if (!isRobot)
                (x, y) = (xSanta, ySanta) = GetCoords(c, xSanta, ySanta);
            else
                (x, y) = (xRobot, yRobot) = GetCoords(c, xRobot, yRobot);

            var result = results.SingleOrDefault(g => g.X == x && g.Y == y);
            if (result == null)
                results.Add(new(x, y, 1));
            else
                result.Count += 1;

            isRobot = !isRobot;
        }

        return results.Count;
    }

    private static List<Grid> CreateResults(int x, int y)
    {
        return new List<Grid> { new(x, y, 1) };
    }

    private static (int x, int y) GetCoords(char c, int x, int y)
    {
        switch (c)
        {
            case '^':
                return (x, ++y);

            case 'v':
                return (x, --y);

            case '>':
                return (++x, y);

            case '<':
                return (--x, y);
        }

        return (x, y);
    }

    private class Grid
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Count { get; set; }

        public Grid(int x, int y, int count)
        {
            X = x;
            Y = y;
            Count = count;
        }
    }
}