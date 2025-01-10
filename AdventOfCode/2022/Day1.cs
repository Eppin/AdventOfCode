namespace AdventOfCode._2022;

public class Day1 : Day
{
    public Day1() : base()
    {
    }

    [Answer("68787", Regular)]
    public override object SolveA()
    {
        return Solve(1);
    }

    [Answer("198041", Regular)]
    public override object SolveB()
    {
        return Solve(3);
    }

    private int Solve(int take)
    {
        var totalCalories = 0;
        var calories = new List<int>();

        foreach (var line in GetSplitInput(false))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                calories.Add(totalCalories);
                totalCalories = 0;
            }
            else
            {
                if (!int.TryParse(line, out var calorie))
                    throw new DataException($"Couldn't read input data [{line}]");

                totalCalories += calorie;
            }
        }

        return calories
            .OrderDescending()
            .Take(take)
            .Sum();
    }
}