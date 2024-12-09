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

    [Answer("2858", Example, Data = "2333133121414131402")]
    [Answer("6221662795602", Regular)]
    public override string SolveB()
    {
        return Solve3().ToString();
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

    private long Solve2()
    {
        var disk = Input.Select((c, i) => new { Index = i, Number = int.Parse($"{c}") });
        var id = 0;

        var list = new List<Map>();
        var list2 = new List<Map>();
        var listR = new List<Map>();

        foreach (var file in disk)
        {
            if (file.Index % 2 == 0)
            {
                list.Add(new(file.Index, file.Number, id));
                list2.Add(new(file.Index, file.Number, id));
                listR.Add(new(file.Index, file.Number, id));
                id++;
            }
            else
            {
                list.Add(new(file.Index, file.Number, null));
                list2.Add(new(file.Index, file.Number, null));
                listR.Add(new(file.Index, file.Number, null));
            }
        }

        // var listR = new List<Map>();
        listR.AddRange(list);
        listR.Reverse();

        //Console.WriteLine();
        //Console.WriteLine();

        long total = 0;
        // var actualIndex = 0;

        var movable = new List<(int From, Map To)>();

        foreach (var backward in listR.Where(l => l.Number.HasValue))
        {
            // Console.WriteLine($"Backward, {backward.Index}: {backward.Number} ({backward.Amount})");

            var forward = list.FirstOrDefault(l => l.Number == null && l.Amount >= backward.Amount && backward.Index >= l.Index); // && l.Amount >= backward.Amount);
            if (forward == null)
            {
                // Console.WriteLine($"Forward is empty, {backward.Number}");
                continue;
            }

            movable.Add((backward.Index, new(forward.Index, backward.Amount, backward.Number)));

            // Console.WriteLine($"Backward {backward.Number} ({backward.Index}) can be moved to {forward.Number} ({forward.Index})");
            forward.Amount -= backward.Amount;
        }

        // Console.WriteLine();
        // Console.WriteLine("Movable:");

        // foreach (var (from, map) in movable)
        // {
        //     Console.WriteLine($"From Index {from} to -> {map.Index}, {map.Number} ({map.Amount})");
        // }

        // Console.WriteLine();
        // Console.WriteLine("Origina:");
        //
        // foreach (var map in list2)
        // {
        //     Console.WriteLine($"{map.Index}, {map.Number} ({map.Amount})");
        //
        //     var moves = movable.Where(m => m.To.Index == map.Index);
        //     foreach (var (fromIndex, replace) in moves)
        //     {
        //         Console.WriteLine($"From Index {fromIndex} to {replace.Index} {replace.Number} ({replace.Amount})");
        //     }
        // }

        // Console.WriteLine();

        var actualIndex = 0;
        for (var i = 0; i < list2.Count; i++)
        {
            var map = list2[i];

            // if (map.Number == null)
            //     continue;

            //

            var moves = movable.Where(m => m.To.Index == map.Index).ToList();

            if (moves.Count == 0)
            {
                for (var j = 0; j < map.Amount; j++)
                {
                    // Console.WriteLine($"{map.Index}, {map.Number} ({map.Amount}) == {actualIndex}*{map.Number}");

                    if (map.Number != null)
                        total += actualIndex * map.Number.Value;

                    actualIndex++;
                }
            }

            var skip = 0;
            var x = false;
            foreach (var (fromIndex, replace) in moves)
            {
                // Console.WriteLine($"From Index {fromIndex} to {replace.Index} {replace.Number} ({replace.Amount})");

                for (var j = 0; j < replace.Amount; j++)
                {
                    // Console.WriteLine($"\t{replace.Index}, {replace.Number} ({replace.Amount}) == {actualIndex}*{replace.Number}");

                    total += actualIndex * replace.Number!.Value;

                    actualIndex++;
                    skip++;
                    x = true;
                }

                // Console.WriteLine($"{map.Amount} vs {replace.Amount}");

                var change = list2.FirstOrDefault(l => l.Index == fromIndex);
                if (change != null) change.Number = null;
            }

            if (x)
            {
                // Console.WriteLine($"{map.Amount} vs {skip}");
                actualIndex += (map.Amount - skip);
            }
        }

        return total;
    }

    private long Solve3()
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
                    // Console.WriteLine($"{forward.Index}, {forward.Amount}, {forward.Number}");
                    //Console.WriteLine($"{actualIndex}*{forward.Number} ({forward.Amount})");

                    total += actualIndex * forward.Number.Value;
                    actualIndex++;
                }
            }
            else
            {
                var needed = forward.Amount;
                //Console.WriteLine($"\tBackwards, {needed}");

                do
                {
                    var backward = listR.FirstOrDefault(l => l.Number != null && needed >= l.Amount && forward.Index <= l.Index);

                    if (backward == null)
                    {
                        for (int i = 0; i < needed; i++)
                        {
                            // Console.WriteLine($"Insert missing .?! -> {needed}, gift: {i + 1}"); // TODO compensate for when we replaced it with a smaller amount!
                            actualIndex++;
                        }

                        break;
                    }

                    for (int i = 0; i < backward.Amount; i++)
                    {
                        // Console.WriteLine($"\tGrab: {backward.Number} * {actualIndex}");
                        total += actualIndex * backward.Number!.Value;
                        actualIndex++;
                    }

                    // while (backward.Amount > 0 && needed > 0) // Break?!
                    // {
                    //     Console.WriteLine($"\tGrab: {backward.Number} (F:{forward.Index}, B:{backward.Index}, beter: {list.IndexOf(backward)}");
                    //     //Console.WriteLine($"\t{actualIndex}*{backward.Number}");
                    //
                    //     total += (actualIndex * backward.Number!.Value);
                    //     actualIndex++;
                    //
                    //     backward.Amount -= 1;
                    needed -= backward.Amount;
                    backward.Number = null;
                    // }
                    //
                    // if (needed <= 0)
                    //     break;
                } while (needed > 0);
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
