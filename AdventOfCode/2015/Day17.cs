namespace AdventOfCode._2015;

public class Day17 : Day
{
    public Day17() : base()
    {
    }

    [Answer("1638", Regular)]
    public override object SolveA()
    {
        return new Combinations<int>(GetSplitInput().Select(int.Parse))
            .AllCombinations.Count(co => co.Sum(c => c) == 150);
    }

    [Answer("17", Regular)]
    public override object SolveB()
    {
        return new Combinations<int>(GetSplitInput().Select(int.Parse))
            .AllCombinations.Where(co => co.Sum(c => c) == 150)
            .GroupBy(c => c.Count())
            .OrderBy(c => c.Key)
            .First()
            .Count();
    }
}
