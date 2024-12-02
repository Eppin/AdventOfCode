namespace AdventOfCode._2015;

public class Day17 : Day
{
    public Day17() : base()
    {
    }

    public override string SolveA()
    {
        return new Combinations<int>(GetSplitInput().Select(int.Parse))
            .AllCombinations.Count(co => co.Sum(c => c) == 150)
            .ToString();
    }

    public override string SolveB()
    {
        return new Combinations<int>(GetSplitInput().Select(int.Parse))
            .AllCombinations.Where(co => co.Sum(c => c) == 150)
            .GroupBy(c => c.Count())
            .OrderBy(c => c.Key)
            .First()
            .Count()
            .ToString();
    }
}
