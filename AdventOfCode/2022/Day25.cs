namespace AdventOfCode._2022;

public class Day25 : Day
{
    public Day25() : base()
    {
    }

    [Answer("2=-0=01----22-0-1-10", Regular)]
    public override object SolveA()
    {
        var sum = SplitInput
            .Select(FromSnafu)
            .Sum();

        return ToSnafu(sum);
    }

    [Answer("christmas \ud83c\udf84", Regular)]
    public override object SolveB()
    {
        throw new NotImplementedException();
    }

    private static long FromSnafu(string input)
    {
        double total = 0;

        var reversed = input
            .Reverse()
            .Select((c, i) => (Index: i, Character: c));

        foreach (var c in reversed)
        {
            var number = c.Character switch
            {
                '-' => -1,
                '=' => -2,
                _ => int.Parse(c.Character.ToString())
            };

            if (number == 0)
                continue;

            total += Math.Pow(5, c.Index) * number;
        }

        return Convert.ToInt64(total);
    }

    private static string ToSnafu(long input)
    {
        var result = string.Empty;
        while (input > 0)
        {
            var mod = input % 5;

            var character = mod switch
            {
                3 => "=",
                4 => "-",
                _ => mod.ToString()
            };

            result = character + result;

            if (mod is 3 or 4)
                input += 5;

            input /= 5;
        }

        return result;
    }
}