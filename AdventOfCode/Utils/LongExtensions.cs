namespace AdventOfCode.Utils;

public static class LongExtensions
{
    public static List<long> Split(this long n, int length = 1)
    {
        var ints = new List<long>();
        var splitBy = (int)Math.Pow(10, length);

        while (n > 0)
        {
            ints.Add(n % splitBy);
            n /= splitBy;
        }

        ints.Reverse();
        return ints;
    }

    public static int Length(this long n)
    {
        if (n >= 0)
        {
            if (n < 10) return 1;
            if (n < 100) return 2;
            if (n < 1000) return 3;
            if (n < 10000) return 4;
            if (n < 100000) return 5;
            if (n < 1000000) return 6;
            if (n < 10000000) return 7;
            if (n < 100000000) return 8;
            if (n < 1000000000) return 9;
            if (n < 10000000000) return 10;
            if (n < 100000000000) return 11;
            if (n < 1000000000000) return 12;

            throw new Exception($"Int value [{n}] is not in range");
        }

        if (n > -10) return 2;
        if (n > -100) return 3;
        if (n > -1000) return 4;
        if (n > -10000) return 5;
        if (n > -100000) return 6;
        if (n > -1000000) return 7;
        if (n > -10000000) return 8;
        if (n > -100000000) return 9;
        if (n > -1000000000) return 10;

        throw new Exception($"Int value [{n}] is not in range");
    }
}
