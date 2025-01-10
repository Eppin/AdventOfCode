namespace AdventOfCode._2019;

public class Day1 : Day
{
    public Day1() : base()
    {
    }

    [Answer("34241", Example, Data = "12{nl}14{nl}1969{nl}100756")]
    [Answer("3279287", Regular)]
    public override object SolveA()
    {
        return Parse().Sum(Calculate);
    }

    [Answer("50346", Example, Data = "100756")]
    [Answer("4916076", Regular)]
    public override object SolveB()
    {
        var total = 0L;
        var queue = new Queue<int>();

        foreach (var mass in Parse())
            queue.Enqueue(mass);

        while (queue.TryDequeue(out var mass))
        {
            var fuel = Calculate(mass);
            if (fuel <= 0) continue;

            queue.Enqueue(fuel);
            total += fuel;
        }

        return total;
    }

    private static int Calculate(int mass) => mass / 3 - 2;

    private int[] Parse()
    {
        return GetSplitInput()
            .Select(int.Parse)
            .ToArray();
    }
}
