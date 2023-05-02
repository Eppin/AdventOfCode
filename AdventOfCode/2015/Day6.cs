namespace AdventOfCode._2015;

using System.Data;
using System.Text.RegularExpressions;

public partial class Day6 : Day
{
    public Day6() : base()
    {
    }

    public override string SolveA()
    {
        var lights = new Dictionary<(int, int), int>();

        foreach (var input in SplitInput)
        {
            var action = ParseLine(input, out var x1, out var x2, out var y1, out var y2);

            for (var i = x1; i <= x2; i++)
            {
                for (var j = y1; j <= y2; j++)
                {
                    if (lights.TryGetValue((i, j), out var state))
                        lights[(i, j)] = ExecuteA(action, state);
                    else
                        lights.Add((i, j), ExecuteA(action, 0));
                }
            }
        }

        return lights.Count(l => l.Value == 1).ToString();
    }

    public override string SolveB()
    {
        var lights = new Dictionary<(int, int), int>();

        foreach (var input in SplitInput)
        {
            var action = ParseLine(input, out var x1, out var x2, out var y1, out var y2);

            for (var i = x1; i <= x2; i++)
            {
                for (var j = y1; j <= y2; j++)
                {
                    if (lights.TryGetValue((i, j), out var state))
                        lights[(i, j)] = ExecuteB(action, state);
                    else
                        lights.Add((i, j), ExecuteB(action, 0));
                }
            }
        }

        return lights.Sum(l => l.Value).ToString();
    }

    private static Action ParseLine(string input, out int x1, out int x2, out int y1, out int y2)
    {
        x1 = 0;
        x2 = 0;
        y1 = 0;
        y2 = 0;

        var action = Action.None;

        var onOrOffMatch = OnOrOffRegex().Match(input);
        if (onOrOffMatch.Success)
        {
            action = onOrOffMatch.Groups[1].Value.Equals("on")
                ? Action.On
                : Action.Off;

            if (!int.TryParse(onOrOffMatch.Groups[2].Value, out x1) || !int.TryParse(onOrOffMatch.Groups[3].Value, out y1) ||
                !int.TryParse(onOrOffMatch.Groups[4].Value, out x2) || !int.TryParse(onOrOffMatch.Groups[5].Value, out y2))
                throw new DataException("Couldn't convert string to int");
        }

        var toggleMatch = ToggleRegex().Match(input);
        if (toggleMatch.Success)
        {
            action = Action.Toggle;

            if (!int.TryParse(toggleMatch.Groups[1].Value, out x1) || !int.TryParse(toggleMatch.Groups[2].Value, out y1) ||
                !int.TryParse(toggleMatch.Groups[3].Value, out x2) || !int.TryParse(toggleMatch.Groups[4].Value, out y2))
                throw new DataException("Couldn't convert string to int");
        }

        return action;
    }

    private static int ExecuteA(Action action, int state)
    {
        return action switch
        {
            Action.On => 1,
            Action.Off => 0,
            Action.Toggle => state == 0 ? 1 : 0,
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
    }

    private static int ExecuteB(Action action, int state)
    {
        return action switch
        {
            Action.On => state + 1,
            Action.Off => Math.Max(state - 1, 0),
            Action.Toggle => state + 2,
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
    }

    private enum Action
    {
        None,
        On,
        Off,
        Toggle
    }

    [GeneratedRegex("turn (on|off) (\\d+),(\\d+) through (\\d+),(\\d+)")]
    private static partial Regex OnOrOffRegex();

    [GeneratedRegex("toggle (\\d+),(\\d+) through (\\d+),(\\d+)")]
    private static partial Regex ToggleRegex();
}