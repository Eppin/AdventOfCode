namespace AdventOfCode._2015;

public class Day5 : Day
{
    public Day5() : base()
    {
    }

    public override string SolveA()
    {
        var disallowed = new[] { "ab", "cd", "pq", "xy" };
        var vowels = new[] { 'a', 'e', 'i', 'o', 'u' };
        var nice = 0;

        foreach (var line in SplitInput)
        {
            if (disallowed.Any(d => line.Contains(d)))
                continue;

            var hasDouble = false;
            for (var i = 0; i < line.Length; i++)
            {
                if (i + 1 >= line.Length || line[i] != line[i + 1])
                    continue;

                hasDouble = true;
                break;
            }

            var vowelCount = line.Count(c => vowels.Contains(c));

            if (hasDouble && vowelCount >= 3)
                nice++;
        }

        return nice.ToString();
    }

    public override string SolveB()
    {
        var nice = 0;

        foreach (var line in SplitInput)
        {
            var hasPair = false;
            var hasRepeating = false;
            for (var i = 0; i < line.Length; i++)
            {
                // Find pair
                if (i + 1 < line.Length)
                {
                    var search = $"{line[i]}{line[i + 1]}";
                    if (Regex.Matches(line, search).Count > 1) // find both, this only finds 1
                        hasPair = true;
                }
                
                // Find repeat
                if (i + 2 < line.Length && line[i] == line[i + 2])
                    hasRepeating = true;
            }

            if (hasPair && hasRepeating)
                nice++;
        }

        return nice.ToString();
    }
}