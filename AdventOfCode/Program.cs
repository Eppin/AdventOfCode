﻿using System.Collections.ObjectModel;
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

    var chosenDay = Prompt.Select("Choose day", days, defaultValue: days.First());

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

    var chosenInput = ChosenInput(day, solve);

    Console.WriteLine($"-- {type.Name} --");
    var sw = Stopwatch.StartNew();

    var (result, expected) = day.Solve(solve, chosenInput);

    if (result == expected)
    {
        Console.WriteLine($"{type.Name} is {result} in {sw.ElapsedMilliseconds} msec");
        return;
    }

    Console.WriteLine(string.IsNullOrWhiteSpace(expected)
        ? $"{type.Name} is {result}, but expected is not given (for {chosenInput} puzzle) in {sw.ElapsedMilliseconds} msec"
        : $"{type.Name} is {result}, but expected {expected} in {sw.ElapsedMilliseconds} msec");
}

static Input ChosenInput(Day day, Solve solve)
{
    Collection<Input> inputs = [];
    if (day.AvailableInputs(solve).Contains(Example))
        inputs.Add(Example);

    if (day.AvailableInputs(solve).Contains(Regular))
        inputs.Add(Regular);

    var chosenInput = Regular;
    if (inputs.Any())
        chosenInput = Prompt.Select("Choose input to run", inputs, defaultValue: inputs.Last());

    return chosenInput;
}
