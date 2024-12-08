namespace AdventOfCode._2024;

public class Day7 : Day
{
    public Day7() : base()
    {
    }

    [Answer("3749", Example, Data = "190: 10 19{nl}3267: 81 40 27{nl}83: 17 5{nl}156: 15 6{nl}7290: 6 8 6 15{nl}161011: 16 10 13{nl}192: 17 8 14{nl}21037: 9 7 18 13{nl}292: 11 6 16 20")]
    [Answer("850435817339", Regular)]
    public override string SolveA()
    {
        return Solve([Action.Plus, Action.Multiply]).ToString();
    }

    [Answer("11387", Example, Data = "190: 10 19{nl}3267: 81 40 27{nl}83: 17 5{nl}156: 15 6{nl}7290: 6 8 6 15{nl}161011: 16 10 13{nl}192: 17 8 14{nl}21037: 9 7 18 13{nl}292: 11 6 16 20")]
    [Answer("104824810233437", Regular)]
    public override string SolveB()
    {
        return Solve([Action.Plus, Action.Multiply, Action.Concatenate]).ToString();
    }

    private long Solve(List<Action> actions)
    {
        var equations = Parse();

        long total = 0;

        foreach (var equation in equations)
        {
            if (Loop(equation.Numbers, actions).Contains(equation.TestValue))
                total += equation.TestValue;
        }

        return total;
    }

    private static List<long> Loop(List<long> numbers, List<Action> actions)
    {
        var result = new List<long>();

        foreach (var action in actions)
        {
            if (numbers.Count == 1)
                return [numbers[0]];

            var current = numbers[0];
            var next = numbers[1];
            var calculate = Calculate(action, current, next);

            var tmp = numbers[2..];
            tmp.Insert(0, calculate);

            result.AddRange(Loop(tmp, actions));
        }

        return result;
    }

    private static long Calculate(Action action, long current, long next)
    {
        return action switch
        {
            Action.Plus => current + next,
            Action.Multiply => current * next,
            Action.Concatenate => long.Parse($"{current}{next}"), // TODO replace by a faster method
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private List<Equation> Parse()
    {
        return GetSplitInput()
            .Select(s =>
            {
                var split = s.Split(":", StringSplitOptions.TrimEntries);
                var numbers = split[1]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse)
                    .ToList();

                return new Equation(long.Parse(split[0]), numbers);
            }).ToList();
    }

    private enum Action
    {
        Plus,
        Multiply,
        Concatenate
    }

    private record struct Equation(long TestValue, List<long> Numbers);
}
