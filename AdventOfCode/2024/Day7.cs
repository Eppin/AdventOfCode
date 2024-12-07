namespace AdventOfCode._2024;

public class Day7 : Day
{
    public Day7() : base()
    {
    }

    // [Answer("3749", Example, Data = "156802: 957 23 8 2 5")]
    [Answer("3749", Example, Data = "190: 10 19{nl}3267: 81 40 27{nl}83: 17 5{nl}156: 15 6{nl}7290: 6 8 6 15{nl}161011: 16 10 13{nl}192: 17 8 14{nl}21037: 9 7 18 13{nl}292: 11 6 16 20")]
    public override string SolveA()
    {
        return Solve().ToString();
    }

    public override string SolveB()
    {
        throw new NotImplementedException();
    }

    private long Solve()
    {
        var equations = Parse();

        long total = 0;
        foreach (var equation in equations)
        {
            var solve = Solve2(equation.Numbers);

            foreach (var so in solve)
            {
                Console.WriteLine($"L:{so.Item1} -> TV:{so.Item2} = {so.Item1 == equation.Numbers.Count - 1}");
            }

            var r = solve.Any(s => s.Item2 == equation.TestValue);
            if (r) total += equation.TestValue;
        }

        return total;
    }

    private static List<(int, int)> Solve2(List<int> numbers)
    {
        var results = new List<(int, int)>();

        foreach (var action in Enum.GetValues<Action>())
        {
            Console.WriteLine($"Action: {action}, C:[{numbers[0]}], N:[{numbers[1]}]");
            var calculate = Calculate(action, numbers[0], numbers[1]);

            // next level, recursive
            var numbers2 = numbers.Skip(2).ToList();

            if (numbers2.Count == 0)
            {
                // Done!
                results.Add((0, calculate));
                continue;
            }

            numbers2.Insert(0, calculate);

            var r = Solve2(numbers2);
            results.AddRange(r);
        }

        return results;
    }

    private static int Calculate(Action action, int current, int next)
    {
        return action switch
        {
            Action.Plus => current + next,
            Action.Multiply => current * next,
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
                    .Select(int.Parse)
                    .ToList();

                return new Equation(long.Parse(split[0]), numbers);
            }).ToList();
    }

    private enum Action
    {
        Plus,
        Multiply
    }

    private record struct Equation(long TestValue, List<int> Numbers);
}
