using System.Text.RegularExpressions;

namespace TestProject1;

public abstract partial class Day
{
    private readonly int _year;
    private readonly int _day;

    protected Day()
    {
        var className = base.ToString();
        if (string.IsNullOrWhiteSpace(className))
            throw new InvalidDataException("Class name can't be empty");

        var matches = AdventOfCodeRegex().Match(className);

        if (!matches.Success)
            throw new DataMisalignedException($"Caller file path needs to contain year and day, current value [{className}]");

        var year = matches.Groups[1].Value;
        var day = matches.Groups[2].Value;

        if (!int.TryParse(year, out _year) || !int.TryParse(day, out _day))
            throw new InvalidDataException($"Missing date [{year}/{day}]");
    }

    //[Fact]
    //public abstract void SolveA();
    //public abstract void SolveB(string expected, object? data);

    [GeneratedRegex(@"TestProject1._(\d{4}).Day(\d{1,2})")] // TODO: AdventOfCode
    private static partial Regex AdventOfCodeRegex();
}
