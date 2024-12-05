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
    [Answer("3062", Regular)]
    public override string SolveB()
    {
        var total = 0;

        var (rules, updates) = Parse();
        var incorrects = Solve(rules, updates).Where(s => !s.Correct);

        foreach (var incorrect in incorrects)
        {
            do
            {
                var result = Solve(rules, [incorrect.Update]).First();

                if (result.Correct)
                {
                    total += result.Update[result.Update.Count / 2];
                    break;
                }

                var iRule0 = incorrect.Update.IndexOf(result.Rule![0]);
                var iRule1 = incorrect.Update.IndexOf(result.Rule[1]);

                (incorrect.Update[iRule0], incorrect.Update[iRule1]) = (incorrect.Update[iRule1], incorrect.Update[iRule0]);
            } while (true);
        }

        return total.ToString();
    }

    private static IEnumerable<Result> Solve(List<List<int>> rules, List<List<int>> updates)
    {
        foreach (var update in updates)
        {
            var correct = true;
            List<int>? ruleHit = null;

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
                        ruleHit ??= rule;
                    }
                }

                if (!state.Contains(false)) continue;

                correct = false;
                break;
            }

            yield return new(correct, ruleHit, update);
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

    private record Result(bool Correct, List<int>? Rule, List<int> Update);
}
