namespace AdventOfCode._2025;

public class Day1 : Day
{
    public Day1() : base()
    {
    }

    [Answer("3", Example, Data = "L68{nl}L30{nl}R48{nl}L5{nl}R60{nl}L55{nl}L1{nl}L99{nl}R14{nl}L82")]
    [Answer("1102", Regular)]
    public override object SolveA()
    {
        var (a, _) = Solve();
        return a;
    }

    [Answer("6", Example, Data = "L68{nl}L30{nl}R48{nl}L5{nl}R60{nl}L55{nl}L1{nl}L99{nl}R14{nl}L82")]
    [Answer("6175", Regular)]
    public override object SolveB()
    {
        var (_, b) = Solve();
        return b;
    }

    private (int EndZero, int AllZero) Solve()
    {
        var allZero = 0;
        var endZero = 0;

        var dial = 50;
        var input = Parse();

        foreach (var (dir, count) in input)
        {
            switch (dir)
            {
                case 'L':
                    {
                        for (var i = 0; i < count; i++)
                        {
                            dial -= 1;
                            if (dial < 0) dial = 99;
                            if (dial == 0) allZero++;
                        }

                        break;
                    }

                case 'R':
                    {
                        for (var i = 0; i < count; i++)
                        {
                            dial += 1;
                            if (dial >= 100) dial = 0;
                            if (dial == 0) allZero++;
                        }

                        break;
                    }
            }

            if (dial == 0) endZero++;
        }

        return (endZero, allZero);
    }

    private List<(char, int)> Parse()
    {
        return SplitInput
            .Select(input =>
            {
                var direction = input[0];
                var distance = int.Parse(input[1..]);

                return (direction, distance);
            }).ToList();
    }
}
