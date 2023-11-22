namespace AdventOfCode;

using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public abstract partial class Day
{
    private readonly int _year;
    private readonly int _day;

    protected Day([CallerFilePath] string? filePath = null)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new InvalidDataException("Caller file path can't be empty");

        var regex = AdventOfCodeRegex();
        var matches = regex.Match(filePath);

        if (!matches.Success)
            throw new DataMisalignedException($"Caller file path needs to contain year and day, current value [{filePath}]");

        var year = matches.Groups[1].Value;
        var day = matches.Groups[2].Value;

        if (!int.TryParse(year, out _year) || !int.TryParse(day, out _day))
            throw new InvalidDataException($"Missing date [{year}/{day}]");
    }

    public abstract string SolveA();
    public abstract string SolveB();
    protected string Input => GetInput();
    protected IEnumerable<string> SplitInput => GetSplitInput();

    protected string[] GetSplitInput(bool removeEmptyEntries = true)
    {
        var options = removeEmptyEntries
            ? StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            : StringSplitOptions.TrimEntries;

        return GetInput().Split(Environment.NewLine, options);
    }
    
    private string GetInput()
    {
        var assembly = typeof(Day).Assembly.Location;
        var folder = Path.GetDirectoryName(assembly);

        if (string.IsNullOrWhiteSpace(folder))
            throw new InvalidOperationException();

        var file = Path.Combine(folder, $"{_year}", "Input", $"Day{_day}.txt");

        if (!File.Exists(file))
            throw new FileNotFoundException();

        return File.ReadAllText(file);
    }

    [GeneratedRegex(@"AdventOfCode.(\d{4}).Day(\d{1,2})\.")]
    private static partial Regex AdventOfCodeRegex();
}