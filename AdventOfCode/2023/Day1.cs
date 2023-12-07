using System.Text.RegularExpressions;

namespace AdventOfCode._2023;

public partial class Day1 : Day
{
    private readonly Dictionary<string, int> _numbers = new()
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 },
    };

    public Day1() : base()
    {
    }

    public override string SolveA()
    {
        return Solve(false).ToString();
    }

    public override string SolveB()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool isPartB)
    {
        var sum = 0;

        foreach (var line in SplitInput)
        {
            if (isPartB)
            {
                var t2 =PartBRegex().Matches(line).Select(m => m.Groups.);
            }
            
            var regex = isPartB ? PartBRegex() : PartARegex();
            var matches = regex.Matches(line);

            if (!matches.Any())
                continue;

            var first = "";
            var last = "";

            if (matches.Count == 1)
            {
                first = last = matches.First().Value;
            }
            else if (matches.Count >= 2)
            {
                first = matches.First().Value;
                last = matches.Last().Value;
            }

            // sum += int.Parse($"{first}{last}");

            if (!int.TryParse(first, out var firstNr))
                _numbers.TryGetValue(first, out firstNr);

            if (!int.TryParse(last, out var lastNr))
                _numbers.TryGetValue(last, out lastNr);

            Console.WriteLine($"{first} - {last} => {firstNr} {lastNr}");
            sum += (firstNr * 10) + lastNr;
        }

        return sum;
    }

    [GeneratedRegex("(\\d)")]
    private static partial Regex PartARegex();

    [GeneratedRegex("(?=(\\d|one|two|three|four|five|six|seven|eight|nine))")]
    private static partial Regex PartBRegex();
}