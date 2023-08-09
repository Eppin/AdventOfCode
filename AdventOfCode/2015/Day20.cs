namespace AdventOfCode._2015;

using System.Diagnostics;

public class Day20 : Day
{
    public Day20() : base()
    {
    }

    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private long Solve(bool isPartB)
    {
        // For part A we don't have to multiply, since we also divided the input by 10
        var divide = isPartB ? 1 : 10;
        var multiply = isPartB ? 11 : 1;

        var input = int.Parse(Input) / divide;
        var houses = new int[input];

        var results = new List<int>();
        for (var elf = 1; elf < input; elf++)
        {
            var visits = 0;

            for (var house = elf; house < input; house += elf)
            {
                houses[house] += elf * multiply;

                if (houses[house] >= input)
                    results.Add(house);

                visits++;

                if (isPartB && visits == 50)
                    break;
            }
        }

        return results.Min();
    }
}
