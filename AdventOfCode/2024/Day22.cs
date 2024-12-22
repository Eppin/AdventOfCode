namespace AdventOfCode._2024;

public class Day22 : Day
{
    public Day22() : base()
    {
    }

    [Answer("37327623", Example, Data = "1{nl}10{nl}100{nl}2024")]
    [Answer("13429191512", Regular)]
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
        var secrets = Parse();
        var total = 0L;

        foreach (var secret in secrets)
        {
            var tmp = secret;
            for (var i = 0; i < 2000; i++)
                tmp = Next(tmp);

            total += tmp;
        }

        return total;
    }

    private static long Next(long secret)
    {
        // Step 1
        var mul64 = secret * 64;
        secret = Mix(mul64, secret);
        secret = Prune(secret);

        // Step 2
        var div32 = secret / 32;
        secret = Mix(div32, secret);
        secret = Prune(secret);

        // Step 3
        var mul2048 = secret * 2048;
        secret = Mix(mul2048, secret);
        secret = Prune(secret);

        return secret;
    }

    private static long Mix(long value, long secret) => value ^ secret;
    private static long Prune(long secret) => secret % 16777216;

    private List<long> Parse()
    {
        return GetSplitInput()
            .Select(long.Parse)
            .ToList();
    }
}
