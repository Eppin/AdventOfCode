namespace AdventOfCode._2024;

public partial class Day13 : Day
{
    public Day13() : base()
    {
    }

    [Answer("480", Example, Data = "Button A: X+94, Y+34{nl}Button B: X+22, Y+67{nl}Prize: X=8400, Y=5400{nl}{nl}Button A: X+26, Y+66{nl}Button B: X+67, Y+21{nl}Prize: X=12748, Y=12176{nl}{nl}Button A: X+17, Y+86{nl}Button B: X+84, Y+37{nl}Prize: X=7870, Y=6450{nl}{nl}Button A: X+69, Y+23{nl}Button B: X+27, Y+71{nl}Prize: X=18641, Y=10279")]
    [Answer("29187", Regular)]
    public override object SolveA()
    {
        return Solve();
    }

    [Answer("875318608908", Example, Data = "Button A: X+94, Y+34{nl}Button B: X+22, Y+67{nl}Prize: X=8400, Y=5400{nl}{nl}Button A: X+26, Y+66{nl}Button B: X+67, Y+21{nl}Prize: X=12748, Y=12176{nl}{nl}Button A: X+17, Y+86{nl}Button B: X+84, Y+37{nl}Prize: X=7870, Y=6450{nl}{nl}Button A: X+69, Y+23{nl}Button B: X+27, Y+71{nl}Prize: X=18641, Y=10279")]
    [Answer("99968222587852", Regular)]
    public override object SolveB()
    {
        return Solve(10_000_000_000_000);
    }

    // Solve by 'simple elimination'
    // a*ax + b*bx = px is a*ay +b*by = py
    // now only 'a' and 'b' are unknown
    private long Solve(long extra = 0)
    {
        var games = Parse();
        var costs = 0L;

        foreach (var game in games)
        {
            var ax = game.ButtonA.X;
            var bx = game.ButtonB.X;

            var ay = game.ButtonA.Y;
            var by = game.ButtonB.Y;

            var px = game.Prize.X + extra;
            var py = game.Prize.Y + extra;

            // Step 1: Calculate the determinant of the coefficient matrix
            var determinant = ax * by - ay * bx;

            if (determinant == 0)
                continue;

            // Step 2: Solve the system
            var _1 = ax * by;
            var _2 = bx * ay;

            var _3 = px * by;
            var _4 = py * bx;

            // Step 3: Subtract the equations
            var _5 = _1 - _2;
            var _6 = _3 - _4;

            // Needs to be an int after division, since we're talking about button presses!
            if (_6 % _5 != 0)
                continue;

            var a = _6 / _5;

            if (a <= 0)
                continue;

            // Step 4: Substitute 'a' into one of the original equations
            var _7 = a * ax;
            var _8 = px - _7;

            // Needs to be an int after division, since we're talking about button presses!
            if (_8 % bx != 0)
                continue;

            var b = _8 / bx;

            if (b <= 0)
                continue;

            var cost = a * 3 + b * 1;
            costs += cost;
        }

        return costs;
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

    private static Coordinate Value(string value)
    {
        var regex = CoordinateRegex();
        var result = regex.Match(value);

        if (!result.Success)
            throw new Exception($"Unable to match X and/or Y for [{value}]");

        var x = result.Groups[1].Value;
        var y = result.Groups[2].Value;

        return new Coordinate(int.Parse(x), int.Parse(y));

    }

    private class Game(Coordinate buttonA, Coordinate buttonB, Coordinate prize)
    {
        public Coordinate ButtonA { get; } = buttonA;
        public Coordinate ButtonB { get; } = buttonB;
        public Coordinate Prize { get; } = prize;
    }

    [GeneratedRegex(@"X.(\d+), Y.(\d+)")]
    private static partial Regex CoordinateRegex();
}
