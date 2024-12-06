namespace AdventOfCode._2021;

public class Day6 : Day
{
    public Day6() : base()
    {
    }

    [Answer("388739", Regular)]
    public override string SolveA()
    {
        return CountFish(80);
    }

    [Answer("1741362314973", Regular)]
    public override string SolveB()
    {
        /* TODO rewrite solution.
         * Use array of 9 position and count the amount of fishes per day
         * Day v	0	1	2	3	4	5	6	7	8	<- Numbers
         * 0	    0	1	1	2	1	0	0	0	0
         * 1	    1	1	2	1	0	0	0	0	0
         * 2	    1	2	1	0	0	0	1	0	1
         * 3	    2	1	0	0	0	1	1	1	1
         * 4	    1	0	0	0	1	1	3	1	2
         */
        throw new NotImplementedException("Current solution is way to slow...");
    }

    private string CountFish(int dayCount)
    {
        var numbers = GetNumbers()
            .Select(n => new Fish(n, false))
            .ToList();

        var days = 0;

        do
        {
            for (var i = 0; i < numbers.Count; i++)
            {
                if (numbers[i].IsNew)
                    continue;

                if (numbers[i].Count-- != 0)
                    continue;

                numbers[i].Count = 6;
                numbers.Add(new Fish(8, true));
            }

            foreach (var number in numbers)
                number.IsNew = false;

            days++;
        } while (days < dayCount);

        return $"{numbers.Count}";
    }

    private List<int> GetNumbers()
    {
        return Input
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(s => int.Parse($"{s}"))
            .ToList();
    }

    private class Fish
    {
        public int Count { get; set; }

        public bool IsNew { get; set; }

        public Fish(int count, bool isNew)
        {
            Count = count;
            IsNew = isNew;
        }

        public override string ToString()
        {
            return $"{Count}";
        }
    }
}
