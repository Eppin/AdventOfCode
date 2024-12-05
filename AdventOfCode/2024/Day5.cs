using AdventOfCode.Models;

namespace AdventOfCode._2024;

public class Day5 : Day
{
    public Day5() : base()
    {
    }

    [Answer("143", Example, Data = "47|53{nl}97|13{nl}97|61{nl}97|47{nl}75|29{nl}61|13{nl}75|53{nl}29|13{nl}97|29{nl}53|29{nl}61|53{nl}97|53{nl}61|29{nl}47|13{nl}75|47{nl}97|75{nl}47|61{nl}75|61{nl}47|29{nl}75|13{nl}53|13{nl}{nl}75,47,61,53,29{nl}97,61,53,29,13{nl}75,29,13{nl}75,97,47,61,53{nl}61,13,29{nl}97,13,75,29,47")]
    [Answer("5948", Regular)]
    public override string SolveA()
    {
        var (rules, updates) = Parse();

        return Solve(rules, updates)
            .Where(s => s.Correct)
            .Select(s => s.Update[s.Update.Count / 2])
            .Sum()
            .ToString();
    }

    [Answer("123", Example, Data = "47|53{nl}97|13{nl}97|61{nl}97|47{nl}75|29{nl}61|13{nl}75|53{nl}29|13{nl}97|29{nl}53|29{nl}61|53{nl}97|53{nl}61|29{nl}47|13{nl}75|47{nl}97|75{nl}47|61{nl}75|61{nl}47|29{nl}75|13{nl}53|13{nl}{nl}75,47,61,53,29{nl}97,61,53,29,13{nl}75,29,13{nl}75,97,47,61,53{nl}61,13,29{nl}97,13,75,29,47")]
    public override string SolveB()
    {
        var total = 0;

        var (rules, updates) = Parse();
        var falseUpdates = Solve(rules, updates).Where(s => !s.Correct);



        foreach (var result in falseUpdates.Skip(2))
        {
            Console.WriteLine();
            var k = Solve(rules, [result.Update]);

            foreach (var j in k)
            {
                Console.WriteLine($"-\t{j.Correct}, {j.Index}, {string.Join(',', j.Update)}");
            }

            //Result? result1;
            //do
            //{
            //    var y = result.Update.ToArray().Permutations().ToList();

            //    var x = result.Update.ToArray().Permutations().Select(y => y.ToList()).ToList();

            //    result1 = Solve(rules, x).FirstOrDefault(s => s.Correct);
            //} while (result1 == null);

            //total += result1.Update[result1.Update.Count / 2];

            ////////////////

            //List<int> tmp = [.. update[..index], .. update[(index + 1)..]];
            //tmp.Insert(0, update[index]);

            //var solved1 = Solve(rules, [tmp]);
            //var solved = solved1.FirstOrDefault(s => s.Correct);

            //var update = new List<int> { 97, 75, 29, 13, 47 };// result.Update;
            //var index = 2;
            //var lastIndex = result.Index;

            //for (var i = 0; i < update.Count; i++)
            //{
            //    List<int> tmp = [.. update[..index], .. update[(index + 1)..]];
            //    tmp.Insert(i, update[index]);

            //    var solved = Solve(rules, [tmp]).First();
            //    //lastIndex = solved.Index


            //    //var solved = solved1.FirstOrDefault(s => s.Correct);

            //    Console.WriteLine($"{string.Join(',', tmp)} -> {solved.Correct}={solved.Index}");

            //    //if (solved != null)
            //    //{
            //    //    total += solved.Update[solved.Update.Count / 2];
            //    //    break;
            //    //}
            //}
        }

        return total.ToString();
    }

    private static IEnumerable<Result> Solve(List<List<int>> rules, List<List<int>> updates)
    {
        foreach (var update in updates)
        {
            var good1 = true;
            var index = 0;

            for (var i = 0; i < update.Count; i++)
            {
                var state = new List<bool>();

                var value = update[i];
                var vRules = rules.Where(r => r[0] == value || r[1] == value);

                foreach (var rule in vRules)
                {
                    var iRule0 = update.IndexOf(rule[0]);
                    var iRule1 = update.IndexOf(rule[1]);

                    // Check if value comes before
                    if (value == rule[0] && (i < iRule1 || !update.Contains(rule[1])))
                        state.Add(true);
                    else if (value == rule[1] && (i > iRule0 || !update.Contains(rule[0])))
                        state.Add(true);
                    else
                    {
                        state.Add(false);
                        Console.WriteLine($"{rule[0]}/{rule[1]} => {value}");
                    }
                }

                if (!state.Contains(false)) continue;

                good1 = false;
                index = i;
                break;
            }

            yield return new(good1, index, update);
        }
    }

    private (List<List<int>> Rules, List<List<int>> Updates) Parse()
    {
        var split = GetSplitInput(false).ToList();

        var empty = split.IndexOf(string.Empty);

        var rules = split
            .Take(empty)
            .Select(r => r
                .Split('|')
                .Select(int.Parse)
                .ToList()
            ).ToList();

        var updates = split
            .Skip(empty + 1)
            .Select(r => r
                .Split(',')
                .Select(int.Parse)
                .ToList()
            ).ToList();

        return (rules, updates);
    }

    private record Result(bool Correct, int Index, List<int> Update);
}
