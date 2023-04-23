using System.Data;
using System.Diagnostics;
using System.Reflection;
using AdventOfCode;
using Sharprompt;
using static System.Int32;

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

    puzzles.Add(new(puzzleYear, puzzleDay, dayType));
}

var years = puzzles
    .Select(p => p.Year)
    .Distinct()
    .OrderDescending()
    .ToList();

while (true)
{
    var chosenYear = Prompt.Select("Choose year", years, defaultValue: years.First());

    var days = puzzles
        .Where(p => p.Year == chosenYear)
        .Select(p => p.Day)
        .Distinct()
        .Order()
        .ToList();

    var chosenDay = Prompt.Select("Choose day", days, defaultValue: days.First());

    var puzzle = puzzles.Single(p => p.Year == chosenYear && p.Day == chosenDay);

    var chosenSolve = Prompt.Select<Solve>("Choose which to run", defaultValue: global::Solve.A);

    Solve(puzzle.Type, chosenSolve);

    Console.Read();
}

static int ParseString(string value)
{
    if (TryParse(new string(value.Where(char.IsDigit).ToArray()), out var result))
        return result;

    throw new DataException($"Can't parse [{value}]");
}

static void Solve(Type type, Solve solve)
{
    var day = Activator.CreateInstance(type) as Day;
    if (day == null)
        throw new EvaluateException($"Can't create instance of [{type.Name}]");

    Console.WriteLine($"-- {type.Name} --");
    var sw = Stopwatch.StartNew();

    var result = solve == global::Solve.A
        ? day.SolveA()
        : day.SolveB();

    Console.WriteLine($"{type.Name} is {result} in {sw.ElapsedMilliseconds} msec");
}

internal enum Solve
{
    A,
    B
}