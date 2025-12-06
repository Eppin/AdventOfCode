namespace AdventOfCode._2025;

public partial class Day6 : Day
{
    public Day6() : base()
    {
    }

    [Answer("4277556", Example, Data = "123 328  51 64 {nl} 45 64  387 23 {nl}  6 98  215 314{nl}*   +   *   +  ")]
    [Answer("6635273135233", Regular)]
    public override object SolveA()
    {
        var (numbers, operators) = ParsePartA();
        var sum = 0L;

        foreach (var (index, values) in numbers)
        {
            sum += values.Aggregate((a, b) =>
            {
                return operators[index] switch
                {
                    "+" => a + b,
                    "*" => a * b,
                    _ => throw new InvalidOperationException()
                };
            });
        }

        return sum;
    }

    [Answer("3263827", Example, Data = "123 328  51 64 {nl} 45 64  387 23 {nl}  6 98  215 314{nl}*   +   *   +  ")]
    [Answer("12542543681221", Regular)]
    public override object SolveB()
    {
        var (numbers, operators) = ParsePartB();
        var sum = 0L;

        foreach (var (index, values) in numbers)
        {
            sum += values.Aggregate((a, b) =>
            {
                return operators[index] switch
                {
                    "+" => a + b,
                    "*" => a * b,
                    _ => throw new InvalidOperationException()
                };
            });
        }
        
        return sum;
    }

    private (Dictionary<long, List<long>> Numbers, string[] Operators) ParsePartA()
    {
        var input = SplitInput.ToArray();

        var numbers = new Dictionary<long, List<long>>();

        for (var i = 0; i < input.Length - 1; i++)
        {
            var matches = NumericRegex().Matches(input[i]);

            for (var j = 0; j < matches.Count; j++)
            {
                var value = long.Parse(matches[j].Value);

                if (!numbers.TryAdd(j, [value])) numbers[j].Add(value);
            }
        }

        var last = input.Last();
        var operators = OperatorRegex()
            .Matches(last)
            .Select(m => m.Value)
            .ToArray();

        return (numbers, operators);
    }
    
    private (Dictionary<long, List<long>> Numbers, string[] Operators) ParsePartB()
    {
        var input = GetSplitInput(trimEntries: false).ToArray();
        var numbers = new Dictionary<long, List<long>>();

        var length = input.Max(s => s.Length);
        var index = 0;

        for (var i = 0; i < length; i++)
        {
            var chars = input
                .SkipLast(1)
                .Select(n => n[i])
                .Where(s => s != 32) // 33 is ' ' (space)
                .ToArray();

            if (chars.Length > 0)
            {
                var value = long.Parse(string.Join("", chars));
                if (!numbers.TryAdd(index, [value])) numbers[index].Add(value);
            }
            else
            {
                index++;
            }
        }

        var last = input.Last();
        var operators = OperatorRegex()
            .Matches(last)
            .Select(m => m.Value)
            .ToArray();

        return (numbers, operators);
    }

    [GeneratedRegex(@"(\d+)")]
    private static partial Regex NumericRegex();

    [GeneratedRegex(@"(\*|\+)")]
    private static partial Regex OperatorRegex();
}
