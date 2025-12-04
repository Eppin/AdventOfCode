namespace AdventOfCode._2025;

using System;
using System.Linq;

public class Day2 : Day
{
    public Day2() : base()
    {
    }

    [Answer("1227775554", Example, Data = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124")]
    [Answer("43952536386", Regular)]
    public override object SolveA()
    {
        var input = Parse();
        long answer = 0;

        foreach (var (startStr, endStr) in input)
        {
            var start = long.Parse(startStr);
            var end = long.Parse(endStr);

            for (var i = start; i <= end; i++)
            {
                var numbers = Numbers(i);

                if (numbers.Count % 2 != 0) continue;

                var a = numbers.Take(numbers.Count / 2);
                var b = numbers.Skip(numbers.Count / 2);

                if (a.SequenceEqual(b))
                    answer += i;
            }
        }

        return answer;
    }

    [Answer("4174379265", Example, Data = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124")]
    [Answer("54486209192", Regular)]
    public override object SolveB()
    {
        var input = Parse();
        long answer = 0;

        foreach (var (startStr, endStr) in input)
        {
            var start = long.Parse(startStr);
            var end = long.Parse(endStr);

            for (var i = start; i <= end; i++)
            {
                if (IsInvalid(i))
                    answer += i;
            }
        }

        return answer;
    }

    private static bool IsInvalid(long value)
    {
        var numbers = Numbers(value);

        if (numbers.Count > 1 && numbers.All(n => n == numbers[0]))
        {
            // All numbers are the same and length at least 2
            return true;
        }

        var span = (ReadOnlySpan<long>)numbers.ToArray();
        var length = span.Length;

        // Check for repeating patterns, but skip the first 2 digits
        for (var i = 2; i <= (length / 2) + 1; i++)
        {
            var pattern = span[..i];
            var success = false;

            for (var j = pattern.Length; j < length; j += pattern.Length)
            {
                // If remaining slice is smaller than pattern, it can't match (mirrors original behavior)
                if (j + pattern.Length > length)
                {
                    success = false;
                    break;
                }

                var slice = span.Slice(j, pattern.Length);

                if (!pattern.SequenceEqual(slice))
                {
                    success = false;
                    break;
                }

                success = true;
            }

            if (success) return true;
        }

        return false;
    }

    private static List<long> Numbers(long value)
    {
        var results = new List<long>();

        var start = value;
        var count = 0;

        do { count++; }
        while ((value /= 10) >= 1);

        for (var i = 0; i < count; i++)
        {
            var next = start / 10;
            var current = start % 10;

            results.Add(current);

            start = next;
        }

        return results;
    }

    private List<(string Start, string End)> Parse()
    {
        return Input
            .Split(',')
            .Select(s =>
            {
                var range = s.Split('-');
                return (range[0], range[1]);
            }).ToList();
    }
}
