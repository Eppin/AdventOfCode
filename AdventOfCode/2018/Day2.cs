namespace AdventOfCode._2018;

public class Day2 : Day
{
    public Day2() : base()
    {
    }

    [Answer("6370", Regular)]
    public override string SolveA()
    {
        int found2Times = 0, found3Times = 0;

        foreach (var s in SplitInput)
        {
            var charCount = new Dictionary<char, int>();

            foreach (var c in s.Where(c => !charCount.TryAdd(c, 1)))
                charCount[c]++;

            if (charCount.Values.Any(count => count == 2))
                found2Times++;

            if (charCount.Values.Any(count => count == 3))
                found3Times++;
        }

        return $"{found2Times * found3Times}";
    }

    [Answer("rmyxgdlihczskunpfijqcebtv", Regular)]
    public override string SolveB()
    {
        foreach (var s in SplitInput)
        {
            var inputExcludeS = SplitInput.Where(x => x != s);
            foreach (var s1 in inputExcludeS)
            {
                var wordMatchCount = 0;
                var result = "";
                for (var i = 0; i < s1.Length; i++)
                {
                    if (s[i] != s1[i])
                        continue;

                    result += s[i];
                    wordMatchCount++;
                }

                if (wordMatchCount == s1.Length - 1)
                    return result;
            }
        }

        throw new InvalidOperationException("No matching words found");
    }
}
