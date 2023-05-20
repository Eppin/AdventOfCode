namespace AdventOfCode._2015;

using System.Diagnostics;

public class Day10 : Day
{
    public Day10() : base()
    {
    }

    public override string SolveA()
    {
        return $"{Solve(Input, 40)}";
    }

    public override string SolveB()
    {
        return $"{Solve(Input, 50)}";
    }

    private static int Solve(string input, int processTimes)
    {
        for (var i = 0; i < processTimes; i++)
        {
            var sw = Stopwatch.StartNew();
            var groups = Parse(input);
            Console.WriteLine($"{i}: {sw.Elapsed}");
            sw.Restart();
            
            
            // var newInput = groups.Aggregate(string.Empty, (current, group) => $"{current}{group.Length}{group[0]}");

            var newInput = string.Empty;
            foreach (var group in groups)
            {
                newInput = $"{newInput}{group.Length}{group[0]}";
            }
            
            sw.Stop();
            Console.WriteLine($"{i}: {newInput.Length}, {sw.Elapsed}");
            input = newInput;
        }

        return input.Length;
    }

    private static IEnumerable<string> Parse(string input)
    {
        var groups = new List<string>();

        var group = string.Empty;
        for (var i = 0; i <= input.Length - 1; i++)
        {
            if (i == input.Length - 1)
            {
                if (group.Contains(input[i]))
                {
                    group += input[i];
                    groups.Add(group);
                }
                else
                {
                    group = $"{input[i]}";
                    groups.Add(group);
                }
            }
            else if (input[i] == input[i + 1])
                group += input[i];
            else
            {
                group += input[i];
                groups.Add(group);
                group = string.Empty;
            }
        }

        return groups;
    }
}
