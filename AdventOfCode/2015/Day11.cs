namespace AdventOfCode._2015;

public class Day11 : Day
{
    public Day11() : base()
    {
    }

    public override string SolveA()
    {
        IEnumerable<char> a = Input;
        int p = 0;
        for (int i = 0; i < 20_000; i++)
        {
            (a, p) = Increment(a.ToList());
            Console.WriteLine($"{string.Join("", a)}, {p}");
        }

        return "";
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private static (IEnumerable<char>, int) Increment(List<char> input, int position = 0)
    {
        var x = input.Count - position - 1;

        input[x] = IncrementChar(input[x]);

        if (input[x] == 'a')
            return Increment(input, ++position);

        if (position > 0 && input[x + 1] == 'a')
            position -= 1;

        return (input, position);
    }

    private static char IncrementChar(char c)
    {
        return 'z' == c ? 'a' : ++c;
    }
}
