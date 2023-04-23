namespace AdventOfCode._2015;

using System.Collections.Concurrent;
using System.Security.Cryptography;

public class Day4 : Day
{
    public Day4() : base()
    {
    }

    public override string SolveA()
    {
        return FindMatch(5, Input);
    }

    public override string SolveB()
    {
        return FindMatch(6, Input);
    }

    private static string FindMatch(int zeroes, string input)
    {
        var zeroesStr = "".PadLeft(zeroes, '0');
        
        var i = 0;
        while (true)
        {
            var md5 = CreateMD5($"{input}{i}");

            if (md5.StartsWith(zeroesStr))
                return i.ToString();

            i++;
        }
    }

    private static string CreateMD5(string input)
    {
        using var md5 = MD5.Create();
        var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes); // .NET 5 +
    }
}