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
        var secrets = Parse();
        var total = 0L;

        foreach (var secret in secrets)
        {
            var tmp = secret;
            for (var i = 0; i < 2000; i++)
                tmp = Next(tmp);

            total += tmp;
        }

        return total.ToString();
    }

    [Answer("23", Example, Data = "1{nl}2{nl}3{nl}2024")]
    [Answer("1582", Regular)]
    public override string SolveB()
    {
        var secrets = Parse();
        
        // Dictionary with changes as key, nested dictionary with secret as key and price as value
        var changes = new Dictionary<string, Dictionary<long, long>>();

        foreach (var secret in secrets)
        {
            var tmp = secret;
            var price = tmp % 10;

            long? change1 = null;
            long? change2 = null;
            long? change3 = null;

            for (var i = 0; i < 1999; i++)
            {
                tmp = Next(tmp);

                var tmpPrice = tmp % 10;
                var change = tmp % 10 - price;
                price = tmpPrice;

                if (change1 != null && change2 != null && change3 != null)
                {
                    var changeStr = $"{change1.Value},{change2.Value},{change3.Value},{change}";
                    if (!changes.TryAdd(changeStr, new Dictionary<long, long> { [secret] = tmpPrice }))
                    {
                        if (!changes[changeStr].ContainsKey(secret))
                            changes[changeStr].Add(secret, tmpPrice);
                    }
                }

                change1 = change2;
                change2 = change3;
                change3 = change;
            }
        }

        return changes
            .MaxBy(x => x.Value.Sum(s => s.Value)).Value
            .Sum(s => s.Value)
            .ToString();
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
