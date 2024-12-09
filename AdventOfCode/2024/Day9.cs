using System;

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
        return Solve().ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private long Solve()
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

        //Console.WriteLine();
        //Console.WriteLine();

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
                    //Console.WriteLine($"{forward.Index}, {forward.Amount}, {forward.Number}");
                    //Console.WriteLine($"{actualIndex}*{forward.Number} ({forward.Amount})");

                    total += (actualIndex * forward.Number.Value);
                    actualIndex++;
                }
            }
            else
            {
                var needed = forward.Amount;
                //Console.WriteLine($"\tBackwards, {needed}");

                foreach (var backward in listR.Where(l => l is { Number: not null, Amount: > 0 } && forward.Index <= l.Index))
                {
                    while (backward.Amount > 0 && needed > 0) // Break?!
                    {
                        //Console.WriteLine($"\tGrab: {backward.Number} (F:{forward.Index}, B:{backward.Index} of {map.Index}, beter: {list.IndexOf(backward)}");
                        //Console.WriteLine($"\t{actualIndex}*{backward.Number}");

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

        return total;
    }

    private class Map(int index, int amount, int? number)
    {
        public int Index { get; set; } = index;
        public int Amount { get; set; } = amount;
        public int? Number { get; set; } = number;
    }
}
