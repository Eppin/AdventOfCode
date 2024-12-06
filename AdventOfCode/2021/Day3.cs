namespace AdventOfCode._2021;

public class Day3 : Day
{
    public Day3() : base()
    {
    }

    [Answer("3009600", Regular)]
    public override string SolveA()
    {
        var flippedInput = FlipArray();

        var gamma = string.Empty;
        var epsilon = string.Empty;

        foreach (var values in flippedInput)
        {
            var count0 = values.Count(c => c.Equals(0));
            var count1 = values.Count(c => c.Equals(1));

            if (count1 > count0)
            {
                gamma += 1;
                epsilon += 0;
            }
            else if (count0 > count1)
            {
                gamma += 0;
                epsilon += 1;
            }
            else
                throw new DataException();
        }

        var g = gamma
            .Select(x => x)
            .Reverse()
            .Select((x, i) => int.Parse($"{x}") * Math.Pow(2, i))
            .Sum();

        var e = epsilon
            .Select(x => x)
            .Reverse()
            .Select((x, i) => int.Parse($"{x}") * Math.Pow(2, i))
            .Sum();

        return $"{g * e}";
    }

    [Answer("6940518", Regular)]
    public override string SolveB()
    {
        var input = GetSplitInput();

        var oxygen = input
            .Select(x => x
                .Select(y => int.Parse($"{y}"))
                .ToArray())
            .ToList();

        var co2Scrubber = input
            .Select(x => x
                .Select(y => int.Parse($"{y}"))
                .ToArray())
            .ToList();

        var length = input[0].Length;

        RemoveUnwanted(length, oxygen, true);
        RemoveUnwanted(length, co2Scrubber, false);

        var c = oxygen
            .Where(x => x != null)
            .SelectMany(x => x)
            .Reverse()
            .Select((x, i) => int.Parse($"{x}") * Math.Pow(2, i))
            .Sum();

        var o = co2Scrubber
            .Where(x => x != null)
            .SelectMany(x => x)
            .Reverse()
            .Select((x, i) => int.Parse($"{x}") * Math.Pow(2, i))
            .Sum();

        return $"{o * c}";
    }

    private static void RemoveUnwanted(int length, List<int[]?> someList, bool isOxygen)
    {
        var _1 = isOxygen
            ? 1
            : 0;

        var _0 = isOxygen
            ? 0
            : 1;

        for (var i = 0; i < length; i++)
        {
            if (someList.Count(x => x != null) <= 1)
                break;

            var count1 = 0;
            var count0 = 0;

            foreach (var t in someList)
            {
                if (t == null) continue;

                if (t[i].Equals(1))
                    count1++;
                else
                    count0++;
            }

            for (var j = 0; j < someList.Count; j++)
            {
                if (someList[j] == null) continue;

                // Remove 0's
                if (count1 < count0 && someList[j][i].Equals(_1))
                    someList[j] = null;
                else if (count0 < count1 && someList[j][i].Equals(_0))
                    someList[j] = null;
                else if (count0 == count1 && someList[j][i].Equals(_0))
                    someList[j] = null;
            }
        }
    }

    private int[][] FlipArray()
    {
        var splittedInput = GetSplitInput()
            .Select(x => x
                .Select(y => int.Parse($"{y}"))
                .ToArray())
            .ToArray();

        var output = new int[splittedInput[0].Length][];

        for (var i = 0; i < splittedInput[0].Length; i++)
        {
            var flipped = new int[splittedInput.Length];

            for (var j = 0; j < splittedInput.Length; j++)
                flipped[j] = splittedInput[j][i];

            output[i] = flipped;
        }

        return output;
    }
}
