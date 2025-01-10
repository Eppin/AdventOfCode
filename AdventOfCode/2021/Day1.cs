namespace AdventOfCode._2021;

public class Day1 : Day
{
    public Day1() : base()
    {
    }

    [Answer("1195", Regular)]
    public override object SolveA()
    {
        var increases = 0;
        var previous = 0;

        foreach (var (i, j) in Parse())
        {
            if (j != 0 && i > previous)
                increases += 1;

            previous = i;
        }

        return $"{increases}";
    }

    [Answer("1235", Regular)]
    public override object SolveB()
    {
        var sums = 0;
        var previous = 0;

        var input = GetSplitInput().ToList();

        foreach (var (i, j) in Parse())
        {
            if (input.Count - 1 < j + 2)
                break;

            var sum = i + int.Parse(input[j + 1]) + int.Parse(input[j + 2]);

            if (j != 0 && sum > previous)
                sums += 1;

            previous = sum;
        }

        return $"{sums}";
    }

    private List<(int, int)> Parse()
    {
        return GetSplitInput()
            .Select((x, y) => (int.Parse(x), y))
            .ToList();
    }
}
