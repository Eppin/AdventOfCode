namespace AdventOfCode._2018;

public class Day1 : Day
{
    public Day1() : base()
    {
    }

    public override string SolveA()
    {
        return SplitInput.Sum(Calculate).ToString();
    }

    public override string SolveB()
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
