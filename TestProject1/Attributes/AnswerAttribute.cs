using System.Reflection;
using System.Text.RegularExpressions;
using TestProject1.Models;
using Xunit.Sdk;

namespace TestProject1.Attributes;

public partial class AnswerAttribute(string answer, Puzzle puzzle) : DataAttribute
{
    public string? Data { get; set; }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        var (year, day) = GetDate(testMethod.DeclaringType?.ToString());
        var input = GetInput(year, day);

        return [[answer, puzzle, input]];
    }

    private static (int Year, int Day) GetDate(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidDataException("Class name can't be empty");

        var matches = AdventOfCodeRegex().Match(name);

        if (!matches.Success)
            throw new DataMisalignedException($"Caller file path needs to contain year and day, current value [{name}]");

        var extractedYear = matches.Groups[1].Value;
        var extractedDay = matches.Groups[2].Value;

        if (!int.TryParse(extractedYear, out var year) || !int.TryParse(extractedDay, out var day))
            throw new InvalidDataException($"Missing date [{extractedYear}/{extractedDay}]");

        return (year, day);
    }

    private string GetInput(int year, int day)
    {
        var assembly = typeof(AnswerAttribute).Assembly.Location;
        var folder = Path.GetDirectoryName(assembly);

        if (string.IsNullOrWhiteSpace(folder))
            throw new InvalidOperationException();

        if (!string.IsNullOrWhiteSpace(Data))
            return Data.Replace("{nl}", Environment.NewLine);

        var path = Path.Combine(folder, $"{year}", "Input", $"Day{day}.txt");

        if (!File.Exists(path))
            throw new FileNotFoundException("Input puzzle file not found", path);

        return File.ReadAllText(path);
    }

    [GeneratedRegex(@"TestProject1._(\d{4}).Day(\d{1,2})")] // TODO: AdventOfCode
    private static partial Regex AdventOfCodeRegex();
}
