namespace AdventOfCode._2015;

public class Day11 : Day
{
    public Day11() : base()
    {
    }

    public override string SolveA()
    {
        IList<char> input = Input.ToList();

        return string.Join("", Solve(input));
    }

    public override string SolveB()
    {
        IList<char> input = Input.ToList();
        input = Solve(input);

        return string.Join("", Solve(input));
    }

    private static IList<char> Solve(IList<char> input)
    {
        do
        {
            (input, _) = Increment(input);
        } while (!Valid(input));

        return input;
    }

    private static bool Valid(IList<char> input)
    {
        if (input.Any(s => s is 'i' or 'o' or 'l'))
            return false;

        var hasStraight = false;
        var overlapping = 0;
        var skipOverlapping = 0;

        for (var i = 0; i < input.Count; i++)
        {
            if (!hasStraight && i + 2 < input.Count)
            {
                var next1 = (char)(input[i] + 1);
                var next2 = (char)(input[i] + 2);

                if (input[i + 1] == next1 && input[i + 2] == next2)
                    hasStraight = true;
            }

            if (i < input.Count - 1 && i != skipOverlapping && input[i] == input[i + 1])
            {
                overlapping++;
                skipOverlapping = i + 1;
            }
        }

        return hasStraight && overlapping >= 2;
    }

    private static (IList<char>, int) Increment(IList<char> input, int position = 0)
    {
        var i = input.Count - position - 1;

        input[i] = IncrementChar(input[i]);

        if (input[i] == 'a')
            return Increment(input, ++position);

        if (position > 0 && input[i + 1] == 'a')
            position -= 1;

        return (input, position);
    }

    private static char IncrementChar(char c)
    {
        return 'z' == c ? 'a' : ++c;
    }
}
