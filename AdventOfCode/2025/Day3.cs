using System.Collections.Generic;

namespace AdventOfCode._2025;

public class Day3 : Day
{
    public Day3() : base()
    {
    }

    //[Answer("", Example, Data = "987654321111111{nl}811111111111119{nl}234234234234278{nl}818181911112111")]
    [Answer("", Example, Data = "12345")]//"{nl}811111111111119{nl}234234234234278{nl}818181911112111")]
    //[Answer("", Regular)]
    public override object SolveA()
    {
        var banks = Parse();

        foreach (var bank in banks)
        {
            //Generate(bank, 0);
            Queueing(bank, 2);
        }

        return "";
    }

    private static void Generate(int[] bank, int depth)
    {
        //for (var i = 0; i < bank.Length; i++)
        //{
        //    var x = bank[(i + 1)..];
        //    Console.WriteLine($"i{i} ({i}-{depth}): {bank[i]} -> {string.Join(',', x)}");
        //    Generate(x, i + 1);
        //}
    }

    private static void Queueing(int[] bank, int length)
    {
        //const int length = 1; // numbers of batteries combined - 1 (take into account combining number)
        var queue = new Queue<State>();

        for (var i = 0; i < bank.Length; i++)
        {
            var state = new State(bank[i], bank[(i + 1)..], 0, "");
            Console.WriteLine($"Enqueue: {state.Value} -> {string.Join(',', state.Rest)}");

            //if (state.Rest.Length >= length) // Take into account length wanted
            queue.Enqueue(state);
        }

        while (queue.TryDequeue(out var state))
        {
            Console.WriteLine($"Value: {state.Value}, Rest: {string.Join(',', state.Rest)}, Length: {state.Length}, {state.Result}");

            for (int i = 0; i < state.Rest.Length; i++)
            {
                Console.WriteLine($"\tValue: {state.Rest[i]}, {string.Join(',', state.Rest[(i + 1)..])} -> {state.Value}");
                queue.Enqueue(new State(state.Rest[i], state.Rest[(i + 1)..], state.Length + 1, $"{state.Result}{state.Value}"));
            }
        }
    }

    private static int Combine(int[] values)
    {
        var result = 0;
        for (var i = 0; i < values.Length; i++)
        {
            result += values[i] * (i * 10);
        }

        return result;
    }

    private record State(int Value, int[] Rest, int Length, string Result);

    public override object SolveB()
    {
        throw new NotImplementedException();
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
