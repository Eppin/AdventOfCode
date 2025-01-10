namespace AdventOfCode._2018;

public class Day1 : Day
{
    public Day1() : base()
    {
    }

    [Answer("479", Regular)]
    public override object SolveA()
    {
        return SplitInput.Sum(Calculate);
    }

    [Answer("66105", Regular)]
    public override object SolveB()
    {
        var total = 0;
        var foundFrequencies = new HashSet<int>();
        while (true)
        {
            foreach (var s in SplitInput)
            {
                total += Calculate(s);

                if (!foundFrequencies.Add(total))
                    return $"{total}";
            }
        }
    }
    
    private static int Calculate(string s)
    {
        var number = int.Parse(s[1..]);
        return s[0] == '+' ? number : number * -1;
    }
}
