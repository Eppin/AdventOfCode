namespace AdventOfCode._2019;

public class Day2 : Day
{
    public Day2() : base()
    {
    }

    [Answer("3500", Example, Data = "1,9,10,3,2,3,11,0,99,30,40,50")]
    [Answer("4484226", Regular)]
    public override object SolveA()
    {
        var codes = Parse();

        if (!IsExample)
        {
            codes[1] = 12;
            codes[2] = 2;
        }

        for (var i = 0; i < codes.Length; i += 4)
        {
            var opcode = codes[i];

            if (opcode == 99)
                break;

            var pos1 = codes[i + 1];
            var pos2 = codes[i + 2];

            var output = codes[i + 3];

            codes[output] = opcode switch
            {
                1 => codes[pos1] + codes[pos2],
                2 => codes[pos1] * codes[pos2],
                _ => throw new InvalidDataException($"Unknown opcode {opcode} at position {i}")
            };
        }

        return codes[0];
    }

    [Answer("5696", Regular)]
    public override object SolveB()
    {
        foreach (var (newPos1, newPos2) in Pairs())
        {
            var codes = Parse();

            codes[1] = newPos1;
            codes[2] = newPos2;

            for (var i = 0; i < codes.Length; i += 4)
            {
                var opcode = codes[i];

                if (opcode == 99)
                    break;

                var pos1 = codes[i + 1];
                var pos2 = codes[i + 2];

                var output = codes[i + 3];

                codes[output] = opcode switch
                {
                    1 => codes[pos1] + codes[pos2],
                    2 => codes[pos1] * codes[pos2],
                    _ => throw new InvalidDataException($"Unknown opcode {opcode} at position {i}")
                };
            }

            if (codes[0] == 19690720)
                return $"{100 * newPos1 + newPos2}";

        }

        throw new InvalidDataException("No solution found");
    }

    private static IEnumerable<(int, int)> Pairs()
    {
        for (var i = 0; i < 99; i++)
        {
            for (var j = 0; j < 99; j++)
            {
                yield return (i, j);
            }
        }
    }

    // Cache parsed codes to avoid re-parsing for each attempt in SolveB
    private int[]? _cachedCodes;

    private int[] Parse()
    {
        _cachedCodes ??= Input.Split(',')
            .Select(int.Parse)
            .ToArray();

        return (int[])_cachedCodes.Clone();
    }
}
