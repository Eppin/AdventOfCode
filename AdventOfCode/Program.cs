using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using AdventOfCode;
using Sharprompt;
using static System.Int32;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.CancelKeyPress += delegate
{
    Console.WriteLine("Closing...");
};

Console.WriteLine("Hello, World!");

var puzzles = new List<Puzzle>();

var dayTypes = Assembly
    .GetExecutingAssembly()
    .GetTypes()
    .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(Day)));

foreach (var dayType in dayTypes)
{
    if (string.IsNullOrWhiteSpace(dayType.FullName))
        continue;

    var split = dayType.FullName.Split('.');

    if (split.Length < 3)
        continue;

    var puzzleYear = ParseString(split[1]);
    var puzzleDay = ParseString(split[2]);

    puzzles.Add(new Puzzle(puzzleYear, puzzleDay, dayType));
}

// Add option at bottom to add year?!
var years = puzzles
    .Select(p => p.Year)
    .Distinct()
    .OrderDescending()
    .ToList();

years.Add(0);


while (true)
{
    var chosenYear = Prompt.Select("Choose year", years, defaultValue: years.First(), textSelector: y => $"{(y == 0 ? "Add year" : $"{y}")}");

    // Add year
    if (chosenYear == 0)
    {
        var newYear = AddYear();

        if (newYear != null)
        {
            await AddDay(newYear.Value);
            Console.WriteLine("Recompile and start again!");
            return;
        }

        continue;
    }

    // Add option at bottom to add day?!
    var days = puzzles
        .Where(p => p.Year == chosenYear)
        .Select(p => p.Day)
        .Distinct()
        .OrderDescending()
        .ToList();

    days.Add(0);

    var chosenDay = Prompt.Select("Choose day", days, defaultValue: days.First(), textSelector: d => $"{(d == 0 ? "Add day" : $"{d}")}");

    // Add day
    if (chosenDay == 0)
    {
        await AddDay(chosenYear);
        Console.WriteLine("Recompile and start again!");
        continue;
    }

    var puzzle = puzzles.Single(p => p.Year == chosenYear && p.Day == chosenDay);

    var chosenSolve = Prompt.Select<Solve>("Choose which to run", defaultValue: AdventOfCode.Models.Solve.A);

    Solve(puzzle.Type, chosenSolve);
    Console.ReadLine();
}

static int ParseString(string value)
{
    if (TryParse(new string(value.Where(char.IsDigit).ToArray()), out var result))
        return result;

    throw new DataException($"Can't parse [{value}]");
}

static void Solve(Type type, Solve solve)
{
    if (Activator.CreateInstance(type) is not Day day)
        throw new EvaluateException($"Can't create instance of [{type.Name}]");

    var (input, expected) = ChosenInput(day, solve);

    Console.WriteLine($"-- {type.Name} --");
    var sw = Stopwatch.StartNew();

    var result = day
        .Solve(solve, input, expected)
        .ToString();

    if (result == expected)
    {
        Console.WriteLine($"{type.Name} is {result} in {sw.ElapsedMilliseconds} msec");
        return;
    }

    Console.WriteLine(string.IsNullOrWhiteSpace(expected)
        ? $"{type.Name} is {result}, but expected is not given (for {input} puzzle) in {sw.ElapsedMilliseconds} msec"
        : $"{type.Name} is {result}, but expected {expected} in {sw.ElapsedMilliseconds} msec");
}

static (Input Input, string Answer) ChosenInput(Day day, Solve solve)
{
    var inputs = day.AvailableInputs(solve)
        .OrderBy(i => i.Input)
        .ThenBy(i => i.Answer)
        .ToList();

    return inputs.Count > 0
        ? Prompt.Select("Choose input to run", inputs, defaultValue: inputs.Last(), textSelector: (i) => $"{i.Input} ({i.Answer})")
        : (Regular, string.Empty);
}

static int? AddYear([CallerFilePath] string? filePath = null)
{
    var dir = Path.GetDirectoryName(filePath);
    if (!Directory.Exists(dir))
    {
        Console.WriteLine("Somehow the directory doesn't exist");
        return null;
    }

    int year;

    do
    {
        year = Prompt.Input<int>("Add year", DateTime.Now.Year);
    } while (year < 2015 || year > DateTime.Now.Year);

    var yearDir = Path.Combine(dir, $"{year}");

    if (Directory.Exists(yearDir))
    {
        Console.WriteLine($"The year {year} directory already exists");
        return null;
    }

    Directory.CreateDirectory(yearDir);
    Directory.CreateDirectory(Path.Combine(yearDir, "Input"));

    return year;
}

static async Task AddDay(int year, [CallerFilePath] string? filePath = null)
{
    int day;

    do
    {
        day = Prompt.Input<int>("Add day");
    } while (day is < 1 or > 31);

    var dir = Path.GetDirectoryName(filePath);

    if (string.IsNullOrWhiteSpace(dir))
    {
        Console.WriteLine("Somehow the current dir is not available");
        return;
    }

    // Download input from AoC
    if (!await DownloadDay(year, day, dir)) return;

    var yearDir = Path.Combine(dir, $"{year}");
    var templateDir = Path.Combine(dir, "Templates");

    var templateContent = File.ReadAllText(Path.Combine(templateDir, "Day.template.csx"));
    var dayContent = templateContent
        .Replace("{YEAR}", $"{year}")
        .Replace("{DAY}", $"{day}");

    File.WriteAllText(Path.Combine(yearDir, $"Day{day}.cs"), dayContent);
}

static async Task<bool> DownloadDay(int year, int day, string dir)
{
    try
    {
        var cookie = Prompt.Input<string>("Enter cookie");
        if (string.IsNullOrWhiteSpace(cookie))
        {
            Console.WriteLine("Cookie can't be null and/or empty");
            return false;
        }

        var baseAddress = new Uri("https://adventofcode.com/");

        var cookieContainer = new CookieContainer();
        cookieContainer.Add(baseAddress, new Cookie("session", cookie));

        using var handler = new HttpClientHandler { CookieContainer = cookieContainer };
        using var client = new HttpClient(handler) { BaseAddress = baseAddress };

        var response = await client.GetAsync($"{year}/day/{day}/input");
        response.EnsureSuccessStatusCode();

        var input = await response.Content.ReadAsStringAsync();
        File.WriteAllText(Path.Combine(dir, $"{year}", "Input", $"Day{day}.txt"), input);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return false;
    }

    return true;
}