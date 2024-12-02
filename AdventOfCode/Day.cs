namespace AdventOfCode;

using System.Reflection;
using System.Runtime.CompilerServices;
using static Solve;

public abstract partial class Day
{
    private readonly int _year;
    private readonly int _day;

    private Input _input = Regular;
    private Solve _solve = A;

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

    public (string Answer, string? Expected) Solve(Solve solve, Input input)
    {
        _solve = solve;
        _input = input;

        var result = solve == A
            ? SolveA()
            : SolveB();

        return (result, AnswerAttribute(solve)?.Answer);
    }

    public IEnumerable<Input> AvailableInputs(Solve solve)
    {
        return GetType()
            .GetMethod($"Solve{solve}")
            ?.GetCustomAttributes<AnswerAttribute>()
            .Select(a => a.Input)
            .Distinct() ?? [];
    }

    protected string Input => GetInput();
    protected IEnumerable<string> SplitInput => GetSplitInput();

    protected string[] GetSplitInput(bool removeEmptyEntries = true)
    {
        var splitOptions = StringSplitOptions.TrimEntries;
        if (removeEmptyEntries) splitOptions |= StringSplitOptions.RemoveEmptyEntries;

        return GetInput().Split(Environment.NewLine, splitOptions);
    }

    private string GetInput()
    {
        var assembly = typeof(Day).Assembly.Location;
        var folder = Path.GetDirectoryName(assembly);

        if (string.IsNullOrWhiteSpace(folder))
            throw new InvalidOperationException();

        var answer = AnswerAttribute(_solve);
        if (!string.IsNullOrWhiteSpace(answer?.Data))
            return answer.Data.Replace("{nl}", Environment.NewLine);

        var path = Path.Combine(folder, $"{_year}", "Input", $"Day{_day}.txt");

        if (!File.Exists(path))
            throw new FileNotFoundException("Puzzle input file not found", path);

        return File.ReadAllText(path);
    }

    private AnswerAttribute? AnswerAttribute(Solve solve)
    {
        var answers = GetType()
            .GetMethod($"Solve{solve}")
            ?.GetCustomAttributes<AnswerAttribute>()
            .ToList();

        if (answers == null || !answers.Any())
            return null;

        return answers.SingleOrDefault(a => a.Input == _input);
    }

    [GeneratedRegex(@"AdventOfCode.(\d{4}).Day(\d{1,2})\.")]
    private static partial Regex AdventOfCodeRegex();
}
