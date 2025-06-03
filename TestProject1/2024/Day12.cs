using System.Text.RegularExpressions;
using TestProject1.Attributes;
using TestProject1.Models;

namespace TestProject1._2024;

public partial class Day12 : Day
{
    [Theory]
    [Answer("161", Puzzle.Example, Data = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))")]
    [Answer("167650499", Puzzle.Regular)]
    public void SolveA(string answer, Puzzle puzzle, string input)
    {
        var result = PartARegex()
            .Matches(input)
            .Select(m => Multiply(m.Groups))
            .Sum()
            .ToString();

        Assert.Equal(answer, result);
    }

    private static int Multiply(GroupCollection group)
    {
        var a = group[1];
        var b = group[2];

        if (!a.Success || !b.Success)
            throw new Exception();

        return int.Parse(a.ValueSpan) * int.Parse(b.ValueSpan);
    }

    private static int Multiply(GroupCollection group, ref bool? enabled)
    {
        if (group[0].Success && group[0].Value == "don't()")
        {
            enabled = false;
            return 0;
        }

        if (group[0].Success && group[0].Value == "do()")
        {
            enabled = true;
            return 0;
        }

        return enabled == true ? Multiply(group) : 0;
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex PartARegex();

    [GeneratedRegex(@"do\(\)|don't\(\)|mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex PartBRegex();
}
