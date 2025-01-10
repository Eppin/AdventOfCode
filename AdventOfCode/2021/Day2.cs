namespace AdventOfCode._2021;

public class Day2 : Day
{
    public Day2() : base()
    {
    }

    [Answer("1604850", Regular)]
    public override object SolveA()
    {
        var commands = GetCommands();

        var horizontal = 0;
        var depth = 0;

        foreach (var (command, unit) in commands)
        {
            switch (command.ToLowerInvariant())
            {
                case "forward":
                    horizontal += unit;
                    break;

                case "down":
                    depth += unit;
                    break;

                case "up":
                    depth -= unit;
                    break;
            }
        }

        return $"{horizontal * depth}";
    }

    [Answer("1685186100", Regular)]
    public override object SolveB()
    {
        var commands = GetCommands();

        var horizontal = 0;
        var depth = 0;
        var aim = 0;

        foreach (var (command, unit) in commands)
        {
            switch (command.ToLowerInvariant())
            {
                case "forward":
                    horizontal += unit;
                    depth += aim * unit;
                    break;

                case "down":
                    aim += unit;
                    break;

                case "up":
                    aim -= unit;
                    break;
            }
        }

        return $"{horizontal * depth}";
    }

    private IEnumerable<(string, int)> GetCommands()
    {
        return GetSplitInput()
            .Select(i => i.Split(' ', StringSplitOptions.TrimEntries))
            .Select(split => (split[0], int.Parse(split[1])));
    }
}
