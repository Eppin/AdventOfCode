namespace AdventOfCode._2025;

public class Day3 : Day
{
    public Day3() : base()
    {
    }

    [Answer("357", Example, Data = "987654321111111{nl}811111111111119{nl}234234234234278{nl}818181911112111")]
    [Answer("17109", Regular)]
    public override object SolveA()
    {
        return Parse().Sum(bank => Generator(bank, 2));
    }

    [Answer("3121910778619", Example, Data = "987654321111111{nl}811111111111119{nl}234234234234278{nl}818181911112111")]
    [Answer("169347417057382", Regular)]
    public override object SolveB()
    {
        return Parse().Sum(bank => Generator(bank, 12));
    }

    private static long Generator(int[] bank, int batteryCount)
    {
        var lastIdx = 0;

        var batteries = new int[batteryCount];

        for (var bIdx = 0; bIdx < batteryCount; bIdx++)
        {
            // Always start searching from the index after the last found battery
            // Or from the start if it's the first battery
            var start = bIdx > 0 ? lastIdx + 1 : 0;

            // Adjust length to ensure enough elements remain for remaining batteries
            var length = bank.Length - (batteryCount - 1) + bIdx;

            for (var b = start; b < length; b++)
            {
                if (bank[b] <= batteries[bIdx]) continue;

                batteries[bIdx] = bank[b];
                lastIdx = b;
            }
        }

        return long.Parse(string.Join(string.Empty, batteries));
    }

    private int[][] Parse()
    {
        return SplitInput
            .Select(s => s
                .Select(c => int.Parse($"{c}"))
                .ToArray()
            ).ToArray();
    }
}
