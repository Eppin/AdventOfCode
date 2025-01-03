namespace AdventOfCode._2024;

public class Day9 : Day
{
    public Day9() : base()
    {
    }

    [Answer("1928", Example, Data = "2333133121414131402")]
    [Answer("6201130364722", Regular)]
    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    [Answer("2858", Example, Data = "2333133121414131402")]
    [Answer("6221662795602", Regular)]
    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private long Solve(bool isPartB)
    {
        var disk = Input.Select((c, i) => new { Index = i, Number = int.Parse($"{c}") });
        var id = 0;

        var list = new List<Map>();

        foreach (var file in disk)
        {
            if (file.Index % 2 == 0)
            {
                list.Add(new(file.Index, file.Number, id));
                id++;
            }
            else
            {
                list.Add(new(file.Index, file.Number, null));
            }
        }

        var listR = new List<Map>(list);
        listR.Reverse();

        long total = 0;
        var actualIndex = 0;

        foreach (var forward in list)
        {
            if (forward.Amount <= 0)
                continue;

            if (forward.Number != null)
            {
                for (var i = 0; i < forward.Amount; i++)
                {
                    total += actualIndex * forward.Number.Value;
                    actualIndex++;
                }
            }
            else
            {
                var needed = forward.Amount;

                if (isPartB)
                {
                    do
                    {
                        var backward = listR.FirstOrDefault(l => l.Number != null && needed >= l.Amount && forward.Index <= l.Index);

                        if (backward == null)
                        {
                            for (var i = 0; i < needed; i++)
                                actualIndex++; // Increase for empty space

                            break;
                        }

                        for (var i = 0; i < backward.Amount; i++)
                        {
                            total += actualIndex * backward.Number!.Value;
                            actualIndex++;
                        }

                        needed -= backward.Amount;
                        backward.Number = null;

                    } while (needed > 0);
                }
                else
                {
                    foreach (var backward in listR.Where(l => l is { Number: not null, Amount: > 0 } && forward.Index <= l.Index))
                    {
                        while (backward.Amount > 0 && needed > 0)
                        {
                            total += (actualIndex * backward.Number!.Value);
                            actualIndex++;

                            backward.Amount -= 1;
                            needed -= 1;
                        }

                        if (needed <= 0)
                            break;
                    }
                }
            }
        }

        return total;
    }

    private class Map(int index, int amount, int? number)
    {
        public int Index { get; } = index;
        public int Amount { get; set; } = amount;
        public int? Number { get; set; } = number;
    }
}
