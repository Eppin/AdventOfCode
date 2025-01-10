namespace AdventOfCode._2015;

using System.Security.Cryptography;

public class Day4 : Day
{
    public Day4() : base()
    {
    }

    [Answer("609043", Example, Data = "abcdef")]
    [Answer("254575", Regular)]
    public override object SolveA()
    {
        return FindMatch(5, Input);
    }

    [Answer("", Example, Data = "")]
    [Answer("1038736", Regular)]
    public override object SolveB()
    {
        return FindMatch(6, Input);
    }

    private static int FindMatch(int zeroes, string input)
    {
        var zeroesStr = "".PadLeft(zeroes, '0');
        
        var i = 0;
        while (true)
        {
            var md5 = CreateMD5($"{input}{i}");

            if (md5.StartsWith(zeroesStr))
                return i;

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