namespace AdventOfCode._2024;

using System.Drawing;

public partial class Day13 : Day
{
    public Day13() : base()
    {
    }

    [Answer("480", Example, Data = "Button A: X+94, Y+34{nl}Button B: X+22, Y+67{nl}Prize: X=8400, Y=5400{nl}{nl}Button A: X+26, Y+66{nl}Button B: X+67, Y+21{nl}Prize: X=12748, Y=12176{nl}{nl}Button A: X+17, Y+86{nl}Button B: X+84, Y+37{nl}Prize: X=7870, Y=6450{nl}{nl}Button A: X+69, Y+23{nl}Button B: X+27, Y+71{nl}Prize: X=18641, Y=10279")]
    [Answer("29187", Regular)]
    public override string SolveA()
    {
        var games = Parse();
        var costs = 0;

        foreach (var game in games)
        {
            var cost = 0;

            for (var a = 1; a <= 100; a++)
            {
                for (var b = 1; b <= 100; b++)
                {
                    var prizeX = game.ButtonA.X * a + game.ButtonB.X * b;
                    var prizeY = game.ButtonA.Y * a + game.ButtonB.Y * b;

                    if (prizeX != game.Prize.X || prizeY != game.Prize.Y)
                        continue;

                    var total = a * 3 + b * 1;

                    if (cost == 0) cost = total;
                    else if (total < cost) cost = total;
                }
            }

            costs += cost;
        }

        return costs.ToString();
    }

    [Answer("", Example, Data = "Button A: X+94, Y+34{nl}Button B: X+22, Y+67{nl}Prize: X=8400, Y=5400{nl}{nl}Button A: X+26, Y+66{nl}Button B: X+67, Y+21{nl}Prize: X=12748, Y=12176{nl}{nl}Button A: X+17, Y+86{nl}Button B: X+84, Y+37{nl}Prize: X=7870, Y=6450{nl}{nl}Button A: X+69, Y+23{nl}Button B: X+27, Y+71{nl}Prize: X=18641, Y=10279")]
    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private List<Game> Parse()
    {
        var games = new List<Game>();

        var input = GetSplitInput();
        for (var i = 0; i < input.Length; i += 3)
        {
            var buttonA = input[i];
            var buttonB = input[i + 1];
            var prize = input[i + 2];

            var game = new Game(Value(buttonA), Value(buttonB), Value(prize));
            games.Add(game);
        }

        return games;
    }

    private static Point Value(string value)
    {
        var regex = CoordinateRegex();
        var result = regex.Match(value);

        if (result.Success)
        {
            var x = result.Groups[1].Value;
            var y = result.Groups[2].Value;

            return new Point(int.Parse(x), int.Parse(y));
        }

        throw new Exception($"Unable to match X and/or Y for [{value}]");
    }

    private class Game(Point buttonA, Point buttonB, Point prize)
    {
        public Point ButtonA { get; } = buttonA;
        public Point ButtonB { get; } = buttonB;
        public Point Prize { get; } = prize;
    }

    [GeneratedRegex(@"X.(\d+), Y.(\d+)")]
    private static partial Regex CoordinateRegex();
}
