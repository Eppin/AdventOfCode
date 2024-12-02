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
            var regex = isPartB ? PartBRegex() : PartARegex();
            var matches = regex
                .Matches(line)
                // Skip the first group because it always contains the entire match
                .SelectMany(m => m.Groups.Cast<Group>().Skip(1))
                .ToList();

            if (matches.Count == 0)
                continue;

            var first = "";
            var last = "";

            switch (matches.Count)
            {
                case 1:
                    first = last = matches.First().Value;
                    break;

                case >= 2:
                    first = matches.First().Value;
                    last = matches.Last().Value;
                    break;
            }

            if (!int.TryParse(first, out var firstNr))
                _numbers.TryGetValue(first, out firstNr);

            if (!int.TryParse(last, out var lastNr))
                _numbers.TryGetValue(last, out lastNr);

            sum += firstNr * 10 + lastNr;
        }

        return sum;
    }

    [GeneratedRegex("(\\d)")]
    private static partial Regex PartARegex();

    [GeneratedRegex("(?=(\\d|one|two|three|four|five|six|seven|eight|nine))")]
    private static partial Regex PartBRegex();
}
