using System.Diagnostics;
using System.Reflection;
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
        .OrderDescending()
        .ToList();

    days.Insert(0, -1);

    var chosenDay = Prompt.Select("Choose day", days, defaultValue: days.First(), textSelector: d => $"{(d == -1 ? "Run all" : $"{d}")}");

    // Run all puzzles
    if (chosenDay == -1)
    {
        var sw = Stopwatch.StartNew();
        foreach (var solve in puzzles.Where(p => p.Year == chosenYear).OrderBy(p => p.Day))
        {
            Solve(solve.Type, AdventOfCode.Models.Solve.A, true);
            Solve(solve.Type, AdventOfCode.Models.Solve.B, true);
            Console.WriteLine();
        }

        Console.WriteLine($"Total time elapsed: {sw.Elapsed:g}");
    }
    // Run a single puzzle
    else
    {
        var puzzle = puzzles.Single(p => p.Year == chosenYear && p.Day == chosenDay);
        var chosenSolve = Prompt.Select<Solve>("Choose which to run", defaultValue: AdventOfCode.Models.Solve.A);
        Solve(puzzle.Type, chosenSolve);
    }

    Console.ReadLine();
}

static int ParseString(string value)
{
    if (TryParse(new string(value.Where(char.IsDigit).ToArray()), out var result))
        return result;

    throw new DataException($"Can't parse [{value}]");
}

static void Solve(Type type, Solve solve, bool automatic = false)
{
    if (Activator.CreateInstance(type) is not Day day)
        throw new EvaluateException($"Can't create instance of [{type.Name}]");

    var answer = ChosenInput(day, solve, automatic);

    Console.WriteLine($"-- {type.Name}, {solve} --");
    if (answer == null)
    {
        Console.WriteLine("No input available.");
        return;
    }

    var sw = Stopwatch.StartNew();

    try
    {
        var result = day
            .Solve(solve, answer)
            .ToString();

        if (result == answer.Value)
        {
            Console.WriteLine($"{type.Name} is {result} in {sw.ElapsedMilliseconds} msec");
            return;
        }

        Console.WriteLine(string.IsNullOrWhiteSpace(answer.Value)
            ? $"{type.Name} is {result}, but expected is not given (for {answer.Input} puzzle) in {sw.ElapsedMilliseconds} msec"
            : $"{type.Name} is {result}, but expected {answer.Value} in {sw.ElapsedMilliseconds} msec");
    }
    catch (NotImplementedException )
    {
        Console.WriteLine("No implementation available.");
    }
}

static Answer? ChosenInput(Day day, Solve solve, bool automatic)
{
    var inputs = day.AvailableInputs(solve)
        .OrderBy(i => i.Input)
        .ThenBy(i => i.Value)
        .ToList();

    if (automatic)
        return inputs.SingleOrDefault(i => i.Input == Regular);

    return inputs.Count > 0
        ? Prompt.Select("Choose input to run", inputs, defaultValue: inputs.Last(), textSelector: TextSelector)
        : new Answer(string.Empty, Regular, null);
}

static string TextSelector(Answer answer)
{
    var extraData = !string.IsNullOrWhiteSpace(answer.Data)
        ? $", {answer.Data?[..4]}..."
        : string.Empty;

    return $"{answer.Input} ({answer.Value}{extraData})";
}
