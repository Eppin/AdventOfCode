namespace AdventOfCode._2024;

public partial class Day3 : Day
{
    public Day3() : base()
    {
    }

    [Answer("161", Example, Data = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))")]
    [Answer("167650499", Regular)]
    public override object SolveA()
    {
        return PartARegex()
            .Matches(Input)
            .Select(m => Multiply(m.Groups))
            .Sum()
            .ToString();
    }

    [Answer("48", Example, Data = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))")]
    [Answer("95846796", Regular)]
    public override object SolveB()
    {
        bool? enabled = true;

        return PartBRegex()
            .Matches(Input)
            .Select(m => Multiply(m.Groups, ref enabled))
            .Sum()
            .ToString();
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
